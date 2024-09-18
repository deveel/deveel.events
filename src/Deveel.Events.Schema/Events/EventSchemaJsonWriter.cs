//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using System.Text.Json;

namespace Deveel.Events {
    /// <summary>
    /// An implementation of <see cref="IEventSchemaWriter"/> that writes
	/// the schema of an event to a JSON stream.
    /// </summary>
    public sealed class EventSchemaJsonWriter : IEventSchemaWriter {
        /// <summary>
        /// Constructs the writer with the given options to configure
		/// the JSON writer.
        /// </summary>
        /// <param name="jsonWriterOptions">
		/// The options that are used to configure the JSON writer.
		/// </param>
        public EventSchemaJsonWriter(JsonWriterOptions? jsonWriterOptions = null) {
			JsonWriterOptions = jsonWriterOptions ?? new System.Text.Json.JsonWriterOptions();
		}

        /// <summary>
        /// The options that are used to configure the JSON writer.
        /// </summary>
        public JsonWriterOptions JsonWriterOptions { get; }

        /// <inheritdoc/>
        public async Task WriteToAsync(Stream stream, IEventSchema schema, CancellationToken cancellationToken = default) {
			using var writer = new Utf8JsonWriter(stream, JsonWriterOptions);
			writer.WriteStartObject();

			writer.WritePropertyName("type");
			writer.WriteStringValue(schema.EventType);

			writer.WritePropertyName("version");
			writer.WriteStringValue(schema.Version);

			writer.WritePropertyName("contentType");
			writer.WriteStringValue(schema.ContentType);

			writer.WritePropertyName("description");
			writer.WriteStringValue(schema.Description);

			writer.WritePropertyName("properties");
			writer.WriteStartObject();
			foreach (var property in schema.Properties) {
				WriteProperty(writer, property);
			}
			writer.WriteEndObject();
			writer.WriteEndObject();

			await writer.FlushAsync(cancellationToken);
		}

		private void WriteProperty(Utf8JsonWriter writer, IEventProperty property) {
			writer.WritePropertyName(property.Name);
			writer.WriteStartObject();

			writer.WritePropertyName("dataType");
			writer.WriteStringValue(property.DataType);

			writer.WritePropertyName("version");
			writer.WriteStringValue(property.Version);

			writer.WritePropertyName("description");
			writer.WriteStringValue(property.Description);

			if (property.Constraints?.Any() ?? false) {
				foreach (var constraint in property.Constraints) {
					WriteConstraint(writer, constraint);
				}
			}

			if (property.Properties?.Any() ?? false) {
				writer.WritePropertyName("properties");
				writer.WriteStartObject();
				foreach (var subProperty in property.Properties) {
					WriteProperty(writer, subProperty);
				}
				writer.WriteEndObject();
			}

			writer.WriteEndObject();
		}

		private void WriteConstraint(Utf8JsonWriter writer, IEventPropertyConstraint constraint) {
			if (constraint is PropertyRequiredConstraint) {
				writer.WritePropertyName("required");
				writer.WriteBooleanValue(true);
			} else if (constraint is EnumMemberConstraint<string> enumConstraint) {
				writer.WritePropertyName("allowedValues");
				writer.WriteStartArray();
				foreach (var value in enumConstraint.AllowedValues) {
					writer.WriteStringValue(value);
				}
				writer.WriteEndArray();
			}

			var constraintType = constraint.GetType();
			if (constraintType.IsGenericType && 
				constraintType.GetGenericTypeDefinition() == typeof(RangeConstraint<>)) {
				var dataType = constraintType.GetGenericArguments()[0];
				var min = constraintType.GetProperty("Min")?.GetValue(constraint);
				var max = constraintType.GetProperty("Max")?.GetValue(constraint);

				if (min != null) {
					writer.WritePropertyName("min");
					WritePropertyValue(writer, dataType, min);
				}

				if (max != null) {
					writer.WritePropertyName("max");
					WritePropertyValue(writer, dataType, max);
				}
			}
		}

		private static void WritePropertyValue(Utf8JsonWriter writer, Type valueType, object? value) {
			if (value == null) {
				writer.WriteNullValue();
				return;
			}

			if (valueType == typeof(int))
				writer.WriteNumberValue((int)value);
			else if (valueType == typeof(long))
				writer.WriteNumberValue((long)value);
			else if (valueType == typeof(float))
				writer.WriteNumberValue((float)value);
			else if (valueType == typeof(double))
				writer.WriteNumberValue((double)value);
			else if (valueType == typeof(decimal))
				writer.WriteNumberValue((decimal)value);
			else if (valueType == typeof(string))
				writer.WriteStringValue((string)value);
			else
				throw new NotSupportedException($"The data type {valueType} is not supported");
		}
	}
}
