using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodinaxProjectMvc.DataAccess.Models
{
    [Table("Subscribers")]
    public class Subscriber : BaseEntity
    {
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string TokenValidationKey { get; set; }

        public Subscriber(string email, string tokenValidationKey)
        {
            Email = email;
            IsEmailConfirmed = false;
            TokenValidationKey = tokenValidationKey;
        }

        public Subscriber()
        {
            Email = string.Empty;
            TokenValidationKey = string.Empty;
        }
    }
}
