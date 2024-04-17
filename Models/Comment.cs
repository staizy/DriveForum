namespace DriveForum.Models
{
    public class Comment
    {
        public Comment() { }

        public int Id { get; set; }
        /// <summary>
        /// Айди поста, на котором оставляем коммент
        /// </summary>
        public int PostId { get; set; }
        public string Context { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public bool IsHidden { get; set; } = false;
        /// <summary>
        /// Айди юзера, который оставляет коммент
        /// </summary>
        public int UserId { get; set; }
    }
}
