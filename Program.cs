namespace Neko
{
    ///////////////////////////////////////////////////////////////////////////////////////////// Class ACCOUNT HEAD //////////////////////////////////////////////////////////////
    class Account                                                 
    {
        private Dictionary<string, Account> _userDictionary = new Dictionary<string, Account>();

        public string Username { get; set; }
        public string Id { get; }

        public int balance { get; }
        private List<Account> _followList = new List<Account>();

 /////////////////////////////////////////////////////////////////////////////////////////////  Follow HEAD //////////////////////////////////////////////////////////////

        // public List<Account> GetFollowList()                ////// ใช้ add user เข้าไปใน list //////////
        // {
        //     List<Account> resultList = new List<Account>();
        //     foreach (Account user in _followList)
        //     {
        //         resultList.Add(user);
        //     }
        //     return resultList;
        // }
        public void FollowUser(Account userToFollow)   //////แอด Follower เข้า List
        {
            if (_followList.Contains(userToFollow))
            {
                Console.WriteLine(this.Username + " has already follow " + userToFollow.Username);
                return;
            }

            _followList.Add(userToFollow);
            Console.WriteLine(this.Username + " follow " + userToFollow.Username);
        }
        public void RemoveFollower(Account User)       /////ลบ Follower
        {
            _followList.Remove(User);
        }
/////////////////////////////////////////////////////////////////////////////////////////////  Follow Bot //////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////  Register Head //////////////////////////////////////////////////////////////
        public void RegisterUser(string userName)       /// UserName ห้ามซ้ำกัน
        {
            if (_userDictionary.ContainsKey(userName))
            {
                Console.WriteLine("Username " + userName + " already registered");
                return;
            }

            Account newUser = new Account       ////ผ่าน if = ชื่อไม่ซ้ำ จะแอดเข้า list
            {
                Username = userName
            };
            _userDictionary.Add(userName, newUser);
            Console.WriteLine("Register user " + newUser.Username);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////  Register Bot //////////////////////////////////////////////////////////////
        public Account GetUser(string userName)      ////ใช้เรียก User ตัวตั้งต้น
        {
            if (!_userDictionary.ContainsKey(userName))
            {
                Console.WriteLine("Username " + userName + " does not exist");
                return null;
            }

            return _userDictionary[userName];
        }


    }
       ///////////////////////////////////////////////////////////////////////////////////////////// Class ACCOUNT BOT //////////////////////////////////////////////////////////////
       ////////////////////////////////////////////////////////////////////////////////////////////////  Post Head//////////////////////////////////////////////////////////////
   
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

//////////////////////////////////////////////////////////////////////////////////////////////// Post Bot//////////////////////////////////////////////////////////////
    class Feed 
    {
        public string UserFeed;
        public string UserPost;
        public string Register;
    }
  ////////////////////////////////////////////////////////////////////////////////////////////////  Main  Head //////////////////////////////////////////////////////////////
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
     ////////////////////////////////////////////////////////////////////////////////////////////////  Main Bot //////////////////////////////////////////////////////////////
}

