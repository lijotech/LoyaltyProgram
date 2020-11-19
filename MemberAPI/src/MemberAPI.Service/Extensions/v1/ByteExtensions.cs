using System;
using System.Text;

namespace MemberAPI.Service.Extensions.v1
{
    public static class ByteExtensions
    {
        public static string ConvertToString(this byte[] bytes)
        {
          return  Encoding.ASCII.GetString(bytes);
        }
    }
}