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
    internal class FocusAreaParser : Commons.BaseParser
    {
        public FocusAreaParser(string inputDirectory, string outputDirectory, string cycle) : base(inputDirectory, outputDirectory, cycle, "fa")
        {
        }


        public override (string Id, JObject Data) ParseFile(string idToken, HtmlDocument document)
        {
            JObject result = new JObject();
            JArray objectivesIds = new JArray();
            JArray capabilitiesIds = new JArray();

            string? id = idToken.Replace("_Focus_Area", string.Empty);
                
            result["id"] = id;
            result["cycle"] = Cycle;
            result["name"] = document.DocumentNode.SelectSingleNode("//div[@class='banner-center']")?.InnerText.EscapeString(); ;
            result["description"] = document.DocumentNode.SelectSingleNode("//div[text()='Description']/..").ChildNodes[3].InnerText.EscapeString();

            var objectivesTab = GetTabNode(document.DocumentNode, row: 4, column: 2);

            result["objectiveids"] = new JArray(ParseTable<string>(objectivesTab, row =>
            {
                return row.SelectSingleNode(".//td").SelectSingleNode(".//a").Attributes["title"].Value;
            }));

            var capabilitiesTab = GetTabNode(document.DocumentNode, row: 4, column: 1);
            
            result["capabilitieids"] = new JArray(ParseTable<string>(capabilitiesTab, row =>
            {
                return row.SelectSingleNode(".//td").SelectSingleNode(".//a").Attributes["title"].Value;
            })); ;


            return new(id, result);
        }


 
    }
}
