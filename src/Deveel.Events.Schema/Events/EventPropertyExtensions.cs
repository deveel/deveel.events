namespace Deveel.Events {
	public static class EventPropertyExtensions {
		public static bool IsRequired(this EventProperty property)
			=> property.Constraints?.OfType<PropertyRequiredConstraint>().Any() ?? false;
	}
}
