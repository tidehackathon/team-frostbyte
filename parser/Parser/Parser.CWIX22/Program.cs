using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Parser.CWIX22;

const string cycle = "cwix22";
const string year = "2022";

File.Create("log.txt");

string inputDirectory = @$"..\..\..\..\..\..\..\data\raw\{cycle}";
string outputDirectory = @$"..\..\..\..\..\..\..\data\normalized\{cycle}";


FocusAreaParser faParser = new FocusAreaParser(inputDirectory, outputDirectory, year);
TestCaseParser tcParser = new TestCaseParser(inputDirectory,outputDirectory, year);

// Parsing
//await faParser.Parse();
await tcParser.Parse();



// Test data

HtmlDocument document = new HtmlDocument();

//string fileName = "D:\\BlackOps\\FrostByteRepo\\frostbyte\\data\\raw\\cwix22\\fa\\https%3A%2F%2Ftide.act.nato.int%2Fmediawiki%2Fcwix22%2Findex.php%2FAir_Focus_Area.html";
string fileName = "D:\\BlackOps\\FrostByteRepo\\frostbyte\\data\\raw\\cwix22\\tc\\https%3A%2F%2Ftide.act.nato.int%2Fmediawiki%2Fcwix22%2Findex.php%2FTC-05708.html"; 
string idToken = Uri.UnescapeDataString(fileName).Split("/", StringSplitOptions.RemoveEmptyEntries).Last().Replace(".html", string.Empty);
document.Load(fileName);

tcParser.ParseFile(idToken, document);

