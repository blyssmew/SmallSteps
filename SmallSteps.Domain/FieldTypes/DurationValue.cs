// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.FieldTypes
{
    using System;
    using SmallSteps.Domain.Abstractions;

    /// <summary>
    /// Represents a duration answer in minutes.
    /// Contient une réponse
    /// </summary>
    public class DurationValue : IFieldValue<int>
    {
        private int? _value;

        /// <summary>
        /// Initializes an unanswered duration.
        /// </summary>
        public DurationValue()
        {
            _value = null;
        }

        /// <summary>
        /// Initializes a duration with a specified value in minutes.
        /// </summary>
        /// <param name="value">The duration in minutes (must be >= 0).</param>
        public DurationValue(int value)
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
                return _value.HasValue;
            }
        }

        /// <summary>
        /// Gets the current duration if available.
        /// </summary>
        public bool TryGetValue(out int value)
        {
            value = _value.GetValueOrDefault();
            return _value.HasValue;
        }

        /// <summary>
        /// Assigns a duration in minutes.
        /// </summary>
        /// <param name="value">The duration in minutes (must be >= 0).</param>
        public int SetValue(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Duration cannot be negative."
                );
            }

            _value = value;
            return value;
        }

        /// <summary>
        /// Marks the duration as unanswered.
        /// </summary>
        public void Clear()
        {
            _value = null;
        }
    }
}
