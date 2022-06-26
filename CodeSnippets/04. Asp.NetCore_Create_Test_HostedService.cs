using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Scheduler.Tdd
{
    public class SchedulerHostedServiceTests
    {
        private readonly IHostBuilder _hostBuilder;
        public SchedulerHostedServiceTests()
        {
            _hostBuilder = new HostBuilder()
                .ConfigureScheduler<SchedulerHostedService>()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((context, configBuilder) =>
                        {
                            configBuilder.AddEnvironmentVariables();
                        })
                        .ConfigureServices(services =>
                        {
                            services.AddControllers();
                        })
                        .Configure(appBuilder =>
                        {
                            appBuilder
                                .UseRouting()
                                .UseEndpoints(endpoints =>
                                {
                                    endpoints.Map("/api/healthz", async request =>
                                    {
                                        string payload = "All working!";
                                        request.Response.StatusCode = (int)HttpStatusCode.OK;
                                        await request.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(payload));
                                    });
                                });
                        })
                        .UseTestServer();
                });
        }

        [Fact]
        public async void TestServer_Should_ReplyResponse()
        {
            // Arrange
            var host = _hostBuilder.Build();
            host.Start();
            var client = host.GetTestClient();

            // Act
            var result = await client.GetAsync("/api/healthz");
            var content = await result.Content.ReadAsStringAsync();

            // Assert
            result.Should().NotBeNull()
                .And.Subject.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Be("All working!");
        }
                
        [Fact]
        public void SchedulerHostedService_Should_Be_InstantiatedSuccessfully()
        {
            // Arrange
            var jobFactory = new Mock<IJobFactory>();
            var schedulerFactory = new Mock<ISchedulerFactory>();
            var jobOptions = new Mock<List<JobOptions>>();

            // Act
            Action act = () => new SchedulerHostedService(jobFactory.Object, schedulerFactory.Object, jobOptions.Object);

            // Assert
            act.Should().NotBeNull();
        }

        [Fact]
        public void SchedulerHostedService_Should_FailIfMissingIJobFactory()
        {
            // Arrange
            var schedulerFactory = new Mock<ISchedulerFactory>();
            var jobOptions = new Mock<List<JobOptions>>();

            // Act
            Action act = () => new SchedulerHostedService(null, schedulerFactory.Object, jobOptions.Object);

            // Assert
           act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'jobFactory')"); ;
        }

        [Fact]
        public void SchedulerHostedService_Should_FailIfMissingISchedulerFactory()
        {
            // Arrange
            var jobFactory = new Mock<IJobFactory>();
            var jobOptions = new Mock<List<JobOptions>>();

            // Act
            Action act = () => new SchedulerHostedService(jobFactory.Object, null, jobOptions.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                 .WithMessage("Value cannot be null. (Parameter 'schedulerFactory')");
        }

        [Fact]
        public void SchedulerHostedService_Should_FailIfMissingJobOptions()
        {
            // Arrange
            var jobFactory = new Mock<IJobFactory>();
            var schedulerFactory = new Mock<ISchedulerFactory>();

            // Act
            Action act = () => new SchedulerHostedService(jobFactory.Object, schedulerFactory.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                 .WithMessage("Value cannot be null. (Parameter 'jobOptions')");
        }
                
    }
}
