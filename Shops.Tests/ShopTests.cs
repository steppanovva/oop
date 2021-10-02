using System.Collections.Generic;
using NUnit.Framework;
using Shops.Tools;
using Shops.Models;

namespace Shops.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Supply_ProductsAddedAndSold()
        {
            var shopManager = new ShopManager();
            var consumer = new Consumer("Consumer", 30);
            var shop = shopManager.Create("First shop", 100);
            var product1 = shopManager.RegisterProduct("apple", 1);
            var product2 = shopManager.RegisterProduct("banana", 2);
            var product3 = shopManager.RegisterProduct("kiwi", 3);
            var products = new List<Product>();
            products.Add(product1);
            products.Add(product2);
            products.Add(product3);
            foreach (var i in products) { i.Count = 5; }
            shop.Supply(products);
            var cProducts = new Dictionary<string, int>();
            cProducts.Add("apple", 1);
            cProducts.Add("banana", 1);
            cProducts.Add("kiwi", 1);
            shop.Sell(cProducts, consumer);
            Assert.AreEqual(4, shop.GetProductInfo("apple").Count);
            Assert.AreEqual(4, shop.GetProductInfo("banana").Count);
            Assert.AreEqual(4, shop.GetProductInfo("kiwi").Count);
            Assert.AreEqual(73, shop.Money);
        }

        [Test]
        public void SetNewPrice_PriceChanged()
        {
            var shopManager = new ShopManager();
            var product = shopManager.RegisterProduct("apple", 1);
            var shop = shopManager.Create("Second shop", 100);
            shop.Supply(product);
            shop.SetProductPrice("apple", 2);
            Assert.AreEqual(2, shop.GetProductInfo("apple").Price);
        }

        [Test]
        public void FindShopWithMinPrice_ThrowException_ProductIsNotRegistered()
        {
            Assert.Catch<ShopsException>(() =>
            {
                var shopManager = new ShopManager();
                var product1 = shopManager.RegisterProduct("apple", 1);
                var product2 = shopManager.RegisterProduct("banana", 2);
                var shop1 = shopManager.Create("First shop", 100);
                var shop2 = shopManager.Create("Second shop", 100);
                shop1.Supply(product1);
                shop2.Supply(product2);
                var pDictionary = new Dictionary<string, int>();
                pDictionary.Add("kiwi", 1);
                shopManager.FindProfitableShop(pDictionary);
            });
        }

        [Test]
        public void FindShopWithMinPrice_ShopIsFound()
        {
            var shopManager = new ShopManager();
            var product = shopManager.RegisterProduct("apple", 1);
            var shop1 = shopManager.Create("First shop", 100);
            var shop2 = shopManager.Create("Second shop", 100);
            shop1.Supply(product);
            shop2.Supply(product);
            shop1.SetProductPrice("apple", 2);
            shop2.SetProductPrice("apple", 3);
            var pDictionary = new Dictionary<string, int>();
            pDictionary.Add("apple", 1);
            var profitableShop = shopManager.FindProfitableShop(pDictionary);
            Assert.AreEqual(shop1.ShopId, profitableShop.ShopId);
        }
    }
}
