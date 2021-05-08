using App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace App.ViewModels
{
    public class ArticleViewModel
    {
        public ArticleViewModel()
        {
        }

        public ArticleViewModel(IEnumerable<ArticleCategory> articleCategories)
        {
            Categories = articleCategories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        }

        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название статьи должно содержать не менее 3 символов")]
        [MinLength(3)]
        public string Name { get; set; }

        [Display(Name = "Текст статьи")]
        [Required(ErrorMessage = "Текст статьи должен содержать не менее 5 символов")]
        [MinLength(5)]
        public string Text { get; set; }


        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Необходимо выбрать категорию")]
        public string CategoryId { get; set; }

        [Display(Name = "Автор")]
        [Required]
        public string Author { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
