﻿using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.FeatureVm
{
    public class FeatureUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Feature Title")]
        [Required(ErrorMessage = "Feature Title is required")]
        public string? Title { get; set; }

        [Display(Name = "Translate title to Russian language")]
        public string? TitleRu { get; set; }

        [Display(Name = "Translate title to Turkish language")]
        public string? TitleTr { get; set; }

        [Display(Name = "Feature Icon")]
        public IFormFile? Icon { get; set; }

        public string? IconPathOrContainer { get; set; }
        public string? IconName { get; set; }
    }
}
