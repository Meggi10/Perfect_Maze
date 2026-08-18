using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public static class TNickName
    {
        private static List<string> nickName = new List<string>();
        private static readonly Random random = new Random();

        public static void Load()
        {
            string json = TResourceReader.ReadManifestText("Data_.NickNames.json");
            var data = JsonSerializer.Deserialize<TNickNameData>(json);
            nickName = data?.NickNames ?? new List<string>();
        }

        public static string GetRandomNickName()
        {
            if (nickName.Count == 0) return string.Empty;
            int index = random.Next(nickName.Count);
            return nickName[index];
        }
    }
    public class TNickNameData
    {
        public List<string> NickNames { get; set; }
    }
}
