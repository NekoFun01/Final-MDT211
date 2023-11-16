namespace Neko
{
    class Account
    {
        private Dictionary<string, Account> _userDictionary = new Dictionary<string, Account>();

        public string Username { get; set; }
        public string Id { get; }

        public int balance { get; }
        private List<Account> _followList = new List<Account>();


        public List<Account> GetFollowList()
        {
            List<Account> resultList = new List<Account>();
            foreach (Account user in _followList)
            {
                resultList.Add(user);
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
        public void RemoveFollower(Account User)
        {
            _followList.Remove(User);
        }

        public void RegisterUser(string userName)
        {
            if (_userDictionary.ContainsKey(userName))
            {
                Console.WriteLine("Username " + userName + " already registered");
                return;
            }

            Account newUser = new Account
            {
                Username = userName
            };
            _userDictionary.Add(userName, newUser);
            Console.WriteLine("Register user " + newUser.Username);
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


    }
    public abstract class Post
    {
        List<Post> postList = new List<Post>();
        public string Content { get; set; }
        public string Username { get; set; }


        public abstract void Display();
    }

    public class TextPost : Post
    {
        public override void Display()
        {
            Console.WriteLine("Text Post by " + Username + ": " + Content);
        }
    }

    public class ImagePost : Post
    {
        public string ImageUrl { get; set; }

        public override void Display()
        {
            Console.WriteLine("Image Post by " + Username + ": " + Content);
            Console.WriteLine("Image URL: " + ImageUrl);
        }
    }


    class Programs
    {
        static void Main(string[] args)
        {
            List<Post> postList = new List<Post>();
            Account test = new Account();
            test.RegisterUser("Neko");
            test.RegisterUser("Nekak");

            test.GetUser("Neko").FollowUser(test.GetUser("Nekak"));
            test.GetUser("Neko").FollowUser(test.GetUser("Nekak"));
            TextPost textPost1 = new TextPost
            {
                Username = "Alice",
                Content = "Hello, World!"
            };

            ImagePost imagePost1 = new ImagePost
            {
                Username = "Charlie",
                Content = "Beautiful sunset",
                ImageUrl = "sunset.jpg"
            };
            postList.Add(textPost1);
            postList.Add(imagePost1);
            foreach (Post post in postList)
            {
                post.Display();
                Console.WriteLine("------");
            }

        }
    }
}

