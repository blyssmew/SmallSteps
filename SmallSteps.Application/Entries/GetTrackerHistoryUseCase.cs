// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Application.Entries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Trackers;

    public class GetTrackerHistoryUseCase
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEntryRepository _entryRepository;

        public GetTrackerHistoryUseCase(ITrackerRepository trackerRepository, IEntryRepository entryRepository)
        {
            ArgumentNullException.ThrowIfNull(trackerRepository);
            ArgumentNullException.ThrowIfNull(entryRepository);

            _trackerRepository = trackerRepository;
            _entryRepository = entryRepository;
        }

        public IReadOnlyList<Entry> Execute(Guid trackerId)
        {
            if (trackerId == Guid.Empty)
            {
                throw new ArgumentException("The tracker ID cannot be empty.", nameof(trackerId));
            }

            // Verify that the tracker exists.
            Tracker tracker = _trackerRepository.GetById(trackerId) ?? throw new KeyNotFoundException("The tracker was not found.");

            // Retrieve and sort its entries, newest first.
            IReadOnlyList<Entry> entries = _entryRepository.GetByTrackerId(tracker.Id);

            return entries.OrderByDescending(entry => entry.Date).ToList();
        }
    }
}
