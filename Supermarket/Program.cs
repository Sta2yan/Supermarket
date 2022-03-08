using System;
using System.Collections.Generic;

namespace Supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();
            supermarket.ServeCustomers();
        }
    }

    class Supermarket
    {
        private Random _random = new Random();
        private int _maximumMoney = 2500;
        private int _minimumMoney = 1000;
        private int _maximumClients = 15;
        private int _minimumClients = 3;
        
        public void ServeCustomers(Queue<Client> clients = null)
        {
            if (clients == null)
            {
                clients = SetQueueClients();
            }

            while (clients.Count > 0)
            {
                Client client = clients.Dequeue();
                client.ShowInfo();
                Console.WriteLine();
                client.Buy();
                Console.ReadKey();
                Console.Clear();
            }

            Console.WriteLine("Магазин пустует ...");
        }

        private Queue<Client> SetQueueClients()
        {
            Queue<Client> clients = new Queue<Client>();

            for (int i = 0; i < _random.Next(_minimumClients, _maximumClients); i++)
            {
                clients.Enqueue(new Client(_random.Next(_minimumMoney, _maximumMoney)));
            }

            return clients;
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
            _products = SetListProduct();
        }

        public void Buy()
        {
            while (CheckPayment() == false)
            {
                Console.WriteLine($"К оплате - {GetAmountPurchases()}");
                int deleteIndex = _random.Next(0, _products.Count);
                Console.WriteLine($"Не хватает средств. Удален товар: {_products[deleteIndex].Name} по цене {_products[deleteIndex].Price}");
                _products.RemoveAt(deleteIndex);
            }

            Money -= GetAmountPurchases();
            Console.WriteLine($"\nОплата прошла успешно! Чек - {GetAmountPurchases()} | Осталось - {Money}");
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

        private bool CheckPayment()
        {
            return Money >= GetAmountPurchases();
        }

        private int GetAmountPurchases()
        {
            int moneyToPay = 0;

            for (int i = 0; i < _products.Count; i++)
            {
                moneyToPay += _products[i].Price;
            }

            return moneyToPay;
        }

        private List<Product> SetListProduct()
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