using System;
using System.IO;

namespace BackupsExtra.Services
{
    public class Config
    {
        public string PathToRestorePoints { get; set; }
        public string PathToLog { get; set; }
        public int MaxNumberOfPoints { get; set; }
        public int ModeOfLimitation { get; set; }
        public string IsTimePrefixNeeded { get; set; }
        public string WayOfLogging { get; set; }

        public void Parse(string configFile)
        {
            string[] parsed = File.ReadAllLines(configFile);
            WayOfLogging = parsed[1];
            PathToLog = parsed[3];
            MaxNumberOfPoints = Convert.ToInt32(parsed[5]);
            ModeOfLimitation = Convert.ToInt32(parsed[7]);
            IsTimePrefixNeeded = parsed[9];
            PathToRestorePoints = parsed[11];
        }
    }
}