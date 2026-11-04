// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.FieldTypes
{
    using System;
    using SmallSteps.Domain.Abstractions;

    /// <summary>
    /// Represents a text answer.
    /// </summary>
    public class TextValue : IFieldValue<string>
    {
        private string? _value;

        /// <summary>
        /// Initializes an unanswered text value.
        /// </summary>
        public TextValue()
        {
            _value = null;
        }

        /// <summary>
        /// Initializes a text value with specified content.
        /// </summary>
        /// <param name="value">The initial text.</param>
        public TextValue(string value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Gets whether a value has been assigned.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return _value is not null;
            }
        }

        /// <summary>
        /// Gets the current text if available.
        /// </summary>
        public bool TryGetValue(out string value)
        {
            value = _value ?? string.Empty;
            return _value is not null;
        }

        /// <summary>
        /// Assigns a text value.
        /// </summary>
        /// <param name="value">The text to assign.</param>
        public string SetValue(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            _value = value;
            return value;
        }

        /// <summary>
        /// Marks the text as unanswered.
        /// </summary>
        public void Clear()
        {
            _value = null;
        }
    }
}
