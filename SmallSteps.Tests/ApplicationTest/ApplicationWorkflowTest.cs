// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.ApplicationTest
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Entries;
    using SmallSteps.Application.Trackers;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Responses;
    using SmallSteps.Domain.Trackers;
    using SmallSteps.Tests.ApplicationTest.TestDoubles;
    using Xunit;

    public class ApplicationWorkflowTest
    {
        [Fact]
        public void CompleteApplicationWorkflow_ShouldCreateTrackersRecordEntriesAndRetrieveHistory()
        {
            // Arrange: Create the in-memory repositories.
            InMemoryTrackerRepository trackerRepository = new();
            InMemoryEntryRepository entryRepository = new();

            // Arrange: Create the use cases.
            CreateTrackerUseCase createTracker = new(trackerRepository);

            AddEntryUseCase addEntry = new(trackerRepository, entryRepository);

            GetTrackerHistoryUseCase getTrackerHistory = new(trackerRepository, entryRepository);

            // Act: Create a tracker with three fields.
            List<FieldDefinition> fields =
            [
                new FieldDefinition("Exercise", FieldType.Checkbox, 0),
                new FieldDefinition("Duration", FieldType.Duration, 1),
                new FieldDefinition("Notes", FieldType.Text, 2)
            ];

            Tracker tracker = createTracker.Execute("Daily habits", fields);

            Assert.Equal(1, trackerRepository.Count);
            Assert.Equal(3, tracker.Fields.Count);

            // Act: Add the first entry.
            DateTimeOffset firstDate = new(2026, 11, 15, 8, 0, 0, TimeSpan.Zero);

            Dictionary<Guid, object> firstAnswers = new()
            {
                [fields[0].Id] = true,
                [fields[1].Id] = 30,
                [fields[2].Id] = "Went for a walk."
            };

            Entry firstEntry = addEntry.Execute(tracker.Id, firstDate, firstAnswers);

            // Act: Add the second entry.
            DateTimeOffset secondDate = new(2026, 11, 17, 8, 0, 0, TimeSpan.Zero);

            Dictionary<Guid, object> secondAnswers = new()
            {
                [fields[0].Id] = false,
                [fields[1].Id] = 0,
                [fields[2].Id] = "Rest day."
            };

            Entry secondEntry = addEntry.Execute(tracker.Id, secondDate, secondAnswers);

            Assert.Equal(2, entryRepository.Count);

            // Act: Retrieve the tracker history.
            IReadOnlyList<Entry> history = getTrackerHistory.Execute(tracker.Id);

            // Assert: Verify the history.
            Assert.Equal(2, history.Count);

            Assert.Equal(secondEntry.Id, history[0].Id);
            Assert.Equal(firstEntry.Id, history[1].Id);

            // Assert: Verify the second entry and its responses.
            Entry latestEntry = history[0];

            Assert.Equal(tracker.Id, latestEntry.TrackerId);
            Assert.Equal(secondDate, latestEntry.Date);
            Assert.Equal(3, latestEntry.Responses.Count);

            Response checkboxResponse = GetResponse(latestEntry, fields[0].Id);

            Response durationResponse = GetResponse(latestEntry, fields[1].Id);

            Response textResponse = GetResponse(latestEntry, fields[2].Id);

            Assert.True(checkboxResponse.TryGetValue(out object? checkboxValue));
            Assert.Equal(false, checkboxValue);

            Assert.True(durationResponse.TryGetValue(out object? durationValue));
            Assert.Equal(0, durationValue);

            Assert.True(textResponse.TryGetValue(out object? textValue));
            Assert.Equal("Rest day.", textValue);

            // Assert: Verify that the first entry is still preserved.
            Assert.Equal(3, history[1].Responses.Count);
            Assert.Equal(firstDate, history[1].Date);
        }

        private static Response GetResponse(Entry entry, Guid fieldId)
        {
            foreach (Response response in entry.Responses)
            {
                if (response.FieldDefinitionId == fieldId)
                {
                    return response;
                }
            }

            throw new InvalidOperationException("The expected response was not found.");
        }
    }
}
