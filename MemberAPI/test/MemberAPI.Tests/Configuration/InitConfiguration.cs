using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemberAPI.Tests.Configuration
{
    public class InitConfiguration
    {
        public static IConfiguration InitConfigurationTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

    }
}
