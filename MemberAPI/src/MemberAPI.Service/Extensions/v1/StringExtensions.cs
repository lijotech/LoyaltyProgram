using System;
using System.Text;

namespace MemberAPI.Service.Extensions.v1
{
    public static class StringExtensions
    {
        public static byte[] ConvertToByte(this string str)
        {
          return  Encoding.ASCII.GetBytes(str);
        }
    }
}