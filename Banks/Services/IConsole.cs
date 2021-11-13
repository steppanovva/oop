using Banks.Services;
namespace Banks.Services
{
    public interface IConsole
    {
        public string EnterName();
        public int EnterId();
        public string AcceptAgreement();
        public string EnterAddress();
        public void ReadNotifications(Client client);
    }
}