﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
namespace NFT
{
    public enum Visibility
    {
        PRIVATE,
        PUBLIC,
    }
    public abstract class Post
    {
        public string Content { get; set; }
        public Account Author { get; set; }
        public Visibility PostVisibility { get; set; }

        public abstract void Display();
    }

    public class TextPost : Post
    {
        public override void Display()
        {
            Console.WriteLine("Text Post by " + Author.Username + ": " + Content);
        }
    }

    public class ImagePost : Post
    {
        public string ImageUrl { get; set; }

        public override void Display()
        {
            Console.WriteLine("Image Post by " + Author.Username + ": " + Content);
            Console.WriteLine("Image URL: " + ImageUrl);
        }
    }

    public class VideoPost : Post
    {
        public string VideoUrl { get; set; }

        public override void Display()
        {
            Console.WriteLine("Video Post by " + Author.Username + ": " + Content);
            Console.WriteLine("Video URL: " + VideoUrl);
        }
    }

    public class SalePost : Post
    {
        public string Saleitem { get; set; }
        public string Cost { get; set; }

        public override void Display()
        {
            Console.WriteLine("Sale Post by " + Author.Username + ": " + Content);
            Console.WriteLine("Sale item: " + Saleitem + " ( {0} ETH )", Cost);
        }
    }


    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int balance { get; set; }


        private List<Account> _followList = new List<Account>();
        private List<Post> _postList = new List<Post>();
        private List<Post> _salepostList = new List<Post>();

        public List<Account> GetFollowList()
        {
            List<Account> resultList = new List<Account>();
            foreach (Account user in _followList)
            {
                resultList.Add(user);
            }
            return resultList;
        }

        public List<Post> GetSalepost()
        {
            List<Post> resultList = new List<Post>();
            foreach (Post post in _salepostList)
            {
                resultList.Add(post);
            }
            return resultList;
        }

        public void FollowUser(Account userToFollow)
        {
            if (_followList.Contains(userToFollow))
            {
                Console.WriteLine(this.Username + " has already follow " + userToFollow.Username);
                return;
            }

            _followList.Add(userToFollow);
            Console.WriteLine(this.Username + " follow " + userToFollow.Username);
        }
        public void RemoveFollower(Account toRemoveUser)
        {
            _followList.Remove(toRemoveUser);
            Console.WriteLine(this.Username + " unfollow " + toRemoveUser.Username);
        }

        public List<Post> FetchPublicPost()
        {
            List<Post> resultList = new List<Post>();
            foreach (Post post in _postList)
            {
                if (post.PostVisibility == Visibility.PUBLIC)
                {
                    resultList.Add(post);
                }
            }
            return resultList;
        }

        public void AddTextPost(string content, Visibility visibility)
        {
            TextPost newPost = new TextPost
            {
                Author = this,
                PostVisibility = visibility,
                Content = content
            };
            _postList.Add(newPost);
        }


        public void AddImagePost(string content, string imageUrl, Visibility visibility)
        {
            ImagePost newPost = new ImagePost
            {
                Author = this,
                PostVisibility = visibility,
                Content = content,
                ImageUrl = imageUrl,
            };
            _postList.Add(newPost);
        }

        public void AddVideoPost(string content, string videoUrl, Visibility visibility)
        {
            VideoPost newPost = new VideoPost
            {
                Author = this,
                PostVisibility = visibility,
                Content = content,
                VideoUrl = videoUrl,
            };
            _postList.Add(newPost);
        }

