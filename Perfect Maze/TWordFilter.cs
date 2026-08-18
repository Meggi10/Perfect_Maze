using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public static class TWordFilter
    {
        private static List<string> forbiddenWords = new List<string>();
        public static void Load()
        {
            string json = TResourceReader.ReadManifestText("Data_.ForbiddenWords.json");
            var data = JsonSerializer.Deserialize<TForbiddenWordsData>(json);
            forbiddenWords = data?.ForbiddenWords ?? new List<string>();
        }
        public static bool IsForbidden(string nick)
        {
            if (string.IsNullOrEmpty(nick)) return false;
            return forbiddenWords.Any(word => nick.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
    public class TForbiddenWordsData
    {
        public List<string> ForbiddenWords { get; set; }
    }
}
