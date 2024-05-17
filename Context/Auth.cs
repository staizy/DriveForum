using DriveForum.DatabaseContext;
using DriveForum.Models;

namespace DriveForum.Context
{
    public class Auth
    {
        public User? User { get; set; } = null;
        public Auth(IHttpContextAccessor ctx, ApplicationContext db)
        {
            if (ctx.HttpContext?.User.Identity != null)
            {
                User = db.Users.Where(o => o.Login == ctx.HttpContext.User.Identity.Name).FirstOrDefault();
            }
        }
    }
}
