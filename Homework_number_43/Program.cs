using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework_number_43
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandShowBackpack = "1";
            const string CommandBuyProduct = "2";
            const string CommandExit = "3";

            List<Product> products = new List<Product>();
            products.Add(new Product("Нож", 10));
            products.Add(new Product("Мечь", 80));
            products.Add(new Product("Щит", 100));

            int playerMoney = 100;

            Seller seller = new Seller(products);
            Player player = new Player(playerMoney);
            Market market = new Market();

            bool isExit = false;
            string userInput;

            while (isExit == false)
            {
                Console.WriteLine($"Добрый день приветствую вас в нашей лавке для покупки у нас имеются такие товары\n" +
                                  $"Количество денег продавца: {seller.Money}\n" +
                                  $"Количество денег игрока: {player.Money}");

                seller.ShowProductList();

                Console.WriteLine($"\n\nДля того что бы посмотреть свой рюкзак нажмите: {CommandShowBackpack}\n" +
                                  $"Для того что бы купить товар нажмите: {CommandBuyProduct}\n" +
                                  $"Для того что бы выйти нажмите: {CommandExit}\n\n");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandShowBackpack:
                        player.ShowProductList();
                        break;

                    case CommandBuyProduct:
                        market.MakeDeal(seller,player);
                        break;

                    case CommandExit:
                        isExit = true;
                        break;
                }

                Console.WriteLine("Для продолжения ведите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Hero
    {
        protected List<Product> _products = new List<Product>();

        public int Money { get; protected set; }

        public void ShowProductList()
        {
            if (_products.Count() == 0)
            {
                return;
            }

            for (int i = 0; i < _products.Count; i++)
            {
                Console.WriteLine($"\nНазвание: {_products[i].Title} - Цена: {_products[i].Price}\n");
            }
        }
    }

    class Product
    {
        public Product(string title, int price)
        {
            Title = title;
            Price = price;
        }

        public string Title { get; private set; }
        public int Price { get; private set; }
    }

    class Market
    {
        public void MakeDeal(Seller seller , Player player)
        {
            Console.WriteLine("Укажите название товара который хотите купить: ", ConsoleColor.Cyan);
            string userInput = Console.ReadLine();

            if (seller.TrySellProduct(out Product product, player.Money, userInput) && product != null)
            {
                player.BuyProduct(product);
            }
        }
    }

    class Player : Hero
    {
        public Player(int money) 
        {
            Money = money;
        }

        public void BuyProduct(Product product)
        {
            if (product != null)
            {
                _products.Add(product);

                Money -= product.Price;

                Console.WriteLine("Придмет успешно перемещён к вам в рюкзак");
            }
        }
    }

    class Seller : Hero
    {
        public Seller(List<Product> products)
        {
            _products = products;
        }

        public bool TrySellProduct(out Product product, int playerMoney, string title)
        {
            product = null;

            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].Title == title)
                {
                    if (playerMoney >= _products[i].Price)
                    {
                        product = _products[i];

                        Money += _products[i].Price;

                        _products.RemoveAt(i);

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("К сожалению у вас не хватает денег\n", ConsoleColor.Red);

                        return false;
                    }
                }
            }

            return false;
        }
    }
}
