using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Web.Data
{
    public class Address
    {
        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
