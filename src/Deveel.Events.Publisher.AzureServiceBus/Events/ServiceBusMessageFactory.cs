﻿// 
//  Copyright 2023-2024 Antonello Provenzano
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System.Text;
using System.Text.Json;

using Azure.Messaging.ServiceBus;

using CloudNative.CloudEvents;

namespace Deveel.Events {
    /// <summary>
    /// A service that is responsible for creating a <see cref="ServiceBusMessage"/>
	/// instance from a <see cref="CloudEvent"/>.
    /// </summary>
    public class ServiceBusMessageFactory {
        /// <summary>
        /// Gets the binary data from the given content type and data.
        /// </summary>
        /// <param name="contentType">
		/// The content type of the data.
		/// </param>
        /// <param name="data">
		/// The data to be converted to binary.
		/// </param>
        /// <returns>
		/// Returns an instance of <see cref="BinaryData"/> that represents the
		/// data in binary format.
		/// </returns>
        /// <exception cref="ArgumentException">
		/// Thrown when the content type is not valid.
		/// </exception>
        /// <exception cref="NotSupportedException">
		/// Thrown when the content type of the event data is not supported.
		/// </exception>
        protected virtual BinaryData? GetBinaryData(string? contentType, object? data) {
			if (contentType == null)
				return null;

			var typePart = contentType.Substring(contentType.IndexOf('/') + 1);
			if (typePart.Length < 2)
                throw new ArgumentException("The content type is not valid");

			BinaryData binaryData;
			if (data == null) {
				return null;
			} else if (typePart.EndsWith("binary") ||
				typePart == "octet-stream") {
				if (data is byte[] bytes)
				{
					binaryData = new BinaryData(bytes);
				} else if (data is string s)
				{
                    var binary = Convert.FromBase64String(s);
                    binaryData = new BinaryData(binary);
				} else
				{
					throw new ArgumentException("The data is not a valid binary format");
				}
			} else if (typePart.EndsWith("json")) {
				if (data is string s)
				{
					binaryData = new BinaryData(Encoding.UTF8.GetBytes(s));
				} else
				{
					// TODO: json options ...
                    var json = JsonSerializer.Serialize(data);
                    binaryData = new BinaryData(Encoding.UTF8.GetBytes(json));
                }
			} else {
				throw new NotSupportedException("The content type of the event data is not supported");
			}

			return binaryData;
		}

        /// <summary>
        /// Gets the subject of the event.
        /// </summary>
        /// <param name="event">
		/// The event to get the subject from.
		/// </param>
        /// <returns>
		/// Returns the subject of the event.
		/// </returns>
        protected virtual string? GetSubject(CloudEvent @event) => @event.Subject;

        // TODO: get the correlation id from the event
        //       from a configured attribute
        /// <summary>
        /// Gets the identifier to be used to correlate the event
		/// in the stream of messages.
        /// </summary>
        protected virtual string GetCorrelationId(CloudEvent @event) => "";

        /// <summary>
        /// Adds the event properties to the set of properties of a message.
        /// </summary>
        /// <param name="properties">
		/// The set of properties to add the event properties to.
		/// </param>
        /// <param name="event">
		/// The event to extract the properties from.
		/// </param>
        protected virtual void AddProperties(IDictionary<string, object> properties, CloudEvent @event)
		{
			if (@event.DataSchema != null)
                properties.Add(ServiceBusMessageProperties.DataVersion, @event.DataSchema.ToString());

			properties.Add(ServiceBusMessageProperties.EventType, @event.Type!);
			properties.Add(ServiceBusMessageProperties.TimeStamp, @event.Time!);

			foreach (var item in @event.GetPopulatedAttributes())
			{
				properties.Add(item.Key.Name, item.Value);
			}
		}

        /// <summary>
        /// Creates a new instance of <see cref="ServiceBusMessage"/> 
		/// from the given event.
        /// </summary>
        /// <param name="event">
		/// The event to create the message from.
		/// </param>
        /// <returns>
		/// Returns a new instance of <see cref="ServiceBusMessage"/>
		/// that represents the event.
		/// </returns>
        public ServiceBusMessage CreateMessage(CloudEvent @event)
		{
			var body = GetBinaryData(@event.DataContentType, @event.Data);

			var message = new ServiceBusMessage
			{
				Body = body,
				MessageId = @event.Id,
				ContentType = @event.DataContentType,
				Subject = GetSubject(@event),
				CorrelationId = GetCorrelationId(@event)
				// TODO: extract the partition key from the event
			};

			AddProperties(message.ApplicationProperties, @event);

			return message;
		}
	}
}
