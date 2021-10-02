using System;

namespace Shops.Models
{
    public class Product
    {
        private string _name;
        private double _price;
        private int _count;

        public Product(string name, double primeCost, int count)
        {
            _name = name;
            _price = primeCost;
            _count = count;
        }

        public string Name { get => _name; set => _name = value; }
        public int Count { get => _count; set => _count = value; }
        public double Price { get => _price; set => _price = value; }
    }
}