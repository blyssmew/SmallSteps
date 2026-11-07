
// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.Responses
{
    using System;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.FieldTypes;
    using SmallSteps.Domain.Trackers;

    /// <summary>
    /// Represents a response provided for a field within an entry.
    /// </summary>
    public class Response
    {
        private readonly Guid _id;
        private readonly Guid _entryId;
        private readonly Guid _fieldDefinitionId;
        private readonly FieldType _type;
        private readonly object _value;

        /// <summary>
        /// Initializes a new response for a field within an entry.
        /// </summary>
        /// <param name="entry">The entry containing the response.</param>
        /// <param name="tracker">The tracker associated with the entry.</param>
        /// <param name="fieldDefinition">The field being answered.</param>
        public Response(Entry entry, Tracker tracker, FieldDefinition fieldDefinition)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(tracker);
            ArgumentNullException.ThrowIfNull(fieldDefinition);

            if (entry.TrackerId != tracker.Id)
            {
                throw new ArgumentException("The entry does not belong to the specified tracker.", nameof(tracker));
            }

            bool fieldBelongsToTracker = false;

            foreach (FieldDefinition field in tracker.Fields)
            {
                if (field.Id == fieldDefinition.Id)
                {
                    fieldBelongsToTracker = true;
                    break;
                }
            }

            if (!fieldBelongsToTracker)
            {
                throw new ArgumentException("The field does not belong to the specified tracker.", nameof(fieldDefinition));
            }

            _id = Guid.NewGuid();
            _entryId = entry.Id;
            _fieldDefinitionId = fieldDefinition.Id;
            _type = fieldDefinition.Type;

            _value = _type switch
            {
                FieldType.Checkbox => new CheckboxValue(),
                FieldType.Duration => new DurationValue(),
                FieldType.Text => new TextValue(),

                _ => throw new ArgumentOutOfRangeException(nameof(fieldDefinition), "The field type is not supported.")
            };
        }

        /// <summary>
        /// Gets the response's unique identifier.
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the identifier of the entry containing this response.
        /// </summary>
        public Guid EntryId
        {
            get
            {
                return _entryId;
            }
        }

        /// <summary>
        /// Gets the identifier of the field being answered.
        /// </summary>
        public Guid FieldDefinitionId
        {
            get
            {
                return _fieldDefinitionId;
            }
        }

        /// <summary>
        /// Gets the expected field type.
        /// </summary>
        public FieldType Type
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Gets whether a value has been assigned.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return _value switch
                {
                    CheckboxValue checkbox => checkbox.HasValue,
                    DurationValue duration => duration.HasValue,
                    TextValue text => text.HasValue,

                    _ => throw new InvalidOperationException("The response contains an unsupported value type.")
                };
            }
        }

        /// <summary>
        /// Gets the current value if available.
        /// </summary>
        /// <param name="value">The retrieved value.</param>
        /// <returns>True if a value exists; otherwise, false.</returns>
        public bool TryGetValue(out object? value)
        {
            switch (_value)
            {
                case CheckboxValue checkbox:
                    if (checkbox.TryGetValue(out bool checkboxValue))
                    {
                        value = checkboxValue;
                        return true;
                    }

                    break;

                case DurationValue duration:
                    if (duration.TryGetValue(out int durationValue))
                    {
                        value = durationValue;
                        return true;
                    }

                    break;

                case TextValue text:
                    if (text.TryGetValue(out string textValue))
                    {
                        value = textValue;
                        return true;
                    }

                    break;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Assigns a value to the response.
        /// </summary>
        /// <param name="value">The value to assign.</param>
        public void SetValue(object value)
        {
            ArgumentNullException.ThrowIfNull(value);

            switch (_value)
            {
                case CheckboxValue checkbox when value is bool checkboxValue:
                    checkbox.SetValue(checkboxValue);
                    break;

                case DurationValue duration when value is int durationValue:
                    duration.SetValue(durationValue);
                    break;

                case TextValue text when value is string textValue:
                    text.SetValue(textValue);
                    break;

                default:
                    throw new ArgumentException("The provided value does not match the field type.", nameof(value));
            }
        }

        /// <summary>
        /// Clears the current response value.
        /// </summary>
        public void Clear()
        {
            switch (_value)
            {
                case CheckboxValue checkbox:
                    checkbox.Clear();
                    break;

                case DurationValue duration:
                    duration.Clear();
                    break;

                case TextValue text:
                    text.Clear();
                    break;

                default:
                    throw new InvalidOperationException("The response contains an unsupported value type.");
            }
        }
    }
}
