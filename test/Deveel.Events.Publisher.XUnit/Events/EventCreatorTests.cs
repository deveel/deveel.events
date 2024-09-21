using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deveel.Events
{
    public class EventCreatorTests
    {
        public EventCreatorTests()
        {
            var services = new ServiceCollection();
            services.AddEventPublisher();

            var provider = services.BuildServiceProvider();

            EventCreator = provider.GetRequiredService<IEventCreator>();
        }

        private IEventCreator EventCreator { get; }
        [Fact]
        public void CreateFromData()
        {
            var @event = EventCreator.CreateEventFromData(new PersonCreated
            {
                FirstName = "John",
                LastName = "Doe",
                Id = "12345",
                Age = 30
            });

            Assert.Equal("person.created", @event.Type);
            Assert.Equal("https://deveel.com/events/person/schema/1.0", @event.DataSchema.ToString());
            Assert.Null(@event.Id);
            Assert.Null(@event.Source);

            var attributes = @event.GetPopulatedAttributes()?.ToDictionary(x => x.Key.Name, y => y.Value);

            Assert.NotNull(attributes);
            Assert.NotEmpty(attributes);
            Assert.Contains("streamtype", attributes.Keys);
            Assert.Equal("person", attributes["streamtype"]);
            Assert.Equal("application/cloudevents+json", @event.DataContentType);

            var json = Assert.IsType<string>(@event.Data);

            Assert.NotNull(json);

            var personCreated = JsonSerializer.Deserialize<PersonCreated>(json);

            Assert.NotNull(personCreated);
            Assert.Equal("John", personCreated.FirstName);
            Assert.Equal("Doe", personCreated.LastName);
            Assert.Equal("12345", personCreated.Id);
            Assert.Equal(30, personCreated.Age);
        }

        [Fact]
        public void CreateFromDataWithoutEventAttribute()
        {
            Assert.Throws<ArgumentException>(() => EventCreator.CreateEventFromData(new { Name = "John" }));
        }


        [Event("person.created", "https://deveel.com/events/person/schema/1.0")]
        [EventAttributes("streamtype", "person")]
        class PersonCreated
        {
            [JsonPropertyName("first_name"), Required]
            public string FirstName { get; set; }

            [JsonPropertyName("last_name"), Required]

            public string LastName { get; set; }

            [JsonPropertyName("id"), Required]
            public string Id { get; set; }

            [JsonPropertyName("age")]
            public int? Age { get; set; }

            [JsonPropertyName("email")]
            public Email? Email { get; set; }
        }

        class Email
        {
            [JsonPropertyName("display_name")]
            public string? DisplayName { get; set; }

            [JsonPropertyName("address"), Required]
            public string Address { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }
        }
    }
}
