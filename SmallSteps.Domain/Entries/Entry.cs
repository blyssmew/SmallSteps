// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.Entries
{
    using System;
    using System.Collections.ObjectModel;
    using SmallSteps.Domain.Responses;

    /// <summary>
    /// Represents a dated session or observation belonging to a tracker.
    /// </summary>
    public class Entry
    {
        private readonly Guid _id;
        private readonly Guid _trackerId;

        private readonly List<Response> _responses;
        private readonly ReadOnlyCollection<Response> _readOnlyResponses;

        private DateTimeOffset _date;

        /// <summary>
        /// Initializes a new entry.
        /// </summary>
        /// <param name="trackerId">The identifier of the owning tracker.</param>
        /// <param name="date">The date of the entry.</param>
        public Entry(Guid trackerId, DateTimeOffset date)
        {
            if (trackerId == Guid.Empty)
            {
                throw new ArgumentException(
                    "An entry must belong to a tracker.",
                    nameof(trackerId));
            }

            _id = Guid.NewGuid();
            _trackerId = trackerId;
            _date = date.ToUniversalTime();

            _responses = [];
            _readOnlyResponses = _responses.AsReadOnly();
        }

        /// <summary>
        /// Gets the entry's unique identifier.
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the identifier of the tracker owning this entry.
        /// </summary>
        public Guid TrackerId
        {
            get
            {
                return _trackerId;
            }
        }

        /// <summary>
        /// Gets the entry's date in UTC.
        /// </summary>
        public DateTimeOffset Date
        {
            get
            {
                return _date;
            }
        }

        /// <summary>
        /// Gets the responses recorded in this entry.
        /// </summary>
        public IReadOnlyList<Response> Responses
        {
            get
            {
                return _readOnlyResponses;
            }
        }

        /// <summary>
        /// Changes the entry's date.
        /// </summary>
        /// <param name="date">The new date.</param>
        public void ChangeDate(DateTimeOffset date)
        {
            _date = date.ToUniversalTime();
        }

        /// <summary>
        /// Adds a response to this entry.
        /// </summary>
        /// <param name="response">The response to add.</param>
        public void AddResponse(Response response)
        {
            ArgumentNullException.ThrowIfNull(response);

            if (response.EntryId != _id)
            {
                throw new ArgumentException(
                    "The response does not belong to this entry.",
                    nameof(response));
            }

            if (_responses.Exists(
                existing => existing.FieldDefinitionId == response.FieldDefinitionId))
            {
                throw new InvalidOperationException(
                    "A response for this field already exists in this entry.");
            }

            _responses.Add(response);
        }
    }
}
