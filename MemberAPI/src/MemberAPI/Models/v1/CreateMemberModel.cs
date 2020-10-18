using MemberAPI.Validators.v1;
using System;
using System.ComponentModel.DataAnnotations;


namespace MemberAPI.Models.v1
{
    public class CreateMemberModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required] 
        public string DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        public string Nationality { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int StateId { get; set; }        
        [Required]
        public string Password { get; set; }     
      
    }
}
