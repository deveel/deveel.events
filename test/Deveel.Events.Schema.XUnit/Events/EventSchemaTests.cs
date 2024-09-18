namespace Deveel.Events
{
    public static class EventSchemaTests
    {
        [Fact]
        public static void VersionedSchema_PropertiesWithoutVersion_Success()
        {
            var schema = new EventSchema("test", "1.0", "application/json");

            schema.Properties.Add("name", "string");
            schema.Properties.Add(new EventProperty("age", "int"));

            var name = schema.Properties["name"];
            Assert.NotNull(name);
            Assert.Equal("1.0", name.Version.ToString());

            var age = schema.Properties["age"];
            Assert.NotNull(age);
            Assert.Equal("1.0", age.Version.ToString());
        }

        [Fact]
        public static void VersionedSchema_PropertiesWithVersionLowerThanEvent_Success()
        {
            var schema = new EventSchema("test", "2.0", "application/json");

            schema.Properties.Add("name", "string", "1.0");
            schema.Properties.Add(new EventProperty("age", "int", "1.2"));

            var name = schema.Properties["name"];
            Assert.NotNull(name);
            Assert.Equal("1.0", name.Version.ToString());

            var age = schema.Properties["age"];
            Assert.NotNull(age);
            Assert.Equal("1.2", age.Version.ToString());
        }

        [Fact]
        public static void VersionedSchema_PropertiesWithVersionHigherThanEvent_Fail()
        {
            var schema = new EventSchema("test", "1.0", "application/json");

            Assert.Throws<ArgumentException>(() => schema.Properties.Add("name", "string", "2.0"));
            Assert.Throws<ArgumentException>(() => schema.Properties.Add(new EventProperty("age", "int", "2.0")));
        }

        [Fact]
        public static void VersionedSchema_PropertiesWithSameName_Fail()
        {
            var schema = new EventSchema("test", "1.0", "application/json");

            schema.Properties.Add("name", "string");

            Assert.Throws<ArgumentException>(() => schema.Properties.Add("name", "int"));
        }

        [Fact]
        public static void VersionedSchema_SetNewProperty_Fail()
        {
            var schema = new EventSchema("test", "1.0", "application/json");

            schema.Properties.Add("name", "string");

            var name = schema.Properties["name"];
            Assert.NotNull(name);
            Assert.Equal("1.0", name.Version.ToString());

            Assert.Throws<KeyNotFoundException>(() => schema.Properties["age"] = new EventProperty("age", "int", "1.0"));

            var age = schema.Properties["age"];
            Assert.Null(age);
        }

        [Fact]
        public static void VersionedSchema_SetExistingProperty_Success()
        {
            var schema = new EventSchema("test", "2.0", "application/json");

            schema.Properties.Add("name", "string", "1.0");

            var name = schema.Properties["name"];
            Assert.NotNull(name);
            Assert.Equal("1.0", name.Version.ToString());

            var newName = new EventProperty("name", "string", "1.1");
            newName.Constraints.Add(new EnumMemberConstraint<string>(new[] { "John", "Jane" }));

            schema.Properties["name"] = newName;

            name = schema.Properties["name"];
            Assert.NotNull(name);
            Assert.Equal("1.1", name.Version.ToString());
        }
    }
}
