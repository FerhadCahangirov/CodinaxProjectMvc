using CodinaxProjectMvc.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.LayoutVm;
public class ContactVm
{
    public IEnumerable<Faq>? Faqs { get; set; }

    [Required(ErrorMessage = "The first name is required.")]
    [Display(Name = "First Name")]
    public string? Firstname { get; set; }

    [Required(ErrorMessage = "The last name is required.")]
    [Display(Name = "Last Name")]
    public string? Lastname { get; set; }

    [Required(ErrorMessage = "The email is required.")]
    [EmailAddress(ErrorMessage = "The email address is not valid.")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The phone number is required.")]
    [Phone(ErrorMessage = "The phone number is not valid.")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "The message is required.")]
    [Display(Name = "Message")]
    public string? Message { get; set; }
    public ContactVm(IEnumerable<Faq>? faqs) 
    {
        Faqs = faqs;
    }
    public ContactVm()
    {
        
    }
}
