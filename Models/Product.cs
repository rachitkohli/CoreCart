using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }
        [Display(Name="The Title")]
        [Required]
        [MinLength(7)]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string FilePath { get; set; }
    }
}
