using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public ArticleCategory Category { get; set; }
        public User Author { get; set; }
        [Display(Name = "Date")]
        public DateTime CreationDate { get; set; }
    }
}
