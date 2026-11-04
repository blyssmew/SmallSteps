// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.FieldTypesTest
{
    using Xunit;
    using SmallSteps.Domain.FieldTypes;

    public class DurationValueTest
    {
        [Fact]
        public void DefaultConstructor_ShouldInitializeWithoutValue()
        {
            DurationValue duration = new();

            Assert.False(duration.HasValue);
            Assert.False(duration.TryGetValue(out int value));
            Assert.Equal(0, value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(1440)]
        [InlineData(int.MaxValue)]
        public void ParameterizedConstructor_ShouldInitializeWithValue(int expectedValue)
        {
            DurationValue duration = new(expectedValue);

            Assert.True(duration.HasValue);
            Assert.True(duration.TryGetValue(out int value));
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(1440)]
        [InlineData(int.MaxValue)]
        public void SetValue_ShouldUpdateValueAndMarkAsAssigned(int newValue)
        {
            DurationValue duration = new();

            int returnedValue = duration.SetValue(newValue);

            Assert.Equal(newValue, returnedValue);
            Assert.True(duration.HasValue);
            Assert.True(duration.TryGetValue(out int value));
            Assert.Equal(newValue, value);
        }

        [Fact]
        public void SetValue_ShouldReplacePreviousValue()
        {
            DurationValue duration = new(30);

            duration.SetValue(60);

            Assert.True(duration.TryGetValue(out int value));
            Assert.Equal(60, value);
        }

        [Fact]
        public void Zero_ShouldBeConsideredAnAssignedValue()
        {
            DurationValue duration = new(0);

            Assert.True(duration.HasValue);
            Assert.True(duration.TryGetValue(out int value));
            Assert.Equal(0, value);
        }

        [Fact]
        public void Clear_ShouldRemoveAssignedValue()
        {
            DurationValue duration = new(60);

            duration.Clear();

            Assert.False(duration.HasValue);
            Assert.False(duration.TryGetValue(out int value));
            Assert.Equal(0, value);
        }

        [Fact]
        public void Clear_WhenAlreadyEmpty_ShouldRemainEmpty()
        {
            DurationValue duration = new();

            duration.Clear();

            Assert.False(duration.HasValue);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-30)]
        [InlineData(-60)]
        [InlineData(int.MinValue)]
        public void ParameterizedConstructor_NegativeValue_ShouldThrowException(int invalidValue)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { _ = new DurationValue(invalidValue); }
            );
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-30)]
        [InlineData(-60)]
        [InlineData(int.MinValue)]
        public void SetValue_NegativeValue_ShouldThrowException(int invalidValue)
        {
            DurationValue duration = new();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => duration.SetValue(invalidValue)
            );

            Assert.False(duration.HasValue);
        }

        [Fact]
        public void SetValue_NegativeValue_ShouldNotReplacePreviousValue()
        {
            DurationValue duration = new(60);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => duration.SetValue(-10)
            );

            Assert.True(duration.HasValue);
            Assert.True(duration.TryGetValue(out int value));
            Assert.Equal(60, value);
        }

        [Fact]
        public void SetValue_NegativeValueAfterClear_ShouldRemainEmpty()
        {
            DurationValue duration = new(60);
            duration.Clear();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => duration.SetValue(-10)
            );

            Assert.False(duration.HasValue);
        }
    }
}
