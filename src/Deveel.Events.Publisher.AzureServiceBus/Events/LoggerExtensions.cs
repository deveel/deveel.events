﻿using Microsoft.Extensions.Logging;

namespace Deveel.Events {
	static partial class LoggerExtensions {
		[LoggerMessage(30001, LogLevel.Debug, "Event of type {EventType} to be published")]
		public static partial void TracePublishingEvent(this ILogger logger, string eventType);

		[LoggerMessage(30002, LogLevel.Error, "Error while publishing an event of type '{EventType}'")]
		public static partial void LogErrorPublishingEvent(this ILogger logger, Exception ex, string eventType);
	}
}
