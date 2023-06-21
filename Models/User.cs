namespace FinancialTracker.Models
{
    public class User
    {
        public string Id { get; private set; } = String.Empty;
        public string FirstName { get; private set; } = String.Empty;
        public string LastName { get; private set; } = String.Empty;
        public string Email { get; private set; } = String.Empty;
        public string Password { get; private set; } = String.Empty;

        public User()
        {
        }

        private User(string id, string firstName, string lastName, string email, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public static User Create(string id, string firstName, string lastName, string email, string password) =>
            new User(id, firstName, lastName, email, password);

        public static User CreateNewUser(string firstName, string lastName, string email, string password) =>
            new User(Guid.NewGuid().ToString(), firstName, lastName, email, password);

        public void ScrubPassword() =>
            Password = String.Empty;

        public void SetPassword(string passwordHash) =>
            Password = passwordHash;
    }
}
