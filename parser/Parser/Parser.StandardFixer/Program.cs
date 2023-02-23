///<summary>
/// This script will fix the standards names.
/// </summary>


using Newtonsoft.Json.Linq;
using Parser.Commons;

string cycle = "cwix19";

string ccDirectory = $@"D:\BlackOps\FrostByteRepo\frostbyte\data\normalized\{cycle}\cc";
string objDirectory = $@"D:\BlackOps\FrostByteRepo\frostbyte\data\normalized\{cycle}\ob";
string ttDirectory = $@"D:\BlackOps\FrostByteRepo\frostbyte\data\normalized\{cycle}\tt";


ParseDirectory(ccDirectory, "standards");
//ParseDirectory(objDirectory, "relevantstandards");
//ParseDirectory(ttDirectory, "standards");


void ParseDirectory(string directory, string key)
{
    foreach (var file in Directory.GetFiles(directory))
    {
        JObject data = JObject.Parse(File.ReadAllText(file));

        CheckData(data, key);

        File.WriteAllText(file, data.ToString(Newtonsoft.Json.Formatting.Indented));

        Utils.WriteOutcome($"Parsed {file}");
    }
}

void CheckData(JObject item, string standardKey)
{
    var standards = item[standardKey] as JArray;

    var replacement = new JArray();

    foreach (var standard in standards)
    {

        string standardString = standard.ToString().Replace("NISP Standard - ", string.Empty)
                                                   .Replace("NISP Coverdoc - ", string.Empty)
                                                   .Replace("GEOINT - ", string.Empty)
                                                   .Trim();

        replacement.Add(standardString);
    }

    item[standardKey] = replacement;
}
