using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Infrastructure
{
    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            DateTime dtCheck=DateTime.MinValue;
            string[] formats = { "dd-MMM-yyyy" };
            DateTime.TryParseExact(source, formats,
                             System.Globalization.CultureInfo.InvariantCulture,
                             DateTimeStyles.None, out dtCheck);
            

            return dtCheck;
        }
    }
}
