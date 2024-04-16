using System.ComponentModel.DataAnnotations;

namespace SavourOdessa.Areas.Admin.Models.Owners
{
    public class OwnerEditViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
