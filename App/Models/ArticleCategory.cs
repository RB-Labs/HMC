using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class ArticleCategory
    {
        public int Id { get; set; }
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}
