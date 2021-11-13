using System.Collections.Generic;

namespace Banks.Services
{
    public class Client
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Id { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public List<string> MessagesFromBank { get; } = new ();
        public void GetMessage(string message)
        {
            MessagesFromBank.Add(message);
        }
    }
}