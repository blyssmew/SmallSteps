// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.ApplicationTest.Entries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SmallSteps.Application.Entries;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Responses;
    using SmallSteps.Domain.Trackers;
    using SmallSteps.Tests.ApplicationTest.TestDoubles;
    using Xunit;

    public class AddEntryUseCaseTest
    {
        private readonly InMemoryTrackerRepository _trackerRepository;
        private readonly InMemoryEntryRepository _entryRepository;

        private readonly Tracker _tracker;
        private readonly FieldDefinition _checkboxField;
        private readonly FieldDefinition _durationField;
        private readonly FieldDefinition _textField;

        private readonly AddEntryUseCase _useCase;

        public AddEntryUseCaseTest()
        {
            _trackerRepository = new InMemoryTrackerRepository();
            _entryRepository = new InMemoryEntryRepository();

            _tracker = new Tracker("Daily habits");

            _checkboxField = new FieldDefinition("Exercise", FieldType.Checkbox, 0);
            _durationField = new FieldDefinition("Duration", FieldType.Duration, 1);
            _textField = new FieldDefinition("Notes", FieldType.Text, 2);

            _tracker.AddField(_checkboxField);
            _tracker.AddField(_durationField);
            _tracker.AddField(_textField);

            _trackerRepository.Add(_tracker);

            _useCase = new AddEntryUseCase(_trackerRepository, _entryRepository);
        }

        [Fact]
        public void Execute_ShouldCreateAndSaveEntryWithResponses()
        {
            DateTimeOffset date = new(
                2026, 11, 17, 8, 30, 0,
                TimeSpan.FromHours(-5));

            Dictionary<Guid, object> answers = new()
            {
                [_checkboxField.Id] = true,
                [_durationField.Id] = 45,
                [_textField.Id] = "Had a productive day!"
            };

            Entry entry = _useCase.Execute(_tracker.Id, date, answers);

            Assert.NotEqual(Guid.Empty, entry.Id);
            Assert.Equal(_tracker.Id, entry.TrackerId);
            Assert.Equal(date.ToUniversalTime(), entry.Date);

            Assert.Equal(3, entry.Responses.Count);

            Response checkboxResponse = entry.Responses.Single(response => response.FieldDefinitionId == _checkboxField.Id);

            Response durationResponse = entry.Responses.Single(response => response.FieldDefinitionId == _durationField.Id);

            Response textResponse = entry.Responses.Single(response => response.FieldDefinitionId == _textField.Id);

            Assert.True(checkboxResponse.TryGetValue(out object? checkboxValue));
            Assert.Equal(true, checkboxValue);

            Assert.True(durationResponse.TryGetValue(out object? durationValue));
            Assert.Equal(45, durationValue);

            Assert.True(textResponse.TryGetValue(out object? textValue));
            Assert.Equal("Had a productive day!", textValue);

            Assert.Same(entry, Assert.Single(_entryRepository.GetByTrackerId(_tracker.Id)));

            Assert.Equal(1, _entryRepository.Count);
        }

        [Fact]
        public void Execute_ShouldAllowUnansweredFields()
        {
            Entry entry = _useCase.Execute(_tracker.Id, DateTimeOffset.UtcNow, new Dictionary<Guid, object>());

            Assert.Equal(3, entry.Responses.Count);

            Assert.All(entry.Responses, response => Assert.False(response.HasValue));

            Assert.Equal(1, _entryRepository.Count);
        }

        [Fact]
        public void Execute_ShouldRejectUnknownTracker()
        {
            Assert.Throws<KeyNotFoundException>(() => _useCase.Execute(Guid.NewGuid(), DateTimeOffset.UtcNow, new Dictionary<Guid, object>()));

            Assert.Equal(0, _entryRepository.Count);
        }

        [Fact]
        public void Execute_ShouldRejectFieldThatDoesNotBelongToTracker()
        {
            FieldDefinition foreignField = new("Foreign field", FieldType.Checkbox, 0);

            Dictionary<Guid, object> answers = new()
            {
                [foreignField.Id] = true
            };

            Assert.Throws<ArgumentException>(() => _useCase.Execute(_tracker.Id, DateTimeOffset.UtcNow, answers));

            Assert.Equal(0, _entryRepository.Count);
        }

        [Fact]
        public void Execute_ShouldRejectValueThatDoesNotMatchFieldType()
        {
            Dictionary<Guid, object> answers = new()
            {
                [_checkboxField.Id] = "This should be a boolean."
            };

            Assert.Throws<ArgumentException>(() => _useCase.Execute(_tracker.Id, DateTimeOffset.UtcNow, answers));

            Assert.Equal(0, _entryRepository.Count);
        }

        [Fact]
        public void Execute_ShouldRejectNullAnswers()
        {
            Assert.Throws<ArgumentNullException>(() => _useCase.Execute(_tracker.Id, DateTimeOffset.UtcNow, null!));

            Assert.Equal(0, _entryRepository.Count);
        }
    }
}
