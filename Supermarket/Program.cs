using System;
using System.Collections.Generic;

namespace Supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int maximumMoney = 2500;
            int minimumMoney = 1000;
            int maximumClients = 15;
            int minimumClients = 3;
            Queue<Client> clients = new Queue<Client>();

            for (int i = 0; i < random.Next(minimumClients, maximumClients); i++)
            {
                clients.Enqueue(new Client(random.Next(minimumMoney, maximumMoney)));
            }

            while (clients.Count > 0)
            {
                Client client = clients.Dequeue();
                client.ShowInfo();
                Console.WriteLine();
                client.Pay();
                Console.ReadKey();
                Console.Clear();
            }

            Console.WriteLine("Магазин пустует ...");
        }
    }

    class Client
    {
        private static int _ids;
        private Random _random = new Random();
        private List<Product> _products = new();

        public int Id { get; private set; }
        public int Money { get; private set; }

        public Client(int money)
        {
            Id = ++_ids;
            Money = money;
            _products = GetListProduct();
        }

        public void Pay()
        {
            int moneyToPay;

            while (CheckPayment(out moneyToPay) == false)
            {
                Console.WriteLine($"К оплате - {moneyToPay}");
                int deleteIndex = _random.Next(0, _products.Count);
                Console.WriteLine($"Не хватает средств. Удален товар: {_products[deleteIndex].Name} по цене {_products[deleteIndex].Price}");
                _products.RemoveAt(deleteIndex);
            }

            Money -= moneyToPay;
            Console.WriteLine($"\nОплата прошла успешно! Чек - {moneyToPay} | Осталось - {Money}");
            _products.Clear();
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Клиент_{Id} | Баланс - {Money}" +
                              $"\n\nПродукты в корзине:");

            for (int i = 0; i < _products.Count; i++)
            {
                _products[i].ShowInfo();
            }
        }

        private bool CheckPayment(out int moneyToPay)
        {
            moneyToPay = 0;

            for (int i = 0; i < _products.Count; i++)
            {
                moneyToPay += _products[i].Price;
            }

            if (Money >= moneyToPay)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Product> GetListProduct()
        {
            List<Product> products = new List<Product>();
            int maximumPrice = 200;
            int minimumPrice = 50;

            for (int i = 0; i < _random.Next(10, 20); i++)
            {
                products.Add(new Product($"Продукт_{i + 1}", _random.Next(minimumPrice, maximumPrice)));
            }

            return products;
        }
    }

    class Product
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Name} | Цена - {Price}");
        }
    }
}