using System.Drawing;

namespace Deveel.Events {
	public static class ConstraintTests {
		[Theory]
		[InlineData("name", true)]
		[InlineData(null, false)]
		public static void TestIsRequired(object? value, bool valid) {
			IEventPropertyConstraint constraint = new PropertyRequiredConstraint();
			Assert.Equal(valid, constraint.IsValid(value));
		}

		[Theory]
		[InlineData(22, 34, 11, false)]
		[InlineData(22, 34, 35, false)]
		[InlineData(22, 34, 25, true)]
		[InlineData(null, 34, 25, true)]
		[InlineData(22, null, 25, true)]
		public static void TestRange(int? min, int? max, int value, bool valid) {
			IEventPropertyConstraint constraint = new RangeConstraint<int>(min, max);
			Assert.Equal(valid, constraint.IsValid(value));
		}

		[Theory]
		[InlineData(typeof(KnownColor), nameof(KnownColor.LawnGreen), true)]
		[InlineData(typeof(KnownColor), "fooBar", false)]
		public static void TestEnumMember(Type enumType, string value, bool valid) {
			var names = Enum.GetNames(enumType);
			IEventPropertyConstraint constraint = new EnumMemberConstraint<string>(names);

			Assert.Equal(valid, constraint.IsValid(value));
		}
	}
}
