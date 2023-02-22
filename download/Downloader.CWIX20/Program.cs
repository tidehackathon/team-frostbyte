using Downloader.Commons;
using Newtonsoft.Json.Linq;
using System;

string cycle = "cwix20";
string authenticationUrl = "https://tide.act.nato.int/login?url=%2Fmediawiki%2Fcwix20%2Findex.php%2FCWIX_2020";
string baseAddress = "https://tide.act.nato.int";
string username = "<<tide_username>>";
string password = "<<tide_password>>";

string schemaFile = Utils.CompatiblePath(@$"..\..\..\..\..\..\data\rules\{cycle}.json");
string outputDirectory = Utils.CompatiblePath(@"..\..\..\..\..\..\data\raw");
string ttDirectory = Utils.CreateIfNotExist(Utils.CompatiblePath(@$"{cycle}\tt"), outputDirectory);
string tcDirectory = Utils.CreateIfNotExist(Utils.CompatiblePath(@$"{cycle}\tc"), outputDirectory);
string ccDirectory = Utils.CreateIfNotExist(Utils.CompatiblePath(@$"{cycle}\cc"), outputDirectory);
string faDirectory = Utils.CreateIfNotExist(Utils.CompatiblePath(@$"{cycle}\fa"), outputDirectory);
string obDirectory = Utils.CreateIfNotExist(Utils.CompatiblePath(@$"{cycle}\ob"), outputDirectory);
string orgDirectory = Utils.CreateIfNotExist(Utils.CompatiblePath(@$"{cycle}\org"), outputDirectory);


JObject schema = JObject.Parse(File.ReadAllText(schemaFile));

string[] ttUrls = await BuildTTUrls();
//string[] tcUrls = await BuildTCUrls();
//string[] ccUrls = await BuildCCUrls();
//string[] faUrls = await BuildFAUrls();
//string[] obUrls = await BuildOBUrls();
//string[] orgUrls = await BuildORGUrls();

var client = await Utils.GetClient(baseAddress, authenticationUrl, username, password);

//Utils.PrintUrls(ttUrls);
//Utils.PrintUrls(tcUrls);
//Utils.PrintUrls(ccUrls);
//Utils.PrintUrls(faUrls);
//Utils.PrintUrls(obUrls);
//Utils.PrintUrls(orgUrls);




await Utils.Download(ttUrls, ttDirectory, client);
//await Utils.Download(tcUrls, tcDirectory, client);
//await Utils.Download(ccUrls, ccDirectory, client);
//await Utils.Download(faUrls, faDirectory, client);
//await Utils.Download(obUrls, obDirectory, client);
//await Utils.Download(orgUrls, orgDirectory, client);



async Task<string[]> BuildTTUrls()
{
    List<string> result = new List<string>();

    string baseUrl = schema["TT"]!["url"]!.ToString();

    (int Start, int End)[] ranges = Utils.ParseRangeString(schema["TT"]!["IDENTIFIER_range"]!.ToString());

    int pad = 5;

    foreach (var range in ranges)
    {
        string[] urls = Enumerable.Range(range.Start, range.End - range.Start).Select(identifier =>
        {
            string id = identifier.ToString().PadLeft(pad, '0');

            string url = baseUrl.Replace("{{IDENTIFIER}}", id);

            return url;
        }).ToArray();

        result.AddRange(urls);
    }

    return result.ToArray();
}

// TODO! - Remove comment after tested and successful!
async Task<string[]> BuildTCUrls()
{
    List<string> result = new List<string>();

    string baseUrl = schema["TC"]!["url"]!.ToString();

    (int Start, int End)[] ranges = Utils.ParseRangeString(schema["TC"]!["IDENTIFIER_range"]!.ToString());

    int pad = 5;

    foreach (var range in ranges)
    {
        string[] urls = Enumerable.Range(range.Start, range.End - range.Start).Select(identifier =>
        {
            string id = identifier.ToString().PadLeft(pad, '0');

            string url = baseUrl.Replace("{{IDENTIFIER}}", id);

            return url;
        }).ToArray();

        result.AddRange(urls);
    }

    return result.ToArray();
}

// TODO! - Remove comment after tested and successful!
async Task<string[]> BuildCCUrls()
{
    List<string> result = new List<string>();

    string baseUrl = schema["CC"]!["url"]!.ToString();

    (int Start, int End)[] ranges = Utils.ParseRangeString(schema["CC"]!["IDENTIFIER_range"]!.ToString());

    int pad = 3;

    foreach (var range in ranges)
    {
        string[] urls = Enumerable.Range(range.Start, range.End - range.Start).Select(identifier =>
        {
            string id = identifier.ToString().PadLeft(pad, '0');

            string url = baseUrl.Replace("{{IDENTIFIER}}", id);

            return url;
        }).ToArray();

        result.AddRange(urls);
    }

    return result.ToArray();
}

// TODO! - Remove comment after tested and successful!
async Task<string[]> BuildFAUrls()
{
    List<string> result = new List<string>();

    string baseUrl = schema["FA"]!["url"]!.ToString();

    string[] values = Utils.ParseValueString(schema["FA"]!["IDENTIFIER_values"]!.ToString());

    foreach (var value in values)
        result.Add(baseUrl.Replace("{{IDENTIFIER}}", value));

    return result.ToArray();
}

// TODO! - Remove comment after tested and successful!
async Task<string[]> BuildOBUrls()
{
    List<string> result = new List<string>();

    string baseUrl = schema["OB"]!["url"]!.ToString();

    // FA Identifiers
    string[] values = Utils.ParseValueString(schema["OB"]!["IDENTIFIER_values"]!.ToString());

    // Identifier ranges
    (int Start, int End)[] ranges = Utils.ParseRangeString(schema["OB"]!["IDENTIFIER_range"]!.ToString());

    for (int i = 0; i < values.Length; i++)
    {
        var range = ranges[i];

        string[] urls = Enumerable.Range(range.Start, range.End - range.Start).Select(identifier =>
        {
            string id = identifier.ToString();

            string url = baseUrl.Replace("{{IDENTIFIER}}", id).Replace("{{FA_IDENTIFIER}}", values[i]);

            return url;
        }).ToArray();

        result.AddRange(urls);

    }
    return result.ToArray();
}

// TODO! - Remove comment after tested and successful!
async Task<string[]> BuildORGUrls()
{
    List<string> result = new List<string>();

    string baseUrl = schema["ORG"]!["url"]!.ToString();

    string[] values = Utils.ParseValueString(schema["ORG"]!["IDENTIFIER_values"]!.ToString());

    foreach (var value in values)
        result.Add(baseUrl.Replace("{{IDENTIFIER}}", value));

    return result.ToArray();
}

