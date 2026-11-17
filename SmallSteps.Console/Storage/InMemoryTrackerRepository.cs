// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the COPYING file in the project root for full license details.

namespace SmallSteps.Console.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Trackers;

    public class InMemoryTrackerRepository : ITrackerRepository
    {
        private readonly Dictionary<Guid, Tracker> _trackers = new();

        public void Add(Tracker tracker)
        {
            ArgumentNullException.ThrowIfNull(tracker);

            _trackers.Add(tracker.Id, tracker);
        }

        public Tracker? GetById(Guid id)
        {
            return _trackers.TryGetValue(id, out Tracker? tracker) ? tracker : null;
        }

        public IReadOnlyList<Tracker> GetAll()
        {
            return _trackers.Values.ToList();
        }
    }
}
