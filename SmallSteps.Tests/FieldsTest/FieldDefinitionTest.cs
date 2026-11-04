// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.FieldsTest
{
    using Xunit;
    using SmallSteps.Domain.Fields;

    public class FieldDefinitionTest
    {
        // --------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            Assert.NotEqual(Guid.Empty, field.Id);
            Assert.Equal("Exercise", field.Name);
            Assert.Equal(FieldType.Checkbox, field.Type);
            Assert.Equal(0, field.DisplayOrder);
        }

        [Fact]
        public void Constructor_ShouldTrimName()
        {
            FieldDefinition field = new("  Exercise  ", FieldType.Checkbox, 0);

            Assert.Equal("Exercise", field.Name);
        }

        [Fact]
        public void Constructor_ShouldGenerateUniqueIds()
        {
            FieldDefinition first = new("Exercise", FieldType.Checkbox, 0);
            FieldDefinition second = new("Duration", FieldType.Duration, 1);

            Assert.NotEqual(first.Id, second.Id);
        }

        [Theory]
        [InlineData(FieldType.Checkbox)]
        [InlineData(FieldType.Duration)]
        [InlineData(FieldType.Text)]
        public void Constructor_ShouldAcceptSupportedTypes(FieldType type)
        {
            FieldDefinition field = new("Test", type, 0);

            Assert.Equal(type, field.Type);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void Constructor_ShouldAcceptValidDisplayOrders(int displayOrder)
        {
            FieldDefinition field = new("Test", FieldType.Text, displayOrder);

            Assert.Equal(displayOrder, field.DisplayOrder);
        }

        // --------------------------------------------------
        // CONSTRUCTOR VALIDATION - NAME
        // --------------------------------------------------

        [Fact]
        public void Constructor_ShouldThrowWhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new FieldDefinition(null!, FieldType.Text, 0));
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
                () => new FieldDefinition(name, FieldType.Text, 0));
        }

        // --------------------------------------------------
        // CONSTRUCTOR VALIDATION - TYPE
        // --------------------------------------------------

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        [InlineData(999)]
        public void Constructor_ShouldThrowWhenTypeIsInvalid(int type)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new FieldDefinition("Test", (FieldType)type, 0));
        }

        // --------------------------------------------------
        // CONSTRUCTOR VALIDATION - DISPLAY ORDER
        // --------------------------------------------------

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(int.MinValue)]
        public void Constructor_ShouldThrowWhenDisplayOrderIsNegative(int displayOrder)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new FieldDefinition("Test", FieldType.Text, displayOrder));
        }

        // --------------------------------------------------
        // RENAME
        // --------------------------------------------------

        [Fact]
        public void Rename_ShouldUpdateName()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            field.Rename("Workout");

            Assert.Equal("Workout", field.Name);
        }

        [Fact]
        public void Rename_ShouldTrimName()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            field.Rename("  Workout  ");

            Assert.Equal("Workout", field.Name);
        }

        [Fact]
        public void Rename_ShouldPreserveId()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);
            Guid originalId = field.Id;

            field.Rename("Workout");

            Assert.Equal(originalId, field.Id);
        }

        [Fact]
        public void Rename_ShouldThrowWhenNameIsNull()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            Assert.Throws<ArgumentNullException>(() => field.Rename(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Rename_ShouldThrowWhenNameIsEmptyOrWhitespace(string name)
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            Assert.Throws<ArgumentException>(() => field.Rename(name));

            // The original name should remain unchanged.
            Assert.Equal("Exercise", field.Name);
        }

        // --------------------------------------------------
        // CHANGE DISPLAY ORDER
        // --------------------------------------------------

        [Fact]
        public void ChangeDisplayOrder_ShouldUpdateDisplayOrder()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            field.ChangeDisplayOrder(5);

            Assert.Equal(5, field.DisplayOrder);
        }

        [Fact]
        public void ChangeDisplayOrder_ShouldAcceptZero()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 5);

            field.ChangeDisplayOrder(0);

            Assert.Equal(0, field.DisplayOrder);
        }

        [Fact]
        public void ChangeDisplayOrder_ShouldPreserveId()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);
            Guid originalId = field.Id;

            field.ChangeDisplayOrder(5);

            Assert.Equal(originalId, field.Id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(int.MinValue)]
        public void ChangeDisplayOrder_ShouldThrowWhenNegative(int displayOrder)
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 3);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => field.ChangeDisplayOrder(displayOrder));

            // The original order should remain unchanged.
            Assert.Equal(3, field.DisplayOrder);
        }

        // --------------------------------------------------
        // FIELD INDEPENDENCE
        // --------------------------------------------------

        [Fact]
        public void ModifyingField_ShouldNotModifyAnotherField()
        {
            FieldDefinition first = new("Exercise", FieldType.Checkbox, 0);
            FieldDefinition second = new("Duration", FieldType.Duration, 1);

            first.Rename("Workout");
            first.ChangeDisplayOrder(5);

            Assert.Equal("Duration", second.Name);
            Assert.Equal(1, second.DisplayOrder);
            Assert.Equal(FieldType.Duration, second.Type);
        }

        [Fact]
        public void ModifyingField_ShouldNotChangeItsType()
        {
            FieldDefinition field = new("Exercise", FieldType.Checkbox, 0);

            field.Rename("Workout");
            field.ChangeDisplayOrder(5);

            Assert.Equal(FieldType.Checkbox, field.Type);
        }
    }
}
