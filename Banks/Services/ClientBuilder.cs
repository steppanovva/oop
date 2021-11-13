namespace Banks.Services
{
    public class ClientBuilder : IClientBuilder
    {
        private Client _client = new Client();

        public ClientBuilder()
        {
            Reset();
        }

        public void AddName(string name)
        {
            _client.Name = name;
        }

        public void AddAddress(string address)
        {
            _client.Address = address;
        }

        public void AddId(int id)
        {
            _client.Id = id;
        }

        public Client GetClient()
        {
            Client result = _client;
            Reset();
            return result;
        }

        private void Reset()
        {
            _client = new Client();
        }
    }
}