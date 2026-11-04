// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.Fields
{
    using System;

    /// <summary>
    /// Represents the definition and configuration of a field.
    /// Does not contain the answer provided by a user.
    /// </summary>
    public class FieldDefinition
    {
        private readonly Guid _id;
        private readonly FieldType _type;
        private string _name;
        private int _displayOrder;

        /// <summary>
        /// Initializes a new field definition.
        /// </summary>
        /// <param name="name">The field's display name.</param>
        /// <param name="type">The field's expected value type.</param>
        /// <param name="displayOrder">The field's display position.</param>
        public FieldDefinition(string name, FieldType type, int displayOrder)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Field name cannot be empty.", nameof(name));
            }

            if (!Enum.IsDefined(typeof(FieldType), type))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            ArgumentOutOfRangeException.ThrowIfNegative(displayOrder);

            _id = Guid.NewGuid();
            _name = name.Trim();
            _type = type;
            _displayOrder = displayOrder;
        }

        /// <summary>
        /// Gets the field's unique identifier.
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the field's display name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the field's expected value type.
        /// </summary>
        public FieldType Type
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Gets the field's display position.
        /// </summary>
        public int DisplayOrder
        {
            get
            {
                return _displayOrder;
            }
        }

        /// <summary>
        /// Changes the field's display name.
        /// </summary>
        /// <param name="name">The new field name.</param>
        public void Rename(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Field name cannot be empty.", nameof(name));
            }

            _name = name.Trim();
        }

        /// <summary>
        /// Changes the field's display position.
        /// </summary>
        /// <param name="displayOrder">The new display position.</param>
        public void ChangeDisplayOrder(int displayOrder)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(displayOrder);

            _displayOrder = displayOrder;
        }
    }
}
