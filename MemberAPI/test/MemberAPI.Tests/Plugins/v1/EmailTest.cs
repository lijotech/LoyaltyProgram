using FakeItEasy;
using MemberAPI.Service.Plugins.v1;
using System;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Text;
using Xunit;

namespace MemberAPI.Tests.Plugins.v1
{
    public class EmailTest
    {
        private  EmailSender _emailSender;
        private readonly EmailConfiguration _emailConfig;
        public EmailTest()
        {          
            _emailConfig = Configuration.InitConfiguration.InitConfigurationTest()
                .GetSection("EmailConfiguration").Get<EmailConfiguration>();
            _emailSender = new EmailSender(_emailConfig);
        }

        [Fact]
        public  void Email_Check_If_Email_Delivered_Succesfully()
        {
            var message = new Message(new string[] { "lijotech@gmail.com" },
          "Email Confirmation", "Click this link to Continue:", null);
            int i=_emailSender.SendEmail(message);
            Assert.Equal(1, i);
        }
    }
}
