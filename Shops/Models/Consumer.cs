namespace Shops.Models
{
    public class Consumer
    {
        private string _name;
        private double _money;
        public Consumer(string name, int money)
        {
            _name = name;
            _money = money;
        }

        public string Name { get => _name; }
        public double Money { get => _money; set => _money = value; }
    }
}