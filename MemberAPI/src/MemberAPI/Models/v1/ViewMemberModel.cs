using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Models.v1
{
    public class ViewMemberModel
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
        public DateTime EntryDate { get; set; }
        public int PaymentCategory { get; set; }
        public int MemberStatus { get; set; }
    }
}
