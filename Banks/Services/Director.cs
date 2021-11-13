namespace Banks.Services
{
    public class Director
    {
        private IClientBuilder _builder;

        public IClientBuilder Builder
        {
            set => _builder = value;
        }

        public void BuildMinimalClient(string name, int id)
        {
            _builder.AddName(name);
            _builder.AddId(id);
        }

        public void BuildFullClient(string name, string address, int id)
        {
            _builder.AddName(name);
            _builder.AddAddress(address);
            _builder.AddId(id);
        }
    }
}