using System.ComponentModel.DataAnnotations.Schema;

namespace DriveForum.Models
{
    public class UserPost
    {
        public UserPost(Car car, string title, string main, IFormFile? mainPhoto)
        {
            Car = car;
            Title = title;
            Main = main;
            MainPhotoUrl = "";
        }

        public UserPost() { }

        public int Id { get; set; }
        /// <summary>
        /// Машина, про которую пост
        /// </summary>
        public Car Car { get; set; }
        public string Title { get; set; }
        public string Main { get; set; }
        public string? MainPhotoUrl { get; set; } = "";
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsModerated { get; set; } = false;
        /// <summary>
        /// Юзер, который сделал пост
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Список комментов, относяшихся к посту
        /// </summary>
        public List<Comment> Comments { get; set; } = new();
    }
}