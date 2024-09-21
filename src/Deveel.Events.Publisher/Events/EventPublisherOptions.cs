﻿//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using System.Text.Json;

namespace Deveel.Events {
    /// <summary>
    /// The options used to configure the <see cref="EventPublisher"/>.
    /// </summary>
    public class EventPublisherOptions {
        /// <summary>
        /// The default source of the events, used when the 
        /// source is not specified in the event.
        /// </summary>
        /// <remarks>
        /// It is recommended to set this value as a singleton 
        /// for the publisher, and not to specify it in the
        /// single events.
        /// </remarks>
        public Uri? Source { get; set; }

        /// <summary>
        /// Indicates if the publisher should throw exceptions
        /// when errors occur during the publishing of events.
        /// </summary>
		public bool ThrowOnErrors { get; set; } = false;

        /// <summary>
        /// The options to use for the JSON serialization of the
        /// content of the events.
        /// </summary>
		public JsonSerializerOptions? JsonSerializerOptions { get; set; } = new JsonSerializerOptions();

        /// <summary>
        /// A list of attributes to be added to all the events
        /// that are published by the publisher.
        /// </summary>
		public Dictionary<string, object?> Attributes { get; set; } = new Dictionary<string, object?>();

        /// <summary>
        /// A default base URI to use for the data schema of the events,
        /// when the schema is not specified in the event.
        /// </summary>
		public Uri? DataSchemaBaseUri { get; set; }
	}
}
