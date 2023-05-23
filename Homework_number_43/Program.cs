using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                        player.ShowListOfItems();
                        break;

                    case CommandBuyProduct:
                        player.BuyProduct(seller);
                        break;

                    case CommandExit:
                        isExit = true;
                        break;
                }
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

    class Player
    {
        private List<Product> _products = new List<Product>();

        public Player(int money)
        {
            Money = money;
        }

        public int Money { get; private set; }

        public void BuyProduct(Seller seller)
        {
            ShowMessage("Укажите название товара который хотите купить: ", ConsoleColor.Cyan);
            string userInput = Console.ReadLine();

            if (seller.TrySellProduct(out Product product, Money, userInput) && product != null)
            {
                _products.Add(product);

                Money -= product.Price;

                ShowMessage("Придмет успешно перемещён к вам в рюкзак");
            }
        }

        public void ShowListOfItems()
        {
            if (_products.Count() > 0)
            {
                ShowMessage($"Рюкзак");

                for (int i = 0; i < _products.Count; i++)
                {
                    ShowMessage($"Название: {_products[i].Title} - Цена: {_products[i].Price}");
                }
            }
            else
            {
                ShowMessage("К сожалению ваш рюкзак пуст!");
            }
        }

        private void ShowMessage(string text, ConsoleColor consoleColor = ConsoleColor.Blue)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }

    class Seller
    {
        private List<Product> _products = new List<Product>();

        public Seller(List<Product> products)
        {
            _products = products;
        }

        public int Money { get; private set; }

        public void ShowProductList()
        {
            ShowMessage($"Список товаров");

            for (int i = 0; i < _products.Count; i++)
            {
                ShowMessage($"Название: {_products[i].Title} - Цена: {_products[i].Price}");
            }
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
                        ShowMessage("К сожалению у вас не хватает денег\n", ConsoleColor.Red);

                        return false;
                    }
                }
            }

            if (product == null)
            {
                ShowMessage("К сожалению у меня нет этого товара в наличии", ConsoleColor.Red);
            }

            return false;
        }

        private void ShowMessage(string text, ConsoleColor consoleColor = ConsoleColor.Green)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
