// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.DomainTest.FieldTypesTest
{
    using Xunit;
    using SmallSteps.Domain.FieldTypes;

    public class CheckboxValueTest
    {
        [Fact]
        public void DefaultConstructor_ShouldInitializeWithoutValue()
        {
            CheckboxValue checkbox = new();

            Assert.False(checkbox.HasValue);
            Assert.False(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ParameterizedConstructor_ShouldInitializeWithValue(bool expectedValue)
        {
            CheckboxValue checkbox = new(expectedValue);

            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetValue_ShouldUpdateValueAndMarkAsAssigned(bool newValue)
        {
            CheckboxValue checkbox = new();

            bool returnedValue = checkbox.SetValue(newValue);

            Assert.Equal(newValue, returnedValue);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.Equal(newValue, value);
        }

        [Fact]
        public void SetValue_ShouldReplacePreviousValue()
        {
            CheckboxValue checkbox = new(true);

            checkbox.SetValue(false);

            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void Clear_ShouldRemoveAssignedValue()
        {
            CheckboxValue checkbox = new(true);

            checkbox.Clear();

            Assert.False(checkbox.HasValue);
            Assert.False(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void Clear_WhenAlreadyEmpty_ShouldRemainEmpty()
        {
            CheckboxValue checkbox = new();

            checkbox.Clear();

            Assert.False(checkbox.HasValue);
            Assert.False(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void ToggleFromTrue_ShouldSetFalse()
        {
            CheckboxValue checkbox = new(true);

            bool result = checkbox.Toggle();

            Assert.False(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void ToggleFromFalse_ShouldSetTrue()
        {
            CheckboxValue checkbox = new(false);

            bool result = checkbox.Toggle();

            Assert.True(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.True(value);
        }

        [Fact]
        public void ToggleWithoutValue_ShouldSetTrue()
        {
            CheckboxValue checkbox = new();

            bool result = checkbox.Toggle();

            Assert.True(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.True(value);
        }

        [Fact]
        public void ToggleMultipleTimes_ShouldAlternateValue()
        {
            CheckboxValue checkbox = new();

            Assert.True(checkbox.Toggle());
            Assert.False(checkbox.Toggle());
            Assert.True(checkbox.Toggle());
            Assert.False(checkbox.Toggle());

            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void Check_ShouldSetTrue()
        {
            CheckboxValue checkbox = new();

            bool result = checkbox.Check();

            Assert.True(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.True(value);
        }

        [Fact]
        public void Check_WhenAlreadyTrue_ShouldRemainTrue()
        {
            CheckboxValue checkbox = new(true);

            bool result = checkbox.Check();

            Assert.True(result);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.True(value);
        }

        [Fact]
        public void Check_WhenFalse_ShouldSetTrue()
        {
            CheckboxValue checkbox = new(false);

            bool result = checkbox.Check();

            Assert.True(result);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.True(value);
        }

        [Fact]
        public void Uncheck_ShouldSetFalse()
        {
            CheckboxValue checkbox = new(true);

            bool result = checkbox.Uncheck();

            Assert.False(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void Uncheck_WhenAlreadyFalse_ShouldRemainFalse()
        {
            CheckboxValue checkbox = new(false);

            bool result = checkbox.Uncheck();

            Assert.False(result);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void Uncheck_WhenWithoutValue_ShouldSetFalse()
        {
            CheckboxValue checkbox = new();

            bool result = checkbox.Uncheck();

            Assert.False(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.False(value);
        }

        [Fact]
        public void ClearThenToggle_ShouldSetTrue()
        {
            CheckboxValue checkbox = new(false);

            checkbox.Clear();
            bool result = checkbox.Toggle();

            Assert.True(result);
            Assert.True(checkbox.HasValue);
            Assert.True(checkbox.TryGetValue(out bool value));
            Assert.True(value);
        }
    }
}
