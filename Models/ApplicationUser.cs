﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCart.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "(0:yyyy-MM-dd)", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
    }
}
