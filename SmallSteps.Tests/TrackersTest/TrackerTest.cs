
// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.TrackersTest
{
    using Xunit;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Trackers;

    public class TrackerTest
    {
        // --------------------------------------------------
        // HELPERS
        // --------------------------------------------------

        /// <summary>
        /// Creates a field definition for testing.
        /// </summary>
        private static FieldDefinition CreateField(
            string name = "Test Field",
            FieldType type = FieldType.Text,
            int displayOrder = 0)
        {
            return new FieldDefinition(name, type, displayOrder);
        }

        // --------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            Tracker tracker = new("Physical Activity");

            Assert.NotEqual(Guid.Empty, tracker.Id);
            Assert.Equal("Physical Activity", tracker.Name);
            Assert.False(tracker.IsArchived);
            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void Constructor_ShouldGenerateUniqueIds()
        {
            Tracker first = new("Physical Activity");
            Tracker second = new("Sleep");

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public void Constructor_ShouldTrimName()
        {
            Tracker tracker = new("  Physical Activity  ");

            Assert.Equal("Physical Activity", tracker.Name);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithoutFields()
        {
            Tracker tracker = new("Physical Activity");

            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void Constructor_ShouldInitializeAsNotArchived()
        {
            Tracker tracker = new("Physical Activity");

            Assert.False(tracker.IsArchived);
        }

        [Fact]
        public void Constructor_ShouldThrowWhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Tracker(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Constructor_ShouldThrowWhenNameIsEmptyOrWhitespace(string name)
        {
            Assert.Throws<ArgumentException>(
                () => new Tracker(name));
        }

        // --------------------------------------------------
        // RENAME
        // --------------------------------------------------

        [Fact]
        public void Rename_ShouldUpdateName()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Rename("Daily Exercise");

            Assert.Equal("Daily Exercise", tracker.Name);
        }

        [Fact]
        public void Rename_ShouldTrimName()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Rename("  Daily Exercise  ");

            Assert.Equal("Daily Exercise", tracker.Name);
        }

        [Fact]
        public void Rename_ShouldPreserveId()
        {
            Tracker tracker = new("Physical Activity");
            Guid originalId = tracker.Id;

            tracker.Rename("Daily Exercise");

            Assert.Equal(originalId, tracker.Id);
        }

        [Fact]
        public void Rename_ShouldThrowWhenNameIsNull()
        {
            Tracker tracker = new("Physical Activity");

            Assert.Throws<ArgumentNullException>(
                () => tracker.Rename(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Rename_ShouldThrowWhenNameIsEmptyOrWhitespace(string name)
        {
            Tracker tracker = new("Physical Activity");

            Assert.Throws<ArgumentException>(
                () => tracker.Rename(name));

            Assert.Equal("Physical Activity", tracker.Name);
        }

        [Fact]
        public void Rename_ShouldPreserveArchiveState()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();
            tracker.Rename("Daily Exercise");

            Assert.True(tracker.IsArchived);
            Assert.Equal("Daily Exercise", tracker.Name);
        }

        [Fact]
        public void Rename_ShouldPreserveFields()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);
            tracker.Rename("Daily Exercise");

            Assert.Single(tracker.Fields);
            Assert.Same(field, tracker.Fields[0]);
        }

        [Fact]
        public void Rename_ShouldNotAffectAnotherTracker()
        {
            Tracker first = new("Physical Activity");
            Tracker second = new("Sleep");

            first.Rename("Daily Exercise");

            Assert.Equal("Daily Exercise", first.Name);
            Assert.Equal("Sleep", second.Name);
        }

        // --------------------------------------------------
        // ARCHIVE
        // --------------------------------------------------

        [Fact]
        public void Archive_ShouldSetIsArchivedToTrue()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();

            Assert.True(tracker.IsArchived);
        }

        [Fact]
        public void Archive_ShouldRemainArchivedWhenCalledMultipleTimes()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();
            tracker.Archive();
            tracker.Archive();

            Assert.True(tracker.IsArchived);
        }

        [Fact]
        public void Archive_ShouldPreserveId()
        {
            Tracker tracker = new("Physical Activity");
            Guid originalId = tracker.Id;

            tracker.Archive();

            Assert.Equal(originalId, tracker.Id);
        }

        [Fact]
        public void Archive_ShouldPreserveName()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();

            Assert.Equal("Physical Activity", tracker.Name);
        }

        [Fact]
        public void Archive_ShouldPreserveFields()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);
            tracker.Archive();

            Assert.Single(tracker.Fields);
            Assert.Same(field, tracker.Fields[0]);
        }

        // --------------------------------------------------
        // RESTORE
        // --------------------------------------------------

        [Fact]
        public void Restore_ShouldSetIsArchivedToFalse()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();
            tracker.Restore();

            Assert.False(tracker.IsArchived);
        }

        [Fact]
        public void Restore_ShouldRemainNotArchivedWhenCalledMultipleTimes()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();
            tracker.Restore();
            tracker.Restore();
            tracker.Restore();

            Assert.False(tracker.IsArchived);
        }

        [Fact]
        public void Restore_ShouldRemainNotArchivedWhenNotPreviouslyArchived()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Restore();

            Assert.False(tracker.IsArchived);
        }

        [Fact]
        public void Restore_ShouldPreserveId()
        {
            Tracker tracker = new("Physical Activity");
            Guid originalId = tracker.Id;

            tracker.Archive();
            tracker.Restore();

            Assert.Equal(originalId, tracker.Id);
        }

        [Fact]
        public void Restore_ShouldPreserveFields()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);
            tracker.Archive();
            tracker.Restore();

            Assert.Single(tracker.Fields);
            Assert.Same(field, tracker.Fields[0]);
        }

        // --------------------------------------------------
        // FIELDS - INITIALIZATION
        // --------------------------------------------------

        [Fact]
        public void Fields_ShouldInitiallyBeEmpty()
        {
            Tracker tracker = new("Physical Activity");

            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void Fields_ShouldReturnSameCollectionInstance()
        {
            Tracker tracker = new("Physical Activity");

            IReadOnlyList<FieldDefinition> first = tracker.Fields;
            IReadOnlyList<FieldDefinition> second = tracker.Fields;

            Assert.Same(first, second);
        }

        [Fact]
        public void Fields_ShouldProvideLiveReadOnlyView()
        {
            Tracker tracker = new("Physical Activity");
            IReadOnlyList<FieldDefinition> fields = tracker.Fields;

            FieldDefinition field = CreateField();

            tracker.AddField(field);

            Assert.Single(fields);
            Assert.Same(field, fields[0]);
        }

        [Fact]
        public void Fields_ShouldBeReadOnly()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition first = CreateField("First");
            FieldDefinition second = CreateField("Second");

            tracker.AddField(first);

            IList<FieldDefinition> fields =
                Assert.IsAssignableFrom<IList<FieldDefinition>>(tracker.Fields);

            Assert.True(fields.IsReadOnly);

            Assert.Throws<NotSupportedException>(
                () => fields.Add(second));

            Assert.Single(tracker.Fields);
            Assert.Same(first, tracker.Fields[0]);
        }

        [Fact]
        public void Fields_ShouldNotAllowReplacingExistingField()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition first = CreateField("First");
            FieldDefinition second = CreateField("Second");

            tracker.AddField(first);

            IList<FieldDefinition> fields =
                Assert.IsAssignableFrom<IList<FieldDefinition>>(tracker.Fields);

            Assert.Throws<NotSupportedException>(
                () => fields[0] = second);

            Assert.Same(first, tracker.Fields[0]);
        }

        [Fact]
        public void Fields_ShouldNotAllowClearingCollectionExternally()
        {
            Tracker tracker = new("Physical Activity");

            tracker.AddField(CreateField());

            IList<FieldDefinition> fields =
                Assert.IsAssignableFrom<IList<FieldDefinition>>(tracker.Fields);

            Assert.Throws<NotSupportedException>(() => fields.Clear());

            Assert.Single(tracker.Fields);
        }

        [Fact]
        public void Fields_ShouldNotAllowRemovingFieldsExternally()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);

            IList<FieldDefinition> fields =
                Assert.IsAssignableFrom<IList<FieldDefinition>>(tracker.Fields);

            Assert.Throws<NotSupportedException>(
                () => fields.Remove(field));

            Assert.Single(tracker.Fields);
        }

        // --------------------------------------------------
        // ADD FIELD
        // --------------------------------------------------

        [Fact]
        public void AddField_ShouldAddField()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);

            Assert.Single(tracker.Fields);
            Assert.Same(field, tracker.Fields[0]);
        }

        [Fact]
        public void AddField_ShouldAddMultipleFields()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition first = CreateField("Exercise", FieldType.Checkbox, 0);
            FieldDefinition second = CreateField("Duration", FieldType.Duration, 1);
            FieldDefinition third = CreateField("Notes", FieldType.Text, 2);

            tracker.AddField(first);
            tracker.AddField(second);
            tracker.AddField(third);

            Assert.Equal(3, tracker.Fields.Count);

            Assert.Same(first, tracker.Fields[0]);
            Assert.Same(second, tracker.Fields[1]);
            Assert.Same(third, tracker.Fields[2]);
        }

        [Fact]
        public void AddField_ShouldPreserveInsertionOrder()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition first = CreateField("First", displayOrder: 0);
            FieldDefinition second = CreateField("Second", displayOrder: 1);
            FieldDefinition third = CreateField("Third", displayOrder: 2);

            tracker.AddField(first);
            tracker.AddField(second);
            tracker.AddField(third);

            Assert.Equal(first, tracker.Fields[0]);
            Assert.Equal(second, tracker.Fields[1]);
            Assert.Equal(third, tracker.Fields[2]);
        }

        [Fact]
        public void AddField_ShouldThrowWhenFieldIsNull()
        {
            Tracker tracker = new("Physical Activity");

            Assert.Throws<ArgumentNullException>(
                () => tracker.AddField(null!));

            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void AddField_ShouldThrowWhenFieldIdAlreadyExists()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);

            Assert.Throws<InvalidOperationException>(
                () => tracker.AddField(field));

            Assert.Single(tracker.Fields);
        }

        [Fact]
        public void AddField_ShouldNotDuplicateFieldWhenIdAlreadyExists()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);

            Assert.Throws<InvalidOperationException>(
                () => tracker.AddField(field));

            // Assert.Equal(1, tracker.Fields.Count);
            Assert.Single(tracker.Fields);
            Assert.Same(field, tracker.Fields[0]);
        }

        [Fact]
        public void AddField_ShouldNotModifyTrackerIdentity()
        {
            Tracker tracker = new("Physical Activity");
            Guid originalId = tracker.Id;

            tracker.AddField(CreateField());

            Assert.Equal(originalId, tracker.Id);
        }

        [Fact]
        public void AddField_ShouldNotModifyTrackerName()
        {
            Tracker tracker = new("Physical Activity");

            tracker.AddField(CreateField());

            Assert.Equal("Physical Activity", tracker.Name);
        }

        [Fact]
        public void AddField_ShouldNotChangeArchiveState()
        {
            Tracker tracker = new("Physical Activity");

            tracker.Archive();
            tracker.AddField(CreateField());

            Assert.True(tracker.IsArchived);
        }

        [Fact]
        public void AddField_ShouldNotAffectAnotherTracker()
        {
            Tracker first = new("Physical Activity");
            Tracker second = new("Sleep");

            first.AddField(CreateField());

            Assert.Single(first.Fields);
            Assert.Empty(second.Fields);
        }

        // --------------------------------------------------
        // REMOVE FIELD
        // --------------------------------------------------

        [Fact]
        public void RemoveField_ShouldRemoveExistingField()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);

            bool result = tracker.RemoveField(field.Id);

            Assert.True(result);
            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void RemoveField_ShouldRemoveOnlySpecifiedField()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition first = CreateField("First");
            FieldDefinition second = CreateField("Second");
            FieldDefinition third = CreateField("Third");

            tracker.AddField(first);
            tracker.AddField(second);
            tracker.AddField(third);

            tracker.RemoveField(second.Id);

            Assert.Equal(2, tracker.Fields.Count);
            Assert.Same(first, tracker.Fields[0]);
            Assert.Same(third, tracker.Fields[1]);
        }

        [Fact]
        public void RemoveField_ShouldReturnFalseWhenFieldDoesNotExist()
        {
            Tracker tracker = new("Physical Activity");

            bool result = tracker.RemoveField(Guid.NewGuid());

            Assert.False(result);
            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void RemoveField_ShouldReturnFalseWhenIdIsEmpty()
        {
            Tracker tracker = new("Physical Activity");

            bool result = tracker.RemoveField(Guid.Empty);

            Assert.False(result);
            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void RemoveField_ShouldReturnFalseWhenCalledAgainOnRemovedField()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField();

            tracker.AddField(field);

            bool firstResult = tracker.RemoveField(field.Id);
            bool secondResult = tracker.RemoveField(field.Id);

            Assert.True(firstResult);
            Assert.False(secondResult);
            Assert.Empty(tracker.Fields);
        }

        [Fact]
        public void RemoveField_ShouldPreserveRemainingFieldReferences()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition first = CreateField("First");
            FieldDefinition second = CreateField("Second");
            FieldDefinition third = CreateField("Third");

            tracker.AddField(first);
            tracker.AddField(second);
            tracker.AddField(third);

            tracker.RemoveField(second.Id);

            Assert.Same(first, tracker.Fields[0]);
            Assert.Same(third, tracker.Fields[1]);
        }

        [Fact]
        public void RemoveField_ShouldPreserveFieldProperties()
        {
            Tracker tracker = new("Physical Activity");
            FieldDefinition field = CreateField("Exercise", FieldType.Checkbox, 3);

            tracker.AddField(field);
            tracker.RemoveField(field.Id);

            Assert.Equal("Exercise", field.Name);
            Assert.Equal(FieldType.Checkbox, field.Type);
            Assert.Equal(3, field.DisplayOrder);
        }

        [Fact]
        public void RemoveField_ShouldPreserveTrackerIdentity()
        {
            Tracker tracker = new("Physical Activity");
            Guid originalId = tracker.Id;

            FieldDefinition field = CreateField();

            tracker.AddField(field);
            tracker.RemoveField(field.Id);

            Assert.Equal(originalId, tracker.Id);
        }

        [Fact]
        public void RemoveField_ShouldNotAffectAnotherTracker()
        {
            Tracker first = new("Physical Activity");
            Tracker second = new("Sleep");

            FieldDefinition firstField = CreateField("First");
            FieldDefinition secondField = CreateField("Second");

            first.AddField(firstField);
            second.AddField(secondField);

            first.RemoveField(firstField.Id);

            Assert.Empty(first.Fields);
            Assert.Single(second.Fields);
            Assert.Same(secondField, second.Fields[0]);
        }

        // --------------------------------------------------
        // TRACKER INDEPENDENCE
        // --------------------------------------------------

        [Fact]
        public void ModifyingTracker_ShouldNotModifyAnotherTracker()
        {
            Tracker first = new("Physical Activity");
            Tracker second = new("Sleep");

            FieldDefinition firstField = CreateField("Exercise");
            FieldDefinition secondField = CreateField("Hours");

            first.AddField(firstField);
            second.AddField(secondField);

            first.Rename("Daily Exercise");
            first.Archive();
            first.RemoveField(firstField.Id);

            Assert.Equal("Sleep", second.Name);
            Assert.False(second.IsArchived);
            Assert.Single(second.Fields);
            Assert.Same(secondField, second.Fields[0]);
        }

        [Fact]
        public void Tracker_ShouldMaintainIndependentFieldCollections()
        {
            Tracker first = new("Physical Activity");
            Tracker second = new("Sleep");

            Assert.NotSame(first.Fields, second.Fields);

            first.AddField(CreateField());

            Assert.Single(first.Fields);
            Assert.Empty(second.Fields);
        }

        [Fact]
        public void TrackerOperations_ShouldNotChangeFieldDefinitions()
        {
            Tracker tracker = new("Physical Activity");

            FieldDefinition field = CreateField(
                "Exercise",
                FieldType.Checkbox,
                2);

            Guid originalId = field.Id;

            tracker.AddField(field);
            tracker.Archive();
            tracker.Rename("Daily Exercise");
            tracker.Restore();
            tracker.RemoveField(field.Id);

            Assert.Equal(originalId, field.Id);
            Assert.Equal("Exercise", field.Name);
            Assert.Equal(FieldType.Checkbox, field.Type);
            Assert.Equal(2, field.DisplayOrder);
        }
    }
}
