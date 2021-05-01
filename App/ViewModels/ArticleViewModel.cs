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
        public string Name { get; set; }
        public string Text { get; set; }
        [Display(Name = "Category")]
        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
