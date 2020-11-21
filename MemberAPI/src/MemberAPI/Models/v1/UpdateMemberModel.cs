using System;

using System.ComponentModel.DataAnnotations;


namespace MemberAPI.Models.v1
{
    public class UpdateMemberModel
    {
        [Required]
        public Guid MemberId { get; set; }
        
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        public string DOB { get; set; }
       
        public string Gender { get; set; }
    
        public string Nationality { get; set; }
      
        public string MobileNo { get; set; }
       
        public int? CountryId { get; set; }
        
        public int? StateId { get; set; }

        public int? MemberStatus { get; set; }
        public int? PaymentCategory { get; set; }              

    }
}
