using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Parser.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Commons
{
    public abstract class BaseParser
    {
        private readonly string _inputDirectory;
        private readonly string _outputDirectory;
        private const int maxDegreeOfParallelism = 1000;
        protected readonly string Cycle;
        
        public BaseParser(string inputDirectory, string outputDirectory, string cycle, string label)
        {
            _inputDirectory = Path.Combine(inputDirectory, label);
            _outputDirectory = Path.Combine(outputDirectory, label);
            Cycle = cycle;
        }

        public string FixId(string? id, int count = 5)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }

            string[] tokens = id.Split("-", StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0) return string.Empty;
            if (tokens.Length == 1) return tokens[0];

            return $"{tokens[0]}-{tokens[1].Substring(0, Math.Min(tokens[1].Length, count))}".Replace("link=", string.Empty);
        }

        public async Task Parse()
        {
            Utils.WriteOutcome("Starting parsing...");

            await Parallel.ForEachAsync(
                source: Directory.GetFiles(_inputDirectory),
                 parallelOptions: new ParallelOptions()
                 {
                     MaxDegreeOfParallelism = maxDegreeOfParallelism
                 },
                 body: async (file, ct) =>
                 {
                     string idToken = Uri.UnescapeDataString(file).Split("/", StringSplitOptions.RemoveEmptyEntries).Last().Replace(".html", string.Empty);

                     try
                     {
                         HtmlDocument htmlDocument = new HtmlDocument();

                         htmlDocument.Load(file);


                         (string Id, JObject Data) parseResult = ParseFile(idToken, htmlDocument);

                         string outputPath = Path.Combine(_outputDirectory, $"{parseResult.Id}.json");

                         File.AppendAllText(outputPath, parseResult.Data.ToString(Newtonsoft.Json.Formatting.Indented));

                         Utils.WriteOutcome($"Parsed {file}");
                     }
                     catch
                     {
                         Utils.WriteOutcome($"Failed to execute file {file}", false);

                         Utils.Log(idToken);
                     }
                 });
        }


        public abstract (string Id, JObject Data) ParseFile(string file, HtmlDocument document);

        protected HtmlNode GetTabNode(HtmlNode node, string identifier)
            => node.SelectNodes($"//div[@class='panel']")?.FirstOrDefault(node => node.SelectSingleNode($".//div[text()='{identifier}']") != null);

        protected HtmlNode GetTabNode(HtmlNode node, int row, int column, int? rowSpan = null, int? columnSpan = null)
        {
            var columnSelector = columnSpan == null ?
                $"grid-column:{column}" :
                $"grid-column:{column}/span {columnSpan}";

            var mscolumnSelector = columnSpan == null ?
                $"-ms-grid-column:{column}" :
                $"-ms-grid-column:{column}; -ms-grid-column-span:{columnSpan}";

            var rowSelector = rowSpan == null ?
                $"grid-row:{row}" :
                $"grid-row:{row}/span {rowSpan}";

            var msrowSelector = rowSpan == null ?
                $"-ms-grid-row:{row}" :
                $"-ms-grid-row:{row}; -ms-grid-row-span:{rowSpan}";


            var styleSelector = string.Join("; ", columnSelector, rowSelector, mscolumnSelector, msrowSelector) + ";";

            return node.SelectSingleNode($"//div[@style='{styleSelector}']");
        }

        protected List<T> ParseTable<T>(HtmlNode tab, Func<HtmlNode, T> rowAction, bool skipFirst = true, int? tableCount = null)
        {
            List<T> result = new List<T>();

            var tableNodes = tab.SelectNodes(".//table");

            if (tableNodes != null)
            {
                int count = 0;

                foreach (var tableNode in tableNodes)
                {
                    var rows = tableNode.SelectSingleNode(".//tbody").SelectNodes(".//tr").Skip(skipFirst ? 1 : 0);

                    result.AddRange(rows.Select(item => rowAction(item)).ToList().Where(item => item != null));

                    count++;

                    if (count == tableCount)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}
