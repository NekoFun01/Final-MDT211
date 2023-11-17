namespace Neko
{
    public enum Visibility
    {
        PRIVATE,
        PUBLIC,
    }

    ///////////////////////////////////////////////////////////////////////////////////////////// Class ACCOUNT HEAD //////////////////////////////////////////////////////////////
    class Account
    {
        private Dictionary<string, Account> _userDictionary = new Dictionary<string, Account>();
        List<Post> postList = new List<Post>();

        public string Username { get; set; }
        public string Id { get; }

        public int balance { get; }
        private List<Account> _followList = new List<Account>();

        /////////////////////////////////////////////////////////////////////////////////////////////  Follow HEAD //////////////////////////////////////////////////////////////

        public List<Account> GetFollowList()                ////// ใช้ add user เข้าไปใน list //////////
        {
            List<Account> resultList = new List<Account>();
            foreach (Account user in _followList)
            {
                resultList.Add(user);
            }
            return resultList;
        }
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
        public List<Post> FetchPublicPost()
        {
            List<Post> resultList = new List<Post>();
            foreach (Post post in postList)
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
                Username = Username,
                PostVisibility = visibility,
                Content = content
            };
            postList.Add(newPost);
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////// Class ACCOUNT BOT //////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////  Post Head//////////////////////////////////////////////////////////////

    public abstract class Post
    {

        public string Content { get; set; }
        public string Username { get; set; }
        public Visibility PostVisibility { get; set; }
       


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

    public class VideoPost : Post
    {
        public string VideoUrl { get; set; }

        public override void Display()
        {
            Console.WriteLine("Video Post by " + Username + ": " + Content);
            Console.WriteLine("Video URL: " + VideoUrl);
        }
    }

    public class SalePost : Post
    {
        public string SaleUrl { get; set; }

        public override void Display()
        {
            Console.WriteLine("Sale Post by " + Username + ": " + Content);
            Console.WriteLine("Sale URL: " + SaleUrl);
        }

    }



    //////////////////////////////////////////////////////////////////////////////////////////////// Post Bot//////////////////////////////////////////////////////////////
    ///
    //////////////////////////////////////////////////////////////////////////////////////////////// Feed Head//////////////////////////////////////////////////////////////
    class Feed
    {
        public string UserFeed;
        public string UserPost;
        public string Register;

        public void ShowUserFeed(Account viewer)
        {
            int MAX_POST_PER_USER = 2;
            int MAX_POST_AMOUNT = 10;

            List<Post> postToDisplayList = new List<Post>();
            List<Account> followList = viewer.GetFollowList();
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

            Console.WriteLine("-- Show " + viewer.Username + " feed ----");
            foreach (Post post in postToDisplayList)
            {
                post.Display();
            }
            Console.WriteLine("----");
        }

        public void ShowUserWall(Account userToDisplay)
        {
            List<Post> postToDisplayList = userToDisplay.FetchPublicPost();

            Console.WriteLine("---- Show " + userToDisplay.Username + " wall ---");
            foreach (Post post in postToDisplayList)
            {
                post.Display();
            }
            Console.WriteLine("---");
        }

    }
    //////////////////////////////////////////////////////////////////////////////////////////////// Feed Bot//////////////////////////////////////////////////////////////
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

           
            postList.Add(textPost1);
           
            foreach (Post post in postList)
            {
                post.Display();
                Console.WriteLine("------");
            }

            test.GetUser("Neko").AddTextPost("A cryptobiote a day keeps the Timefall away.", Visibility.PUBLIC);

            // Dictionary<string, string> posts = new Dictionary<string, string>();

            // bool exit = false;

            // while (!exit)
            // {
            //     Console.WriteLine("1. Add Post\n2. View Feed\n3. Exit");
            //     Console.Write("Select an option: ");
            //     string option = Console.ReadLine();

            //     switch (option)
            //     {
            //         case "1":
            //             Console.Write("Enter username: ");
            //             string username = Console.ReadLine();
            //             Console.Write("Enter post: ");
            //             string post = Console.ReadLine();

            //             posts.Add(username, post);
            //             Console.WriteLine("Post added successfully!\n");
            //             break;

            //         case "2":
            //             Console.WriteLine("Feed:");
            //             foreach (KeyValuePair<string, string> entry in posts)
            //             {
            //                 Console.WriteLine($"Username: {entry.Key}\nPost: {entry.Value}\n");
            //             }
            //             break;

            //         case "3":
            //             exit = true;
            //             break;

            //         default:
            //             Console.WriteLine("Invalid option. Please try again.\n");
            //             break;
            //     }
            // }
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////  Main Bot //////////////////////////////////////////////////////////////
}

