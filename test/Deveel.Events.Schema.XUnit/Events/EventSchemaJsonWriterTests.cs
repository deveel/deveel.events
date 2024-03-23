using System.Text.Json;
using System.Text.Json.Nodes;

namespace Deveel.Events {
	public static class EventSchemaJsonWriterTests {
		[Fact]
		public static async Task WriteSimpleSchemaToJson() {
			var schema = new EventSchema("test", "1.0", "binary");
			schema.Properties.Add(new EventProperty("id", "string"));
			schema.Properties.Add(new EventProperty("name", "string"));
			schema.Properties.Add(new EventProperty("age", "int"));

			var writer = new EventSchemaJsonWriter();
			using var stream = new MemoryStream();
			await writer.WriteToAsync(stream, schema);

			stream.Position = 0;
			using var reader = new StreamReader(stream);
			var json = await reader.ReadToEndAsync();

			var obj = JsonSerializer.Deserialize<JsonNode>(json)!;
			Assert.NotNull(obj);
			Assert.NotNull(obj["type"]);
			Assert.Equal("test", obj["type"].GetValue<string>());
			Assert.NotNull(obj["version"]);
			Assert.Equal("1.0", obj["version"].GetValue<string>());
			Assert.NotNull(obj["contentType"]);
			Assert.Equal("binary", obj["contentType"].GetValue<string>());
			Assert.NotNull(obj["properties"]);

			var properties = obj["properties"].AsObject();

			Assert.NotNull(properties);
			Assert.NotNull(properties["id"]);
			Assert.NotNull(properties["id"]["dataType"]);
			Assert.Equal("string", properties["id"]["dataType"].GetValue<string>());

			Assert.NotNull(properties["name"]);
			Assert.NotNull(properties["name"]["dataType"]);
			Assert.Equal("string", properties["name"]["dataType"].GetValue<string>());

			Assert.NotNull(properties["age"]);
			Assert.NotNull(properties["age"]["dataType"]);
			Assert.Equal("int", properties["age"]["dataType"].GetValue<string>());
		}

		[Fact]
		public static async Task WriteSchemaWithConstraintsToJson() {
			var schema = new EventSchema("test", "1.0", "binary");
			schema.Properties.Add(new EventProperty("id", "string"));
			schema.Properties.Add(new EventProperty("name", "string"));
			schema.Properties.Add(new EventProperty("age", "int") {
				Constraints = {
					new PropertyRequiredConstraint(),
					new RangeConstraint<int>(14, 32)
				}
			});

			var writer = new EventSchemaJsonWriter();
			using var stream = new MemoryStream();
			await writer.WriteToAsync(stream, schema);

			stream.Position = 0;
			using var reader = new StreamReader(stream);
			var json = await reader.ReadToEndAsync();

			var obj = JsonSerializer.Deserialize<JsonNode>(json)!;
			Assert.NotNull(obj);
			Assert.NotNull(obj["type"]);
			Assert.Equal("test", obj["type"].GetValue<string>());
			Assert.NotNull(obj["version"]);
			Assert.Equal("1.0", obj["version"].GetValue<string>());
			Assert.NotNull(obj["contentType"]);
			Assert.Equal("binary", obj["contentType"].GetValue<string>());
			Assert.NotNull(obj["properties"]);

			var properties = obj["properties"].AsObject();
			Assert.NotNull(properties);
			Assert.NotNull(properties["age"]);
			Assert.NotNull(properties["age"]["dataType"]);
			Assert.Equal("int", properties["age"]["dataType"].GetValue<string>());
			Assert.NotNull(properties["age"]["required"]);
			Assert.True(properties["age"]["required"].GetValue<bool>());
			Assert.NotNull(properties["age"]["min"]);
			Assert.Equal(14, properties["age"]["min"].GetValue<int>());
			Assert.NotNull(properties["age"]["max"]);
			Assert.Equal(32, properties["age"]["max"].GetValue<int>());
		}
	}
}
