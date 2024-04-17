using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
        public string? Description { get; set; } = null;
        public string Email { get; set; }
        [Required(ErrorMessage = "anlak")]
        [EnglishStringSyntax]
        [StringLength(maximumLength: 18, MinimumLength = 4, ErrorMessage = "Длина логина должна быть от 4 до 18 символов")]
        public string Login { get; set; }
        [StringLength(maximumLength: 18, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 4 до 18 символов")]
        [EnglishStringSyntax]
        public string Password { get; set; }
        public string Role { get; set; } = "User";
        public bool IsBanned { get; set; } = false;
        public List<UserPost>? UserPosts { get; set; } = null;
        public List<Comment>? UserComments { get; set; } = null;
    }
}