using System;
using System.IO;
using System.Collections.Generic;
using Sports.Logic.Interface;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Logic
{
    public class TextLog : ILogLogic
    {
        char separator = '|';
        string logFilePath = Directory.GetCurrentDirectory() + @"\Log.txt";

        public void AddEntry(string entry, string username, DateTime date)
        {
            if (!File.Exists(logFilePath))
            {
                CleanLog();
            }
            string logLine = entry + separator + username + separator + date.ToString() + ';';
            File.AppendAllText(logFilePath, logLine);
        }

        public ICollection<string> GetBetweenDates(DateTime startDate, DateTime endDate)
        {
                StreamReader reader = File.OpenText(logFilePath);
                string text = reader.ReadToEnd();
                text = text.Replace("\n", "").Replace("\r", "");
                ICollection<string> entries = new List<string>();
                string[] logs = text.Split(';');
                foreach (string log in logs)
                {
                    if (log != "")
                    {
                        DateTime date = DateTime.Parse(log.Split(separator)[2]);
                        if (startDate.Date <= date.Date && endDate.AddDays(1).Date > date)
                        {
                            string entry = log.Replace(separator, ' ');
                            entries.Add(entry);
                        }
                    }
                }
                reader.Close();
                return entries;
        }

        public void CleanLog()
        {
                StreamWriter writer = File.CreateText(logFilePath);
                writer.Close();
                File.WriteAllText(logFilePath, String.Empty);
        }
    }
}
