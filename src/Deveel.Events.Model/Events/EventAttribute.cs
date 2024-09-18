﻿using System;

namespace Deveel.Events {
    /// <summary>
    /// An attribute that is used to describe an event type and its
	/// versioned schema.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class EventAttribute : Attribute {
        /// <summary>
        /// Constructs an attribute that describes an event type
        /// and its version or the URL to the schema.
        /// </summary>
        /// <param name="eventType">
        /// The type of the event that is being described.
        /// </param>
        /// <param name="dataSchemaOrVersion">
        /// The URL to the schema or the version of the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the event type or the schema/version is <c>null</c>.
        /// </exception>
        public EventAttribute(string eventType, string dataSchemaOrVersion) {
			ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));
			ArgumentNullException.ThrowIfNull(dataSchemaOrVersion, nameof(dataSchemaOrVersion));

			EventType = eventType;

			if (Uri.TryCreate(dataSchemaOrVersion, UriKind.Absolute, out var uri))
			{
                DataSchema = uri;
            } else
			{
                DataVersion = dataSchemaOrVersion;
            }
		}

        /// <summary>
        /// The type of the event that is being described.
        /// </summary>
		public string EventType { get; }

        /// <summary>
        /// The URL to the schema of the event.
        /// </summary>
		public Uri? DataSchema { get; }

        /// <summary>
        /// The version of the event.
        /// </summary>
		public string? DataVersion { get; set; }

        /// <summary>
        /// A description of the event type.
        /// </summary>
		public string? Description { get; set; }
	}
}
