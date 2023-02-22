using Downloader.Commons;
using Newtonsoft.Json.Linq;
using System;

string authenticationUrl = "https://tide.act.nato.int/login?url=%2Fmediawiki%2Fcwix22%2Findex.php%2FCWIX_2022";
string baseAddress = "https://tide.act.nato.int";
string username = "<<tide_username>>";
string password = "<<tide_password>>";

var client = await Utils.GetClient(baseAddress, authenticationUrl, username, password);
string basePath = "..\\..\\..\\..\\..\\..\\data\\normalized";
string inputPath = $"{basePath}\\countries.json";
string outputDirectory = $"{basePath}\\flags";

JArray data = JArray.Parse(File.ReadAllText(inputPath));

if (!Directory.Exists(outputDirectory))
{
    Directory.CreateDirectory(outputDirectory);
}

foreach (var item in data)
{
    string url = item["url"]!.ToString();

    var flagDownloadResult = await client.GetAsync(url);

    var imageBytes = await flagDownloadResult.Content.ReadAsByteArrayAsync();

    var imagePath = Path.Combine(outputDirectory, $"{item["name"]}.png");

    File.WriteAllBytes(imagePath, imageBytes);
}