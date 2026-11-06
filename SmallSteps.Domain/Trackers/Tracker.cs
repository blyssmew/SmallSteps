// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.Trackers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SmallSteps.Domain.Fields;

    /// <summary>
    /// Represents a tracker and its field configuration.
    /// </summary>
    public class Tracker
    {
        private readonly Guid _id;
        private readonly List<FieldDefinition> _fields;
        private readonly ReadOnlyCollection<FieldDefinition> _readOnlyFields;

        private string _name;
        private bool _isArchived;

        /// <summary>
        /// Initializes a new tracker.
        /// </summary>
        /// <param name="name">The tracker's display name.</param>
        public Tracker(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tracker name cannot be empty.", nameof(name));
            }

            _id = Guid.NewGuid();
            _name = name.Trim();
            _isArchived = false;

            _fields = [];
            _readOnlyFields = _fields.AsReadOnly();
        }

        /// <summary>
        /// Gets the tracker's unique identifier.
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the tracker's display name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets whether the tracker is archived.
        /// </summary>
        public bool IsArchived
        {
            get
            {
                return _isArchived;
            }
        }

        /// <summary>
        /// Gets the tracker's configured fields.
        /// </summary>
        public IReadOnlyList<FieldDefinition> Fields
        {
            get
            {
                return _readOnlyFields;
            }
        }

        /// <summary>
        /// Changes the tracker's display name.
        /// </summary>
        /// <param name="name">The new tracker name.</param>
        public void Rename(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tracker name cannot be empty.", nameof(name));
            }

            _name = name.Trim();
        }

        /// <summary>
        /// Archives the tracker.
        /// </summary>
        public void Archive()
        {
            _isArchived = true;
        }

        /// <summary>
        /// Restores an archived tracker.
        /// </summary>
        public void Restore()
        {
            _isArchived = false;
        }

        /// <summary>
        /// Adds a field to the tracker.
        /// </summary>
        /// <param name="field">The field to add.</param>
        public void AddField(FieldDefinition field)
        {
            ArgumentNullException.ThrowIfNull(field);

            if (_fields.Exists(existing => existing.Id == field.Id))
            {
                throw new InvalidOperationException("A field with the same ID already exists in this tracker.");
            }

            _fields.Add(field);
        }

        /// <summary>
        /// Removes a field from the tracker.
        /// </summary>
        /// <param name="fieldId">The ID of the field to remove.</param>
        /// <returns>True if the field was removed; otherwise, false.</returns>
        public bool RemoveField(Guid fieldId)
        {
            int index = _fields.FindIndex(field => field.Id == fieldId);

            if (index == -1)
            {
                return false;
            }

            _fields.RemoveAt(index);
            return true;
        }
    }
}
