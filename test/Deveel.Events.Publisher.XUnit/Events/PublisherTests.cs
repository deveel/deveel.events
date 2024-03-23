using System.Text.Json.Serialization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Deveel.Events {
	public class PublisherTests
    {
		public PublisherTests(ITestOutputHelper outputHelper) {
			var services = new ServiceCollection()
				.AddLogging(logging => logging.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Debug));

			var builder = services
				.AddEventPublisher(options => {
					options.Source = "test";
					options.Attributes.Add("env", "test");
				})
				.AddTestChannel(@event => Events.Add(@event));

			var provider = services.BuildServiceProvider();
			Publisher = provider.GetRequiredService<EventPublisher>();
		}

		private IList<IEvent> Events { get; } = new List<IEvent>();

		private EventPublisher Publisher { get; }

		[Fact]
		public async void PublishSimpleEvent() {
			var @event = new Event("person.created", "1.0") {
				EventData = new EventData(new {
					FirstName = "John",
					LastName = "Doe"
				})
			};

			await Publisher.PublishEventAsync(@event);

			Assert.Single(Events);
			Assert.Equal("person.created", Events[0].EventType);
			Assert.Equal("1.0", Events[0].DataVersion);

			Assert.NotNull(Events[0].EventId);
			Assert.Equal("test", Events[0].Source);
			Assert.Equal("test", Events[0].Attributes["env"]);
		}

		[Fact]
		public async Task PublishEventData() {
			await Publisher.PublishAsync(new PersonCreated {
				Id = "123",
				FirstName = "John",
				LastName = "Doe"
			});

			Assert.Single(Events);
			Assert.Equal("person.created", Events[0].EventType);
			Assert.Equal("1.0", Events[0].DataVersion);
			Assert.NotNull(Events[0].EventId);
			Assert.Equal("test", Events[0].Source);
			Assert.Equal("test", Events[0].Attributes["env"]);

			Assert.Equal(EventContentType.Object, Events[0].EventData.ContentType);
			Assert.IsType<PersonCreated>(Events[0].EventData.Content);
		}

		[Event("person.created", "1.0")]
		class PersonCreated {
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("first_name")]
			public string FirstName { get; set; }

			[JsonPropertyName("last_name")]
			public string LastName { get; set; }
		}
    }
}
