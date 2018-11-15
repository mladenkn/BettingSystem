using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BetingSystem.DAL;
using Xunit;

namespace BetingSystem.Tests
{
    public class AutoMapperProfilesTest
    {
        [Fact]
        public void Run()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MapperProfile()));
            config.AssertConfigurationIsValid();
        }
    }
}
