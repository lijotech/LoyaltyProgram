using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberAPI.Service.Plugins.v1
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
