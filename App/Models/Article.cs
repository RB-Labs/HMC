using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название статьи должно содержать не менее 3 символов")]
        [MinLength(3)]
        public string Name { get; set; }

        [Display(Name = "Текст статьи")]
        [Required(ErrorMessage = "Текст статьи должен содержать не менее 5 символов")]
        [MinLength(5)]
        public string Text { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать категорию")]
        public ArticleCategory Category { get; set; }

        [Required]
        public User Author { get; set; }

        [Display(Name = "Дата создания")]
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
