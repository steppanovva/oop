using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Models
{
    public class Shop
    {
        private string _name;
        private int _shopId;
        private double _money;
        private List<Product> _products;
        private int _defaultExtraCharge = 1;

        public Shop(string name, int shopId, int money)
        {
            _name = name;
            _shopId = shopId;
            _money = money;
            _products = new List<Product>();
        }

        public int ShopId { get => _shopId; }
        public double Money { get => _money;  }
        public static int TotalCost(Dictionary<Product, int> products)
        {
            return products.Sum(i => i.Value);
        }

        public Product GetProductInfo(string name)
        {
            var product = _products.Find(x => x.Name == name);
            if (product == null)
                throw new ShopsException("Product " + name + " is not found.");
            return product;
        }

        public void Supply(Product p)
        {
            var product = _products.Find(x => x.Name == p.Name);
            if (product != null)
            {
                product.Count += p.Count;
                product.Price *= _defaultExtraCharge;
            }
            else
            {
                if (!ShopManager.RegisteredProducts.Exists(x => x.Name == p.Name))
                    throw new ShopsException("Product " + p.Name + " is not registered.");
                _products.Add(new Product(p.Name, p.Price * _defaultExtraCharge, p.Count));
            }

            _money -= p.Price * p.Count;
        }

        public void Supply(List<Product> newProducts)
        {
            foreach (var i in newProducts)
                Supply(i);
        }

        public void SetProductPrice(string name, double newPrice)
        {
            var product = _products.Find(x => x.Name == name);
            if (product == null)
                throw new ShopsException("There is no " + name + " product.");
            product.Price = newPrice;
        }

        public Dictionary<Product, int> AvailableProducts(Dictionary<string, int> products)
        {
            var availableProducts = new Dictionary<Product, int>();
            foreach ((string key, int value) in products)
            {
                Product product = _products.Find(x => x.Name == key);
                if (product == null)
                    throw new ShopsException("There is no such product with name " + key);
                if (product.Count < value)
                    throw new ShopsException("There is not enough products of " + product.Name + " " + product.Count + " available.");
                availableProducts.Add(product, value);
            }

            return availableProducts;
        }

        public void Sell(Dictionary<string, int> products, Consumer consumer) // <vendor code, count>
        {
            var availableProducts = AvailableProducts(products);
            double total = TotalCost(availableProducts);
            if (consumer.Money < total)
                throw new ShopsException("Consumer " + consumer + " has not enough money");
            consumer.Money -= total;
            _money += total;
            foreach ((Product key, int value) in availableProducts)
            {
                var product = _products.Find(x => x.Name == key.Name);
                product.Count -= value;
            }
        }
    }
}