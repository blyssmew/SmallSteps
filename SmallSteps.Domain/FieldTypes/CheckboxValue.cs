// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.FieldTypes
{
    using SmallSteps.Domain.Abstractions;

    /// <summary>
    /// Represents a checkbox answer.
    /// Contient une réponse
    /// </summary>
    public class CheckboxValue : IFieldValue<bool>
    {
        private bool? _value;

        /// <summary>
        /// Initializes an unanswered checkbox.
        /// </summary>
        public CheckboxValue()
        {
            _value = null;
        }

        /// <summary>
        /// Initializes a checkbox with a specified value.
        /// </summary>
        /// <param name="value">The initial checkbox value.</param>
        public CheckboxValue(bool value)
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
        /// Gets the current checkbox value if available.
        /// </summary>
        public bool TryGetValue(out bool value)
        {
            value = _value.GetValueOrDefault();
            return _value.HasValue;
        }

        /// <summary>
        /// Assigns the checkbox value.
        /// </summary>
        public bool SetValue(bool value)
        {
            _value = value;
            return value;
        }

        /// <summary>
        /// Marks the checkbox as unanswered.
        /// </summary>
        public void Clear()
        {
            _value = null;
        }

        /// <summary>
        /// Inverts the checkbox state.
        /// An unanswered checkbox becomes checked.
        /// </summary>
        public bool Toggle()
        {
            return SetValue(!(_value ?? false));
        }

        /// <summary>
        /// Sets the checkbox to checked.
        /// </summary>
        public bool Check()
        {
            return SetValue(true);
        }

        /// <summary>
        /// Sets the checkbox to unchecked.
        /// </summary>
        public bool Uncheck()
        {
            return SetValue(false);
        }
    }
}
