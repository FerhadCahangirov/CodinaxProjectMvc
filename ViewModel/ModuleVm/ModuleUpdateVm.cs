using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.ModuleVm
{
    public class ModuleUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Title should be minimum 3 characters and a maximum of 250 characters")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        public ModuleUpdateVm(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public ModuleUpdateVm()
        {
            Title = string.Empty;
            Id = Guid.Empty;
        }

    }
}
