using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace PowerUtils.AspNetCore.ErrorHandler.Tests.Tests
{
    public sealed class ErrorHandlerExtensionsTests
    {
        [Fact]
        public void Whenn_add_error_handler_after_configure_api_behavior_options_should_return_default_client_error_mapping()
        {
            // Arrange
            var status = 504;
            var title = "fakeTitle";
            var link = "fakeLink";

            var services = new ServiceCollection();
            services.Configure<ApiBehaviorOptions>(options => options.ClientErrorMapping.Add(status, new ClientErrorData
            {
                Title = title,
                Link = link
            }));

            services.AddErrorHandler();

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            options.Value.ClientErrorMapping.TryGetValue(status, out var act);


            // Assert
            act.Title.Should().NotBe(title);
            act.Link.Should().NotBe(link);
        }

        [Fact]
        public void Whenn_add_error_handler_before_configure_api_behavior_options_should_return_override_client_error_mapping()
        {
            // Arrange
            var status = 504;
            var title = "fakeTitle";
            var link = "fakeLink";

            var services = new ServiceCollection();

            services.AddErrorHandler();
            services.Configure<ApiBehaviorOptions>(options => options.ClientErrorMapping[status] = new ClientErrorData
            {
                Title = title,
                Link = link
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            options.Value.ClientErrorMapping.TryGetValue(status, out var act);


            // Assert
            act.Title.Should().Be(title);
            act.Link.Should().Be(link);
        }

        [Fact]
        public void Add_client_error_mapping_with_default()
        {
            // Arrange
            var status = 774;
            var title = "mockeTitle";
            var link = "mockerLink";

            var services = new ServiceCollection();
            services.Configure<ApiBehaviorOptions>(options => options.ClientErrorMapping.Add(status, new ClientErrorData
            {
                Title = title,
                Link = link
            }));

            services.AddErrorHandler();

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ApiBehaviorOptions>>();


            // Act
            options.Value.ClientErrorMapping.TryGetValue(status, out var act);


            // Assert
            act.Title.Should().Be(title);
            act.Title.Should().Be(title);
            act.Link.Should().Be(link);
        }

        [Fact]
        public void When_property_naming_policy_does_not_set_should_use_camel_case()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddErrorHandler();

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ErrorHandlerOptions>>();


            // Act
            var act = options.Value.PropertyHandler("FakeTitle");


            // Assert
            act.Should().Be("fakeTitle");
        }

        [Fact]
        public void When_property_naming_policy_set_should_use_setted_policy()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddErrorHandler(o => o.PropertyNamingPolicy = PropertyNamingPolicy.SnakeCase);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ErrorHandlerOptions>>();


            // Act
            var act = options.Value.PropertyHandler("FakeTitle");


            // Assert
            act.Should().Be("fake_title");
        }
    }
}
