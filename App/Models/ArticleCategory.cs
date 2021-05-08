using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class ArticleCategory
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название категории должно содержать не менее 3 символов")]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
