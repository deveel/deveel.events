//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

namespace Deveel.Events {
	public sealed class EventGuidGeneratorOptions {
		public string? Format { get; set; } = EventGuidGenerator.DefaultFormat;
	}
}
