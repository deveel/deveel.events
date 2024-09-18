// 
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

using Microsoft.Extensions.Logging;

namespace Deveel.Events {
	static partial class LoggerExtensions {
		[LoggerMessage(30001, LogLevel.Debug, "Event of type {EventType} to be published")]
		public static partial void TracePublishingEvent(this ILogger logger, string? eventType);

		[LoggerMessage(30002, LogLevel.Error, "Error while publishing an event of type '{EventType}'")]
		public static partial void LogErrorPublishingEvent(this ILogger logger, Exception ex, string? eventType);
	}
}
