using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public static class TPlayerLog
    {
        private static string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_", "players.json");
        private static List<TPlayerSession> Session = new List<TPlayerSession>();
        public static List<TPlayerSession> GetAll() => Session;

        public static void Load()
        {
            if (!File.Exists(FilePath))
            {
                Session = new List<TPlayerSession>();
                return;
            }
            string json = File.ReadAllText(FilePath);
            Session = JsonSerializer.Deserialize<List<TPlayerSession>>(json) ?? new List<TPlayerSession>();
        }

        public static void AddSession(string name, DiffLvls level, Mode mode, TimeSpan time)
        {
            Session.Add(new TPlayerSession
            {
                Name = name,
                Level = level,
                Mode = mode,
                Time = time,
                PlayedAt = DateTime.Now
            });
            Save();
        }

        public static void Save()
        {
            string json = JsonSerializer.Serialize(Session, new JsonSerializerOptions { WriteIndented = true });
            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllText(FilePath, json);
        }
    }
}
