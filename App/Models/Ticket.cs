using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название заявки должно содержать не менее 3 символов")]
        [MinLength(3)]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Описание заявки должно содержать не менее 5 символов")]
        [MinLength(5)]
        public string Text { get; set; }

        [Display(Name = "Автор")]
        public User Author { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime Date { get; set; }
    }
}
