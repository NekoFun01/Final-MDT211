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
        public void RemoveFollower(Account accountnum)
        {
            _followList.Remove(accountnum);
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

    }
    class Programs
    {
        static void Main(string[]args)
        {
            Account test = new Account();
            test.RegisterUser("Neko");
            test.RegisterUser("Neko");
        }
    }
}

