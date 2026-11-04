// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.FieldTypesTest
{
    using Xunit;
    using SmallSteps.Domain.FieldTypes;

    public class TextValueTest
    {
        [Fact]
        public void DefaultConstructor_ShouldInitializeWithoutValue()
        {
            TextValue text = new();

            Assert.False(text.HasValue);
            Assert.False(text.TryGetValue(out string value));
            Assert.Equal(string.Empty, value);
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("Some text")]
        [InlineData("Text with numbers 123")]
        [InlineData("Texte français avec accents éàç")]
        [InlineData("Line 1\nLine 2")]
        [InlineData("   ")]
        [InlineData("")]
        public void ParameterizedConstructor_ShouldInitializeWithValue(string expectedValue)
        {
            TextValue text = new(expectedValue);

            Assert.True(text.HasValue);
            Assert.True(text.TryGetValue(out string value));
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("Some text")]
        [InlineData("")]
        [InlineData("   ")]
        public void SetValue_ShouldUpdateValueAndMarkAsAssigned(string newValue)
        {
            TextValue text = new();

            string returnedValue = text.SetValue(newValue);

            Assert.Equal(newValue, returnedValue);
            Assert.True(text.HasValue);
            Assert.True(text.TryGetValue(out string value));
            Assert.Equal(newValue, value);
        }

        [Fact]
        public void SetValue_ShouldReplacePreviousValue()
        {
            TextValue text = new("Old text");

            text.SetValue("New text");

            Assert.True(text.TryGetValue(out string value));
            Assert.Equal("New text", value);
        }

        [Fact]
        public void EmptyString_ShouldBeConsideredAnAssignedValue()
        {
            TextValue text = new("");

            Assert.True(text.HasValue);
            Assert.True(text.TryGetValue(out string value));
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public void Whitespace_ShouldBePreserved()
        {
            TextValue text = new("   ");

            Assert.True(text.HasValue);
            Assert.True(text.TryGetValue(out string value));
            Assert.Equal("   ", value);
        }

        [Fact]
        public void Clear_ShouldRemoveAssignedValue()
        {
            TextValue text = new("Hello");

            text.Clear();

            Assert.False(text.HasValue);
            Assert.False(text.TryGetValue(out string value));
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public void Clear_WhenAlreadyEmpty_ShouldRemainEmpty()
        {
            TextValue text = new();

            text.Clear();

            Assert.False(text.HasValue);
        }

        [Fact]
        public void SetValue_Null_ShouldThrowArgumentNullException()
        {
            TextValue text = new();

            Assert.Throws<ArgumentNullException>(
                () => text.SetValue(null!)
            );

            Assert.False(text.HasValue);
        }

        [Fact]
        public void SetValue_Null_ShouldNotReplacePreviousValue()
        {
            TextValue text = new("Existing text");

            Assert.Throws<ArgumentNullException>(
                () => text.SetValue(null!)
            );

            Assert.True(text.HasValue);
            Assert.True(text.TryGetValue(out string value));
            Assert.Equal("Existing text", value);
        }

        [Fact]
        public void ParameterizedConstructor_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => { _ = new TextValue(null!); }
            );
        }
    }
}
