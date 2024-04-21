namespace DriveForum.Models
{
    public class User
    {
        public User(string username, string email, string login, string password)
        {
            Login = login;
            Password = password;
            Username = username;
            Email = email;
        }

        public User() { }

        public int Id { get; set; }
        public string Username { get; set; }
        public string? Description { get; set; } = null!;
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; } = Roles.User;
        public bool IsBanned { get; set; } = false;
        public List<UserPost>? UserPosts { get; set; } = new();
        public List<Comment>? UserComments { get; set; } = new();
        public List<UserCar> Cars { get; set; } = new();
    }
    public enum Roles
    {
        User,
        Moderator
    }
}