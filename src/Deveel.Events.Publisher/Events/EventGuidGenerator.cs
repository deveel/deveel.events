//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using Microsoft.Extensions.Options;

namespace Deveel.Events {
	public sealed class EventGuidGenerator : IEventIdGenerator {
		private readonly EventGuidGeneratorOptions _options;

		public const string DefaultFormat = "N";

		public EventGuidGenerator(IOptions<EventGuidGeneratorOptions> options) {
			_options = options?.Value ?? new EventGuidGeneratorOptions();
		}

		public static readonly EventGuidGenerator Default 
			= new EventGuidGenerator(Options.Create(new EventGuidGeneratorOptions()));

		public string GenerateId() => Guid.NewGuid().ToString(_options.Format ?? DefaultFormat);
	}
}
