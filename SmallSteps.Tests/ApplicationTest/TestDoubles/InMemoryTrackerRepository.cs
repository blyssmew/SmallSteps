// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.ApplicationTest.TestDoubles
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Trackers;

    internal sealed class InMemoryTrackerRepository : ITrackerRepository
    {
        private readonly Dictionary<Guid, Tracker> _trackers = [];

        public int Count
        {
            get { return _trackers.Count; }
        }

        public void Add(Tracker tracker)
        {
            ArgumentNullException.ThrowIfNull(tracker);

            _trackers.Add(tracker.Id, tracker);
        }

        public Tracker? GetById(Guid id)
        {
            return _trackers.TryGetValue(id, out Tracker? tracker) ? tracker : null;
        }
    }
}
