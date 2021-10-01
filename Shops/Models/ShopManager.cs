using System.Collections.Generic;
namespace Shops.Models
{
    public class ShopManager
    {
        private static int _shopId = 1;
        private static List<Product> _products;
        private List<Shop> _shops;
        public ShopManager()
        {
            _shops = new List<Shop>();
            _products = new List<Product>();
        }

        public static List<Product> RegisteredProducts { get => _products; }
        public Shop Create(string name, int money)
        {
            var shop = new Shop(name, _shopId++, money);
            _shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name, double primeCost)
        {
            var product = new Product(name, primeCost, 1);
            _products.Add(product);
            return product;
        }

        public Shop FindProfitableShop(Dictionary<string, int> products)
        {
            Shop minPriceShop = null;
            double minPrice = -1.0;
            foreach (var shop in _shops)
            {
                var availableProducts = shop.AvailableProducts(products);
                double total = Shop.TotalCost(availableProducts);
                if (!(total < minPrice) && minPrice != -1.0) continue;
                minPrice = total;
                minPriceShop = shop;
            }

            return minPriceShop;
        }
    }
}