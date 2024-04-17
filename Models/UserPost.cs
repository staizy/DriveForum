namespace DriveForum.Models
{
    public class UserPost
    {
        public UserPost() { }

        public int Id { get; set; }
        /// <summary>
        /// Айди машины, про которую пост
        /// </summary>
        public int CarModelId { get; set; }
        public string Title { get; set; }
        public string Main { get; set; }
        public string MainPhotoUrl { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsModerated { get; set; } = false;
        /// <summary>
        /// Айди юзера, который сделал пост
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Список комментов, относяшихся к посту
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}