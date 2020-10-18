using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Models.v1
{
    public class ConfirmEmailModel
    {
        [Required]
        public string u { get; set; }

        [Required]
        public string t { get; set; }
    }
}
