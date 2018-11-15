using Autofac;
using Autofac.Core;
using BetingSystem.RestApi;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BetingSystem.Tests
{
    public class ServiceContainerTest
    {
        [Fact]
        public void Run()
        {
            var container = ServiceContainerFactory.Create(new ServiceCollection()).BeginLifetimeScope();

            foreach (var registry in container.ComponentRegistry.Registrations)
            {
                foreach (TypedService serviceDescriptor in registry.Services)
                {
                    var service = container.Resolve(serviceDescriptor.ServiceType);
                    service.Should().NotBeNull();
                }
            }
        }
    }
}
