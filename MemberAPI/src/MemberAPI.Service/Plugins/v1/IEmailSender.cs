using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberAPI.Service.Plugins.v1
{
    public interface IEmailSender
    {
        int SendEmail(Message message);
        Task<int> SendEmailAsync(Message message);
    }
}
