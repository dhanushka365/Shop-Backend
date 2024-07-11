using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string ?Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string ?Password { get; set; }

    }
}
