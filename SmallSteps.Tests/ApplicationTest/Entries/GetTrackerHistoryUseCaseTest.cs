// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.ApplicationTest.Entries
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Entries;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Trackers;
    using SmallSteps.Tests.ApplicationTest.TestDoubles;
    using Xunit;

    public class GetTrackerHistoryUseCaseTest
    {
        private readonly InMemoryTrackerRepository _trackerRepository;
        private readonly InMemoryEntryRepository _entryRepository;

        private readonly Tracker _tracker;
        private readonly GetTrackerHistoryUseCase _useCase;

        public GetTrackerHistoryUseCaseTest()
        {
            _trackerRepository = new InMemoryTrackerRepository();
            _entryRepository = new InMemoryEntryRepository();

            _tracker = new Tracker("Daily habits");

            _trackerRepository.Add(_tracker);

            _useCase = new GetTrackerHistoryUseCase(_trackerRepository, _entryRepository);
        }

        [Fact]
        public void Execute_ShouldReturnEntriesNewestFirst()
        {
            Entry oldestEntry = new(_tracker.Id, new DateTimeOffset(2026, 11, 15, 8, 0, 0, TimeSpan.Zero));

            Entry newestEntry = new(_tracker.Id, new DateTimeOffset(2026, 11, 17, 8, 0, 0, TimeSpan.Zero));

            Entry middleEntry = new(_tracker.Id, new DateTimeOffset(2026, 11, 16, 8, 0, 0, TimeSpan.Zero));

            _entryRepository.Add(oldestEntry);
            _entryRepository.Add(newestEntry);
            _entryRepository.Add(middleEntry);

            IReadOnlyList<Entry> history = _useCase.Execute(_tracker.Id);

            Assert.Equal(3, history.Count);

            Assert.Equal(newestEntry.Id, history[0].Id);
            Assert.Equal(middleEntry.Id, history[1].Id);
            Assert.Equal(oldestEntry.Id, history[2].Id);
        }

        [Fact]
        public void Execute_ShouldReturnEmptyListWhenNoEntriesExist()
        {
            IReadOnlyList<Entry> history = _useCase.Execute(_tracker.Id);

            Assert.Empty(history);
        }

        [Fact]
        public void Execute_ShouldNotReturnEntriesFromAnotherTracker()
        {
            Tracker anotherTracker = new("Another tracker");
            _trackerRepository.Add(anotherTracker);

            Entry trackerEntry = new(_tracker.Id, DateTimeOffset.UtcNow);

            Entry anotherEntry = new(anotherTracker.Id, DateTimeOffset.UtcNow);

            _entryRepository.Add(trackerEntry);
            _entryRepository.Add(anotherEntry);

            IReadOnlyList<Entry> history = _useCase.Execute(_tracker.Id);

            Assert.Single(history);
            Assert.Equal(trackerEntry.Id, history[0].Id);
        }

        [Fact]
        public void Execute_ShouldRejectUnknownTracker()
        {
            Assert.Throws<KeyNotFoundException>(() => _useCase.Execute(Guid.NewGuid()));
        }

        [Fact]
        public void Execute_ShouldRejectEmptyTrackerId()
        {
            Assert.Throws<ArgumentException>(() => _useCase.Execute(Guid.Empty));
        }
    }
}
