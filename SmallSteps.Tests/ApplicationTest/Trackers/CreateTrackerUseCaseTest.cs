// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.ApplicationTest.Trackers
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Trackers;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Trackers;
    using SmallSteps.Tests.ApplicationTest.TestDoubles;
    using Xunit;

    public class CreateTrackerUseCaseTest
    {
        private readonly InMemoryTrackerRepository _trackerRepository;
        private readonly CreateTrackerUseCase _useCase;

        public CreateTrackerUseCaseTest()
        {
            _trackerRepository = new InMemoryTrackerRepository();
            _useCase = new CreateTrackerUseCase(_trackerRepository);
        }

        [Fact]
        public void Execute_ShouldCreateAndSaveTrackerWithFields()
        {
            List<FieldDefinition> fields =
            [
                new FieldDefinition("Exercise", FieldType.Checkbox, 0),
                new FieldDefinition("Duration", FieldType.Duration, 1),
                new FieldDefinition("Notes", FieldType.Text, 2)
            ];

            Tracker tracker = _useCase.Execute("Daily habits", fields);

            Assert.NotEqual(Guid.Empty, tracker.Id);
            Assert.Equal("Daily habits", tracker.Name);
            Assert.Equal(3, tracker.Fields.Count);

            Assert.Equal(fields, tracker.Fields);

            Assert.Same(tracker, _trackerRepository.GetById(tracker.Id));

            Assert.Equal(1, _trackerRepository.Count);
        }

        [Fact]
        public void Execute_ShouldRejectInvalidTrackerName()
        {
            Assert.Throws<ArgumentException>(() => _useCase.Execute(" ", new List<FieldDefinition>()));

            Assert.Equal(0, _trackerRepository.Count);
        }

        [Fact]
        public void Execute_ShouldRejectNullFields()
        {
            Assert.Throws<ArgumentNullException>(() => _useCase.Execute("Daily habits", null!));

            Assert.Equal(0, _trackerRepository.Count);
        }
    }
}
