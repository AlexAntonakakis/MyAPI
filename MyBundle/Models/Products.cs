using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyBundle.Models
{

    public class Product
    {
        public int Id { get; set; }
        
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name should be between 1 and 100 characters")]
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, float.MaxValue)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Code is required")]
        public int Code { get; set; }
        public DateTime? ExpDate { get; set; }
        [Required(ErrorMessage = "Available Date is required")]
        public DateTime AvailDate { get; set; }

        public int Active { get; set; } = 1;

    }
}