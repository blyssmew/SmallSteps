// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.Abstractions
{
    /// <summary>
    /// Defines a typed answer value for a field.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    public interface IFieldValue<T>
    {
        /// <summary>
        /// Gets whether a value has been assigned.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Gets the current value, if available.
        /// </summary>
        bool TryGetValue(out T value);

        /// <summary>
        /// Assigns a value.
        /// </summary>
        T SetValue(T value);

        /// <summary>
        /// Clears the current value.
        /// </summary>
        void Clear();
    }
}
