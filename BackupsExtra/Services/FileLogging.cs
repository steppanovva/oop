using System;
using System.IO;

namespace BackupsExtra.Services
{
    public class FileLogging : ILog
    {
        private readonly string _timePrefix;

        public FileLogging(string isTimePrefixNeeded, string pathToLogFile)
        {
            _timePrefix = isTimePrefixNeeded;
            PathToLogFile = pathToLogFile;
        }

        private string PathToLogFile { get; }

        public void Write(string message)
        {
            using var sw = new StreamWriter(PathToLogFile, true);
            if (_timePrefix == "yes")
            {
                sw.Write(DateTime.Now + message + '\n');
                return;
            }

            sw.Write(message);
        }
    }
}