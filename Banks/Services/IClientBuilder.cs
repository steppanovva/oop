namespace Banks.Services
{
    public interface IClientBuilder
    {
        void AddName(string name);
        void AddAddress(string address);
        void AddId(int id);
    }
}