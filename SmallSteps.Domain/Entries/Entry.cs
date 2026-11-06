// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Domain.Entries
{
    using System;

    public class Entry
    {
        private readonly Guid _id;
        private readonly Guid _trackerId;
        private DateTimeOffset _date;

        public Entry(Guid trackerId, DateTimeOffset date)
        {
            if (trackerId == Guid.Empty)
            {
                throw new ArgumentException("An entry must belong to a tracker.", nameof(trackerId));
            }

            _id = Guid.NewGuid();
            _trackerId = trackerId;
            _date = date.ToUniversalTime();
        }

        public Guid Id { get { return _id; } }
        public Guid TrackerId { get { return _trackerId; } }
        public DateTimeOffset Date { get { return _date; } }

        public void ChangeDate(DateTimeOffset date)
        {
            _date = date.ToUniversalTime();
        }
    }
}
