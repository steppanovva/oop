using System;
using System.Linq;

namespace Banks.Services
{
    public class BankConsole : IConsole
    {
        public string EnterName()
        {
            Console.WriteLine("Enter your name: ");
            return Console.ReadLine();
        }

        public int EnterId()
        {
            Console.WriteLine("Enter your Id: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        public string AcceptAgreement()
        {
            Console.WriteLine("Do you want to get notifications?");
            return Console.ReadLine();
        }

        public string EnterAddress()
        {
            Console.WriteLine("Do you want to enter address? An account without it will work with restrictions.");
            string answer = Console.ReadLine();
            return answer is "yes" or "Yes" ? Console.ReadLine() : null;
        }

        public void ReadNotifications(Client client)
        {
            if (!client.MessagesFromBank.Any())
                Console.WriteLine("No messages");
            foreach (string message in client.MessagesFromBank)
            {
                Console.WriteLine(message);
            }
        }
    }
}