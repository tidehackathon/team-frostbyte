using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models;

namespace Tide.Normalize.Countries
{
    public static class CountryUtils
    {
        private static readonly IDictionary<string, Nation> __nations = new ConcurrentDictionary<string, Nation>();

        private static Nation? Fetch(string code)
        {
            if(string.IsNullOrWhiteSpace(code)) return null;
            if(code.StartsWith(" ")||code.EndsWith(" "))
                code = code.Trim();
            if(__nations.ContainsKey(code)) return __nations[code];
            else
            {
                using var db = Context.Db;
                var nation = db.Nations.FirstOrDefault(x => x.Name == code);

                if (nation == null) return null;

                __nations.Add(code, nation);
                return nation;
            }
        }

        public static bool Contains(string code) => Fetch(code) != null;
        public static Nation Get(string code)=> Fetch(code)?? throw new KeyNotFoundException(code);

        public static IReadOnlyCollection<Nation> Parse()
        {
            string path = Path.Combine(Context.Path, "countries.json");
            string text = File.ReadAllText(path);

            var models = JsonConvert.DeserializeObject<List<CountryModel>>(text) ?? new();
            return models.Where(x=>!string.IsNullOrWhiteSpace(x.Name)&&!string.IsNullOrWhiteSpace(x.Src)).Select(x => new Nation()
            {
                 Name=x.Name,
                 Logo=x.Src.Split("/", StringSplitOptions.RemoveEmptyEntries).Last().Replace("75px-","")
            }).ToList();
        }

        public static void Save()
        {
            var nations = Parse();
            using var context = Context.Db;
            context.Nations.AddRange(nations);
            context.SaveChanges();
        }

        private class CountryModel
        {
            [JsonProperty("name")]
            public string Name { get; set; } = string.Empty;
            [JsonProperty("url")]
            public string Src { get; set; } = string.Empty;
        }
    }
}
