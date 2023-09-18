namespace FinancialTracker.Models
{
    public class User
    {
        public string Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        private User(string id, string firstName, string lastName, string email, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

    }
}
