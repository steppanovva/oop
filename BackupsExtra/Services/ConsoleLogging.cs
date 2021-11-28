using System;

namespace BackupsExtra.Services
{
    public class ConsoleLogging : ILog
    {
        private readonly string _timePrefix;

        public ConsoleLogging(string isTimePrefixNeeded)
        {
            _timePrefix = isTimePrefixNeeded;
        }

        public void Write(string message)
        {
            if (_timePrefix == "yes")
            {
                Console.WriteLine(DateTime.Now + " " + message + '\n');
                return;
            }

            Console.WriteLine(message);
        }
    }
}