        public void AddSalePost(string content, string saleitem, string cost, Visibility visibility)
        {
            SalePost newPost = new SalePost
            {
                Author = this,
                PostVisibility = visibility,
                Content = content,
                Saleitem = saleitem,
                Cost = cost
            };
            _postList.Add(newPost);
            _salepostList.Add(newPost);
        }



    }

    class Application
    {
        private Dictionary<string, Account> _userDictionary = new Dictionary<string, Account>();
        public bool CheckLoop = true;


        public void ShowUserFeed(Account viewer)
        {
            if (_userDictionary.ContainsValue(viewer))
            {

                int MAX_POST_PER_USER = 5;
                int MAX_POST_AMOUNT = 10;

                List<Post> postToDisplayList = new List<Post>();
                List<Account> followList = viewer.GetFollowList();

                // Adding posts from followed users
                foreach (Account user in followList)
                {
                    if (postToDisplayList.Count >= MAX_POST_AMOUNT)
                    {
                        break;
                    }

                    int postCount = 0;
                    foreach (Post post in user.FetchPublicPost())
                    {
                        if (postCount >= MAX_POST_PER_USER || postToDisplayList.Count >= MAX_POST_AMOUNT)
                        {
                            break;
                        }
                        postToDisplayList.Add(post);
                        ++postCount;
                    }
                }

                // Adding sale posts from all users to the feed
                foreach (Account user in _userDictionary.Values)
                {
                    foreach (Post salePost in user.GetSalepost())
                    {
                        if (postToDisplayList.Count >= MAX_POST_AMOUNT)
                        {
                            break;
                        }
                        postToDisplayList.Add(salePost);
                    }
                }

                Console.WriteLine("--------------------");
                Console.WriteLine("= Show " + viewer.Username + " feed =");

                foreach (Post post in postToDisplayList)
                {
                    post.Display();
                }

                Console.WriteLine("--------------------");
            }

        }

        public void ShowUserWall(Account userToDisplay)
        {
            List<Post> postToDisplayList = userToDisplay.FetchPublicPost();
            Console.WriteLine("--------------------");
            Console.WriteLine("= Show " + userToDisplay.Username + " Post =");

            foreach (Post post in postToDisplayList)
            {
                post.Display();
            }
            Console.WriteLine("--------------------");

        }


        public void BotAccount()
        {
            Account nekoBotAccount = new Account
            {
                Username = "Neko",
                Password = "Neko007",
                balance = 0
            };

            Account nekakBotAccount = new Account
            {
                Username = "Nekak",
                Password = "Nekak008",
                balance = 0
            };

            _userDictionary.Add("Neko", nekoBotAccount);
            _userDictionary.Add("Nekak", nekakBotAccount);

        }
        public void RegisterUser()
        {

            // Account nekoBotAccount = new Account
            // {
            //     Username = "Neko",
            //     Password = "Neko007",
            //     balance = 0
            // };

            // Account nekakBotAccount = new Account
            // {
            //     Username = "Nekak",
            //     Password = "Nekak008",
            //     balance = 0
            // };

            // _userDictionary.Add("Neko", nekoBotAccount);
            // _userDictionary.Add("Nekak", nekakBotAccount);

            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            CheckLoop = true;
            if (_userDictionary.ContainsKey(username))
            {
                Console.WriteLine("Username " + username + " already registered");
                CheckLoop = false;
                return;
            }
            else
            {
                CheckLoop = true;
            }

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            Account newAccount = new Account
            {
                Username = username,
                Password = password,
                balance = 0
            };

            _userDictionary.Add(username, newAccount);
            Console.WriteLine("Register user " + newAccount.Username);
        }


        public void LoginUser()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            CheckLoop = true;
            if (_userDictionary.ContainsKey(username) && _userDictionary.ContainsKey(password))
            {
                Console.WriteLine("Login successful. Welcome, " + username + ".");
                CheckLoop = false;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                CheckLoop = true;
            }
        }
        public Account GetUser(string userName)
        {
            if (!_userDictionary.ContainsKey(userName))
            {
                Console.WriteLine("Username " + userName + " does not exist");
                return null;
            }

            return _userDictionary[userName];
        }

        public void ListAccount()
        {
            Console.WriteLine("All Account:");
            foreach (KeyValuePair<string, Account> acc in _userDictionary)//เหมือนเป็นclassใหม่
            {
                Console.WriteLine(" User: " + acc.Value.Username);

            }
        }

        public List<Account> SearchByAccountUsername(string accountUsername)
        {
            List<string> search = new List<string>();
            foreach (Account account in _userDictionary.Values)
            {
                if (account.Username == accountUsername)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("Account found  : " + accountUsername);
                    Console.WriteLine("--------------------");
                    search.Add(account.Username);
                }
                else if ("Exit" == accountUsername)
                {
                    break;
                }

            }
            if (search.Count == 0)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("Account not found  : " + accountUsername);
                Console.WriteLine("--------------------");
            }


            return null;
        }

    }

    public class Item
    {
        public string Item_name { get; set; }
        public double Item_price { get; set; }
        public string Item_author { get; set; }

        public Item(string itemName, double itemPrice, string itemAuthor)
        {
            Item_name = itemName;
            Item_price = itemPrice;
            Item_author = itemAuthor;
        }
    }
    public class Inventory
    {
        private Dictionary<string, Item> _itemDictionary = new Dictionary<string, Item>();
        private List<Item> _items;
        private List<Item> _market = new List<Item>();

        public List<Item> Items { get { return _items; } }

        public Inventory()
        {
            _items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public List<Item> DisplayInventory(Inventory inventory)
        {
            Console.WriteLine("Inventory:");
            foreach (var item in inventory._items)
            {
                Console.WriteLine($"Item name: {item.Item_name}, Price: {item.Item_price}, Author: {item.Item_author}");
            }

            return null;
        }

        public List<Item> SearchByItemName(string itemName)
        {
            List<string> search = new List<string>();

            foreach (Item item in _items)
            {
                if (item.Item_name == itemName)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("== Found item: " + itemName + ", Price: " + item.Item_price);
                    Console.WriteLine("--------------------");
                    search.Add(item.Item_name);

                }

            }

            if (search.Count == 0)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("== Not found item: " + itemName);
                Console.WriteLine("--------------------");

            }

            return null;
        }
        public void marketaddbot()
        {
            _market.Add(new Item("Testbot1", 5.99, "name1"));
            _market.Add(new Item("Testbot2", 17.99, "name2"));
            _market.Add(new Item("Neio", 1.99, "Test"));
        }
        public List<Item> Market()
        {
            foreach (var item in _market)
            {
                Console.WriteLine($"Item name: {item.Item_name}, Price: {item.Item_price}, Author: {item.Item_author}");
            }
            return null;

        }
        public List<Item> SearchMarket(string searchmarket)
        {
            List<string> searchMarket = new List<string>();

            foreach (Item item in _market)
            {
                if (item.Item_name == searchmarket)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("== Found item: " + searchmarket + ", Price: " + item.Item_price);
                    Console.WriteLine("--------------------");
                    searchMarket.Add(item.Item_name);

                }

            }
            foreach (Item item in _market)
            {
                if (item.Item_author == searchmarket)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("== Found Author: " + searchmarket + ", Price: " + item.Item_price);
                    Console.WriteLine("--------------------");
                    searchMarket.Add(item.Item_author);

                }

            }

            if (searchMarket.Count == 0)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("== Not found item: " + searchmarket);
                Console.WriteLine("--------------------");

            }

            return null;
        }

    }

    class UI
    {
        private Application application;

        public UI()
        {
            application = new Application();
        }

        public void Start()
        {
            bool isRegister = true;
            bool isRunning = true;
            Console.Clear();
            application.BotAccount();
            Inventory bot = new Inventory();
            bot.marketaddbot();
           


            while (isRegister)
            {
                Console.WriteLine("Hello, Make NFT trading easy with us.");
                Console.WriteLine("====================");
                Console.WriteLine("\u001b[1mOption\u001b[0m");
                Console.WriteLine("1.Login");
                Console.WriteLine("2.Register");
                Console.WriteLine("3.Exit");
                Console.WriteLine("====================");
                Console.Write("Select option: ");

                string optionChoice = Console.ReadLine();
                switch (optionChoice)
                {
                    case "1":
                        application.LoginUser();
                        if (application.CheckLoop == false)
                        {

                            isRegister = false;
                        }
                        break;

                    case "2":
                        application.RegisterUser();
                        if (application.CheckLoop == true)
                        {
                            isRegister = false;
                        }
                        break;

                    case "3":
                        isRegister = false;
                        isRunning = false;
                        break;
                }
            }

            if (isRunning == true)
            {
                application.GetUser("Nekak").FollowUser(application.GetUser("Neko"));
                application.GetUser("Nekak").RemoveFollower(application.GetUser("Neko"));
                application.GetUser("Neko").RemoveFollower(application.GetUser("Nekak"));

                application.GetUser("Neko").AddTextPost("This is fine.", Visibility.PUBLIC);
                application.GetUser("Nekak").AddTextPost("This is not fine.", Visibility.PUBLIC);
                application.GetUser("Nekak").AddImagePost("This is not fine.", "cat.jpg", Visibility.PUBLIC);
                application.GetUser("Neko").AddSalePost("This is not fine.", "cat", "1000", Visibility.PUBLIC);

                application.ShowUserWall(application.GetUser("Neko"));
                application.ShowUserWall(application.GetUser("Nekak"));

                while (isRunning)
                {
                    Console.WriteLine("Menu");
                    Console.WriteLine("====================");
                    Console.WriteLine("1.Feed post");
                    Console.WriteLine("2.Inventory");
                    Console.WriteLine("3.Search");
                    Console.WriteLine("4.Market");
                    Console.WriteLine("5.Exit");
                    Console.WriteLine("====================");
                    Console.Write("Menu select: ");
                    int menuSelect = int.Parse(Console.ReadLine());

                    switch (menuSelect)
                    {
                        case 1:
                            string UserName = "";
                            while (UserName != "Exit")
                            {
                                Console.Write("Please input username  or \"Exit\" to exit :");
                                UserName = Console.ReadLine();
                                if (UserName == "Exit")
                                {
                                    break;
                                }
                                application.ShowUserFeed(application.GetUser(UserName));


                            }
                            break;


                        case 2:
                            Inventory inventory = new Inventory();
                            inventory.Market();
                            inventory.AddItem(new Item("1", 5.99, "Nekak"));
                            inventory.AddItem(new Item("2", 17.99, "Nekak"));
                            inventory.AddItem(new Item("3", 1.99, "Nekak"));


                            inventory.DisplayInventory(inventory);
                            Console.Write("Search inventory (Y/N): ");
                            var choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "Y":
                                    string Itemkeyword = "";
                                    while (Itemkeyword != "Exit")
                                    {
                                        Console.Write("Please input item name  or \"Exit\" to exit :");
                                        Itemkeyword = Console.ReadLine();
                                        if (Itemkeyword == "Exit")
                                        {
                                            break;
                                        }
                                        Console.WriteLine();

                                        inventory.SearchByItemName(Itemkeyword);


                                    }

                                    break;

                                case "N":
                                    break;
                            }
                            break;

                        case 3:
                            string keyword = "";
                            while (keyword != "Exit")
                            {
                                Console.Write("Please input username to search or \"Exit\" to exit :");
                                keyword = Console.ReadLine();
                                if (keyword == "Exit")
                                {
                                    break;
                                }
                                application.SearchByAccountUsername(keyword);
                                
                            }
                            break;
                        case 4:
                        bool CheckMarket = true;
                        while(CheckMarket)
                        {
                            
                            Console.WriteLine("List Of Market");
                            
                            bot.Market();
                            Console.Write("Input Username or Item Name to search in market \"Exit\" to exit :");
                            string market = Console.ReadLine();
                            if (market == "Exit")
                            {
                                break;
                            }
                            bot.SearchMarket(market);
                            Console.Write("Do You Want To Search Another Item Or User IN Market ?");
                            Console.WriteLine("Y/N");
                            string marketnext = Console.ReadLine();
                            if (marketnext == "N")
                            {
                                break;
                            }

                        }

                            break;
                        case 5:
                            isRunning = false;
                            break;
                    }

                    Console.Clear();
                }
            }

            Console.Clear();
            for (int a = 3; a >= 0; a--)
            {
                Console.WriteLine("See you soon!");
                Console.Write("Appication Will Close In : {0}", a);
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }

    class Programs
    {
        static void Main(string[] args)
        {
            UI ui = new UI();
            ui.Start();
        }
    }
}
