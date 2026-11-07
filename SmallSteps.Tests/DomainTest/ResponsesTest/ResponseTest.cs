// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.DomainTest.ResponsesTest
{
    using System;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Responses;
    using SmallSteps.Domain.Trackers;
    using Xunit;

    public class ResponseTest
    {
        private static DateTimeOffset CreateDate()
        {
            return new DateTimeOffset(
                2026, 11, 6,
                13, 30, 0,
                TimeSpan.FromHours(-5));
        }

        private static (
            Tracker Tracker,
            Entry Entry,
            FieldDefinition Field)
            CreateSetup(FieldType type)
        {
            Tracker tracker = new Tracker("Test tracker");

            FieldDefinition field = new FieldDefinition(
                "Test field",
                type,
                0);

            tracker.AddField(field);

            Entry entry = new Entry(tracker.Id, CreateDate());

            return (tracker, entry, field);
        }

        private static Response CreateResponse(FieldType type)
        {
            var setup = CreateSetup(type);

            return new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);
        }

        // Constructor

        [Fact]
        public void Constructor_ShouldGenerateNonEmptyId()
        {
            Response response = CreateResponse(FieldType.Checkbox);

            Assert.NotEqual(Guid.Empty, response.Id);
        }

        [Fact]
        public void Constructor_ShouldGenerateUniqueIds()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Response first = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public void Constructor_ShouldPreserveEntryId()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Response response = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Assert.Equal(setup.Entry.Id, response.EntryId);
        }

        [Fact]
        public void Constructor_ShouldPreserveFieldDefinitionId()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Response response = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Assert.Equal(setup.Field.Id, response.FieldDefinitionId);
        }

        [Theory]
        [InlineData(FieldType.Checkbox)]
        [InlineData(FieldType.Duration)]
        [InlineData(FieldType.Text)]
        public void Constructor_ShouldPreserveFieldType(FieldType type)
        {
            Response response = CreateResponse(type);

            Assert.Equal(type, response.Type);
        }

        [Fact]
        public void Constructor_ShouldRejectNullEntry()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Assert.Throws<ArgumentNullException>(
                () => new Response(
                    null!,
                    setup.Tracker,
                    setup.Field));
        }

        [Fact]
        public void Constructor_ShouldRejectNullTracker()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Assert.Throws<ArgumentNullException>(
                () => new Response(
                    setup.Entry,
                    null!,
                    setup.Field));
        }

        [Fact]
        public void Constructor_ShouldRejectNullFieldDefinition()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Assert.Throws<ArgumentNullException>(
                () => new Response(
                    setup.Entry,
                    setup.Tracker,
                    null!));
        }

        [Fact]
        public void Constructor_ShouldRejectTrackerThatDoesNotOwnEntry()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Tracker anotherTracker = new Tracker("Another tracker");
            anotherTracker.AddField(setup.Field);

            Assert.Throws<ArgumentException>(
                () => new Response(
                    setup.Entry,
                    anotherTracker,
                    setup.Field));
        }

        [Fact]
        public void Constructor_ShouldRejectFieldThatDoesNotBelongToTracker()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            FieldDefinition anotherField = new FieldDefinition(
                "Another field",
                FieldType.Checkbox,
                1);

            Assert.Throws<ArgumentException>(
                () => new Response(
                    setup.Entry,
                    setup.Tracker,
                    anotherField));
        }

        // Initial value

        [Theory]
        [InlineData(FieldType.Checkbox)]
        [InlineData(FieldType.Duration)]
        [InlineData(FieldType.Text)]
        public void Constructor_ShouldInitializeWithoutValue(FieldType type)
        {
            Response response = CreateResponse(type);

            Assert.False(response.HasValue);
        }

        [Theory]
        [InlineData(FieldType.Checkbox)]
        [InlineData(FieldType.Duration)]
        [InlineData(FieldType.Text)]
        public void TryGetValue_ShouldReturnFalseWhenUnanswered(FieldType type)
        {
            Response response = CreateResponse(type);

            bool result = response.TryGetValue(out object? value);

            Assert.False(result);
            Assert.Null(value);
        }

        // SetValue

        [Fact]
        public void SetValue_ShouldAssignCheckboxValue()
        {
            Response response = CreateResponse(FieldType.Checkbox);

            response.SetValue(true);

            Assert.True(response.HasValue);
            Assert.True(response.TryGetValue(out object? value));
            Assert.IsType<bool>(value);
            Assert.Equal(true, value);
        }

        [Fact]
        public void SetValue_ShouldAllowFalseCheckboxValue()
        {
            Response response = CreateResponse(FieldType.Checkbox);

            response.SetValue(false);

            Assert.True(response.HasValue);
            Assert.True(response.TryGetValue(out object? value));
            Assert.Equal(false, value);
        }

        [Fact]
        public void SetValue_ShouldAssignDurationValue()
        {
            Response response = CreateResponse(FieldType.Duration);

            response.SetValue(480);

            Assert.True(response.HasValue);
            Assert.True(response.TryGetValue(out object? value));
            Assert.IsType<int>(value);
            Assert.Equal(480, value);
        }

        [Fact]
        public void SetValue_ShouldAllowZeroDuration()
        {
            Response response = CreateResponse(FieldType.Duration);

            response.SetValue(0);

            Assert.True(response.HasValue);
            Assert.True(response.TryGetValue(out object? value));
            Assert.Equal(0, value);
        }

        [Fact]
        public void SetValue_ShouldAssignTextValue()
        {
            Response response = CreateResponse(FieldType.Text);

            response.SetValue("A good day.");

            Assert.True(response.HasValue);
            Assert.True(response.TryGetValue(out object? value));
            Assert.IsType<string>(value);
            Assert.Equal("A good day.", value);
        }

        [Fact]
        public void SetValue_ShouldAllowEmptyText()
        {
            Response response = CreateResponse(FieldType.Text);

            response.SetValue(string.Empty);

            Assert.True(response.HasValue);
            Assert.True(response.TryGetValue(out object? value));
            Assert.Equal(string.Empty, value);
        }

        [Theory]
        [InlineData(FieldType.Checkbox, 123)]
        [InlineData(FieldType.Duration, "Not a duration")]
        [InlineData(FieldType.Text, true)]
        public void SetValue_ShouldRejectIncompatibleType(
            FieldType type,
            object value)
        {
            Response response = CreateResponse(type);

            Assert.Throws<ArgumentException>(
                () => response.SetValue(value));
        }

        [Fact]
        public void SetValue_ShouldRejectNull()
        {
            Response response = CreateResponse(FieldType.Text);

            Assert.Throws<ArgumentNullException>(
                () => response.SetValue(null!));
        }

        [Fact]
        public void SetValue_ShouldRejectNegativeDuration()
        {
            Response response = CreateResponse(FieldType.Duration);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => response.SetValue(-1));
        }

        [Fact]
        public void SetValue_ShouldReplaceExistingValue()
        {
            Response response = CreateResponse(FieldType.Text);

            response.SetValue("First answer");
            response.SetValue("Updated answer");

            Assert.True(response.TryGetValue(out object? value));
            Assert.Equal("Updated answer", value);
        }

        [Fact]
        public void SetValue_ShouldPreserveResponseIdentity()
        {
            Response response = CreateResponse(FieldType.Checkbox);
            Guid originalId = response.Id;

            response.SetValue(true);

            Assert.Equal(originalId, response.Id);
        }

        [Fact]
        public void SetValue_ShouldPreserveAssociations()
        {
            var setup = CreateSetup(FieldType.Checkbox);

            Response response = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            response.SetValue(true);

            Assert.Equal(setup.Entry.Id, response.EntryId);
            Assert.Equal(setup.Field.Id, response.FieldDefinitionId);
        }

        // Clear

        [Theory]
        [InlineData(FieldType.Checkbox, true)]
        [InlineData(FieldType.Duration, 120)]
        [InlineData(FieldType.Text, "An answer")]
        public void Clear_ShouldRemoveAssignedValue(
            FieldType type,
            object value)
        {
            Response response = CreateResponse(type);

            response.SetValue(value);
            response.Clear();

            Assert.False(response.HasValue);
            Assert.False(response.TryGetValue(out object? result));
            Assert.Null(result);
        }

        [Theory]
        [InlineData(FieldType.Checkbox)]
        [InlineData(FieldType.Duration)]
        [InlineData(FieldType.Text)]
        public void Clear_ShouldAllowUnansweredResponse(FieldType type)
        {
            Response response = CreateResponse(type);

            response.Clear();

            Assert.False(response.HasValue);
        }

        [Fact]
        public void Clear_ShouldPreserveResponseIdentity()
        {
            Response response = CreateResponse(FieldType.Text);
            Guid originalId = response.Id;

            response.SetValue("An answer");
            response.Clear();

            Assert.Equal(originalId, response.Id);
        }

        [Fact]
        public void Clear_ShouldAllowAssigningNewValue()
        {
            Response response = CreateResponse(FieldType.Text);

            response.SetValue("First answer");
            response.Clear();
            response.SetValue("Second answer");

            Assert.True(response.TryGetValue(out object? value));
            Assert.Equal("Second answer", value);
        }

        // Independence

        [Fact]
        public void Responses_ShouldHaveIndependentValues()
        {
            var setup = CreateSetup(FieldType.Text);

            Response first = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            first.SetValue("First answer");

            Assert.False(second.HasValue);
        }

        [Fact]
        public void Responses_ShouldHaveIndependentIdentities()
        {
            var setup = CreateSetup(FieldType.Text);

            Response first = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = new Response(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Assert.NotEqual(first.Id, second.Id);
        }
    }
}
