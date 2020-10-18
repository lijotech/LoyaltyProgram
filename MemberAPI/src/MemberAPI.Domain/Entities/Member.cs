using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MemberAPI.Domain.Entities
{
    public partial class Member
    {
        public Guid MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string MobileNo { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EntryBy { get; set; }        
        public DateTime EntryDate { get; set; }
        [Description("Details are their in the core API, common payment categories are 1=Gold,2=Silver,3=Bronze,4=Guest")]
        public int PaymentCategory { get; set; }
        [Description("The details of the status information is there in the core API")]
        public int MemberStatus { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string ForgotPasswordConfirmationToken { get; set; }
    }
}
