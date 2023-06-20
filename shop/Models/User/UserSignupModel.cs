using Microsoft.AspNetCore.Mvc;

namespace shop.Models.User
{
    public class UserSignupModel
    {
        [FromForm(Name = "user-login")] // <input name="user-login"...
        public string Login { get; set; } = null!;

        [FromForm(Name = "user-password")]
        public string Password { get; set; } = null!;

        [FromForm(Name = "user-repeat")]
        public string RepeatPassword { get; set; } = null!;

        [FromForm(Name = "user-name")]
        public string? RealName { get; set; } = null!;

        [FromForm(Name = "user-email")]
        public string Email { get; set; } = null!;

        [FromForm(Name = "user-avatar")]
        public IFormFile AvatarFile { get; set; } = null!;

        [FromForm(Name = "user-confirm")]
        public Boolean Agree { get; set; } 
    }
}
