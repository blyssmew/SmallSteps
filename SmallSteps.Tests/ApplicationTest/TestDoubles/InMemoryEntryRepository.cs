// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.ApplicationTest.TestDoubles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Entries;

    internal sealed class InMemoryEntryRepository : IEntryRepository
    {
        private readonly List<Entry> _entries = new();

        public int Count
        {
            get { return _entries.Count; }
        }

        public void Add(Entry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);

            _entries.Add(entry);
        }

        public IReadOnlyList<Entry> GetByTrackerId(Guid trackerId)
        {
            return _entries.Where(entry => entry.TrackerId == trackerId).ToList();
        }
    }
}
