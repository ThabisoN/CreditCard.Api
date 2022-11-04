using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCard.Api.Model
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Card Number is required")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Expiry Date is required")]
        public string Expiry { get; set; }
        [Required(ErrorMessage = "CVC is required")]
        public int CVC { get; set; }

    }
}
