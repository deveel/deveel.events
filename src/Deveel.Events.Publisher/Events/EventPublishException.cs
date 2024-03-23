namespace Deveel.Events {
	public class EventPublishException : Exception {
		public EventPublishException() {
		}

		public EventPublishException(string? message) : base(message) {
		}

		public EventPublishException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
