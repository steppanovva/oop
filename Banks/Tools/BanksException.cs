using System;

namespace Banks.Tools
{
    public class BanksException : Exception
    {
        public BanksException() { }

        public BanksException(string message)
            : base(message) { }
    }
}