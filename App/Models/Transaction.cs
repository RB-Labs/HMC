using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public User Customer { get; set; }

        [Display(Name = "Значение баланса")]
        [Required(ErrorMessage = "Необходимо задать значение")]
        public double Value { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Описание должно содержать не менее 3 символов")]
        [MinLength(3)]
        public string Description { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime Date { get; set; }
    }
}
