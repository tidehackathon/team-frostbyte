using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Parser.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.CWIX22
{
    internal class TestCaseParser : Commons.BaseParser
    {
        public TestCaseParser(string inputDirectory, string outputDirectory, string cycle) : base(inputDirectory, outputDirectory, cycle, "tc")
        {
        }

        public override (string Id, JObject Data) ParseFile(string idToken, HtmlDocument document)
        {
            var result = new JObject();

            result["id"] = idToken;
            result["cycle"] = Cycle;

            if (!ParseReferences(document, result))
            {
                throw new ApplicationException($"Failed to parse {idToken}, no TT available");
            }
            ParseStatus(document, result);
            ParseParticipants(document, result);
            ParseObjectives(document, result);
            ParsePartialResults(document, result);
            ParseFinalResults(document, result);

            
            return (idToken, result);
        }


        private void ParseStatus(HtmlDocument document, JObject result)
        {
            HtmlNode objectivesTab = GetTabNode(document.DocumentNode, row: 1, column: 1);

            result["status"] = Utils.EscapeString(objectivesTab.SelectSingleNode(".//div[@class='banner-bottom-center']").SelectSingleNode(".//div").InnerText);
        }

        private void ParseFinalResults(HtmlDocument document, JObject result)
        {
            var secondTab = document.DocumentNode.SelectSingleNode("//div[@id='Final_Result']");

            var secondTabTable = secondTab.SelectNodes(".//table").First();

            result["finalresult"] = JObject.FromObject(new
            {
                result = Utils.EscapeString(GetNode(secondTabTable, "Result")?.InnerText),
                shortfallinvestigation = Utils.EscapeString(GetNode(secondTabTable, "Shortfall Investigation")?.InnerText) == "Yes" ? true : false,
                issuecategory = Utils.EscapeString(GetIssueCategoryNode(secondTabTable)?.InnerText).Replace("-", string.Empty).Trim(),
                remarks = Utils.EscapeString(GetNode(secondTabTable, "Remarks")?.InnerText)
            });

            HtmlNode GetNode(HtmlNode node, string identifier) => node.SelectSingleNode($".//th[text()='{identifier}']/..//td");

            HtmlNode GetIssueCategoryNode(HtmlNode node) => node.SelectSingleNode(".//a[@title='Issue Categories']/..//..//..//td");
        }

        private void ParsePartialResults(HtmlDocument document, JObject result)
        {
            var firstTab = document.DocumentNode.SelectSingleNode("//div[@id='Partial_Results']");

            var firstTabTables = firstTab.SelectNodes(".//table").Skip(1).ToList();

            JObject partialResult = new JObject();

            partialResult["providers"] = new JArray(ParseParticipantsTable(firstTabTables[0]));
            partialResult["consumers"] = new JArray(ParseParticipantsTable(firstTabTables[1]));
            partialResult["mediators"] = new JArray(ParseParticipantsTable(firstTabTables.Count > 2 ? firstTabTables[2] : null));

            result["partialresult"] = partialResult;

            List<JObject?> ParseParticipantsTable(HtmlNode? node)
            {
                if(node == null)
                {
                    return new List<JObject?>();
                }

                List<JObject?> providersData = ParseTable(
                   tab: node.ParentNode,
                   rowAction: row =>
                   {
                       var columns = row.SelectNodes(".//td");

                       string? id = columns[0].SelectSingleNode(".//a")?.Attributes["title"]?.Value;

                       if (id == null)
                       {
                           return null;
                       }

                       string status = columns[1].InnerText;
                       string? remarks = columns[2].InnerText;

                       return JObject.FromObject(new
                       {
                           id,
                           status,
                           remarks
                       });
                   });

                return providersData;
            }
        }

        private void ParseObjectives(HtmlDocument document, JObject result)
        {

            HtmlNode objectivesTab = GetTabNode(document.DocumentNode, row: 4, column: 1, columnSpan: 2);

            var data = ParseTable(
                tab: objectivesTab,
                rowAction: row =>
                {
                    return row.SelectSingleNode(".//a")?.Attributes["title"]?.Value;
                }
            );


            result["objectiveids"] = new JArray(data);
        }

        private void ParseParticipants(HtmlDocument document, JObject result)
        {
            HtmlNode participantsTab = GetTabNode(document.DocumentNode, row: 2, column: 2, rowSpan: 2);

            var data = ParseTable(
                tab: participantsTab,
                rowAction: row =>
                {
                    string category = Utils.EscapeString(row.SelectSingleNode(".//th").InnerText);
                    List<string> ids = row.SelectNodes(".//a")?.Select(anchor => anchor.Attributes["title"].Value).ToList() ?? new();

                    return new { ParticipantCategory = category, Ids = ids };
                },
                skipFirst: false);

            result["providerids"] = new JArray(data.FirstOrDefault(item => item.ParticipantCategory == "Providers")?.Ids ?? new List<string>());
            result["consumerids"] = new JArray(data.FirstOrDefault(item => item.ParticipantCategory == "Consumers")?.Ids ?? new List<string>());
            result["mediatorids"] = new JArray(data.FirstOrDefault(item => item.ParticipantCategory == "Mediators")?.Ids ?? new List<string>());
        }

        private bool ParseReferences(HtmlDocument document, JObject result)
        {
            HtmlNode generalTab = GetTabNode(document.DocumentNode, row: 2, column: 1);

             
            var ttData = ParseTable(
                    tab: generalTab,
                    rowAction: row =>
                    {
                        var headerNode = row.SelectSingleNode(".//th");
                        if (headerNode != null)
                        {
                            string category = Utils.EscapeString(headerNode.InnerText);

                            if (category == "Test Template")
                            {
                                List<string> ids = row.SelectNodes(".//a")?.Select(anchor => anchor.Attributes["title"].Value).ToList() ?? new();

                                var ttRowData = ParseTable(row,
                                    rowAction: innerRow =>
                                    {
                                        return FixId(innerRow.SelectSingleNode("..//a")?.Attributes["title"]?.Value, count: 5);
                                    });

                                if (ttRowData.Count == 0)
                                {
                                    return null;
                                }
                                else return ttRowData.First();
                            }
                        }

                        return null;
                    },
                    skipFirst: false,
                    tableCount: 1);

            if (ttData.Count > 0)
            {
                result["testtemplateid"] = ttData.First();

                return true;
            }

            return false;
        }
    }
}
