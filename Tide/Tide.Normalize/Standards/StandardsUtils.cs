using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Data.Models;
using Tide.Data.Models.Standards;

namespace Tide.Normalize.Standards
{
    public static class StandardsUtils
    {
        private static readonly IDictionary<string, Standard> __standards = new ConcurrentDictionary<string, Standard>();

        private static Standard? Fetch(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            if (code.StartsWith(" ") || code.EndsWith(" "))
                code = code.Trim();
            if (__standards.ContainsKey(code)) return __standards[code];
            else
            {
                using var db = Context.Db;
                var standard = db.Standards.FirstOrDefault(x => x.Name == code);

                if (standard == null) return null;

                __standards.Add(code, standard);
                return standard;
            }
        }

        public static bool Contains(string code) => Fetch(code) != null;
        public static Standard Get(string code) => Fetch(code) ?? throw new KeyNotFoundException(code);
        public static IReadOnlyCollection<Standard> Get(IEnumerable<string> codes)
        {
            List<Standard> standards = new();
            foreach (var code in codes)
                if(Contains(code))
                    standards.Add(Get(code));

            return standards;
        }
        public static IReadOnlyCollection<Standard> Standards()
        {
            string path = Path.Combine(Context.Path, "standards.json");
            string text = File.ReadAllText(path);

            var models = JsonConvert.DeserializeObject<List<Model>>(text) ?? new();
            return models.Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Title) && !string.IsNullOrWhiteSpace(x.Type)).Select(x => new Standard()
            {
                Name = x.Name.Trim(),
                Title = x.Title.Trim(),
                Type = x.Type.Trim(),
            }).ToList();
        }
        public static void Save()
        {
            var nations = Standards();
            using var context = Context.Db;
            context.Standards.AddRange(nations);
            context.SaveChanges();
        }

        private class Model
        {
            public string Name { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
        }
    }
}
