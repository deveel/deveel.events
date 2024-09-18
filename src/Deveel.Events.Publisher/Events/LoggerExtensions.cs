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
		[LoggerMessage(-30001, LogLevel.Error, "Could not create the event of type '{EventType}'")]
		public static partial void LogEventCreateError(this ILogger logger, Exception ex, Type eventType);

		[LoggerMessage(-30002, LogLevel.Error, "Could not create the event from the factory of type '{FactoryType}'")]
        public static partial void LogEventFactoryError(this ILogger logger, Exception ex, Type factoryType);

        [LoggerMessage(-30002, LogLevel.Error, "Could not publish the event of type '{EventType}' through the channel of type '{ChannelType}'")]
		public static partial void LogEventPublishError(this ILogger logger, Exception ex, string eventType, Type channelType);

		[LoggerMessage(400012, LogLevel.Debug, "Publishing an event of type '{EventType}' through the channel of type '{ChannelType}'")]
		public static partial void TraceEventPublishing(this ILogger logger, string eventType, Type channelType);

		[LoggerMessage(400013, LogLevel.Debug, "The event of type '{EventType}' was successfully published through the channel of type '{ChannelType}'")]
		public static partial void TraceEventPublished(this ILogger logger, string eventType, Type channelType);
	}
}
