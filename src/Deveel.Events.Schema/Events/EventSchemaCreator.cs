using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Deveel.Events {
	static class EventSchemaCreator {
		public static EventSchema FromEventData(Type dataType) {
			var attribute = dataType.GetCustomAttribute<EventAttribute>();
			if (attribute == null)
				throw new ArgumentException($"The type {dataType} is not an event data type");

			var schema = new EventSchema(attribute.EventType, attribute.DataSchema, "object") {
				Description = attribute.Description
			};

			var properties = GetProperties(dataType);

			foreach (var property in properties) {
				schema.Properties.Add(property);
			}

			return schema;
		}

		private static string? GetEventVersion(MemberInfo member) {
			var baseType = member.DeclaringType;
			var attribute = baseType?.GetCustomAttribute<EventAttribute>();

			while (attribute == null && baseType != null) {
				baseType = baseType.BaseType;
				attribute = baseType?.GetCustomAttribute<EventAttribute>();
			}

			return attribute?.DataSchema;
		}

		private static IEnumerable<EventProperty> GetProperties(Type dataType) {
			return dataType.GetMembers(BindingFlags.Instance | BindingFlags.Public)
				.Where(x => x.MemberType == MemberTypes.Property || x.MemberType == MemberTypes.Field)
				.Select(CreateEventProperty);
		}

		private static EventProperty CreateEventProperty(MemberInfo member) {
			var propertyName = member.Name;
			var defaultVersion = GetEventVersion(member);
			var version = defaultVersion;
			string? description = null;

			JsonPropertyNameAttribute? jsonProperty;
			var attribute = member.GetCustomAttribute<EventPropertyAttribute>();
			if (attribute != null) {
				propertyName = attribute.Name;
				version = attribute.Version ?? defaultVersion;
				description = attribute.Description;
			}
			if ((jsonProperty = member.GetCustomAttribute<JsonPropertyNameAttribute>()) != null &&
				String.IsNullOrWhiteSpace(propertyName)) {
				propertyName = jsonProperty.Name;
			}

			if (String.IsNullOrWhiteSpace(propertyName))
				propertyName = member.Name;

			var propertyType = member.MemberType == MemberTypes.Property
				? ((PropertyInfo) member).PropertyType
				: ((FieldInfo) member).FieldType;

			var dataType = GetDataType(propertyType);
			var property = new EventProperty(propertyName, dataType, version) {
				Description = description
			};

			var constraints = GetConstraints(member);
			foreach (var constraint in constraints) {
				property.Constraints.Add(constraint);
			}

			if (!IsPrimitiveType(propertyType)) {
				foreach (var subProperty in GetProperties(propertyType)) {
					property.Properties.Add(subProperty);
				}
			}

			return property;
		}

		private static bool IsPrimitiveType(Type propertyType) {
			return propertyType == typeof(string) ||
				propertyType == typeof(int) ||
				propertyType == typeof(long) ||
				propertyType == typeof(float) ||
				propertyType == typeof(double) ||
				propertyType == typeof(decimal) ||
				propertyType == typeof(bool) ||
				propertyType == typeof(Guid) ||
				propertyType == typeof(DateTime) ||
				propertyType == typeof(DateTimeOffset);
		}

		private static string GetDataType(Type propertyType) {
			var nullableType = Nullable.GetUnderlyingType(propertyType);
			if (nullableType != null)
				return GetDataType(nullableType);

			if (propertyType == typeof(string))
				return "string";
			if (propertyType == typeof(int))
				return "int";
			if (propertyType == typeof(long))
				return "long";
			if (propertyType == typeof(float))
				return "float";
			if (propertyType == typeof(double))
				return "double";
			if (propertyType == typeof(decimal))
				return "money";
			if (propertyType == typeof(bool))
				return "boolean";
			if (propertyType == typeof(DateTime))
				return "dateTime";
			if (propertyType == typeof(DateTimeOffset))
				return "dateTimeOffset";
			if (propertyType == typeof(Guid))
				return "guid";

			if (propertyType.IsArray) {
				var elementType = propertyType.GetElementType();
				return $"{GetDataType(elementType!)}[]";
			}

			if (propertyType.IsEnum) {
				// TODO: allow the specification of other type of allowed values (eg. int, string, etc)
				return "string";
			}

			return propertyType.FullName!;
		}

		private static IEnumerable<IEventPropertyConstraint> GetConstraints(MemberInfo member) {
			var attributes = member.GetCustomAttributes(true);

			foreach (var attribute in attributes) {
				if (attribute is RequiredAttribute) {
					yield return new PropertyRequiredConstraint();
				} else if (attribute is RangeAttribute range) {
					yield return CreateRangeConstraint(member, range);
				}
			}

			if (IsEnum(member, out var enumType)) {
				yield return CreateEnumConstraint(enumType);
			}
		}

		private static IEventPropertyConstraint CreateEnumConstraint(Type enumType) {
			// TODO: allow the specification of other type of allowed values (eg. int, string, etc)
			var values = Enum.GetNames(enumType);
			return new EnumMemberConstraint<string>(values);
		}

		private static bool IsEnum(MemberInfo x, [MaybeNullWhen(false)] out Type enumType) {
			var propertyType = x.MemberType == MemberTypes.Property
				? ((PropertyInfo) x).PropertyType
				: ((FieldInfo) x).FieldType;

			enumType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
			return enumType.IsEnum;
		}

		private static IEventPropertyConstraint CreateRangeConstraint(MemberInfo member, RangeAttribute range) {
			var memberType = member.MemberType == MemberTypes.Property
				? ((PropertyInfo) member).PropertyType
				: ((FieldInfo) member).FieldType;

			if (range.Maximum != null && !memberType.IsInstanceOfType(range.Maximum))
				throw new ArgumentException("The maximum value is not compatible with the member type", nameof(range));
			if (range.Minimum != null && !memberType.IsInstanceOfType(range.Minimum))
				throw new ArgumentException("The minimum value is not compatible with the member type", nameof(range));

			var constrainType = typeof(RangeConstraint<>).MakeGenericType(memberType);
			return (IEventPropertyConstraint) Activator.CreateInstance(constrainType, range.Minimum, range.Maximum)!;
		}
	}
}
