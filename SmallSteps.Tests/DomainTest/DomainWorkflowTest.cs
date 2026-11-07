// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.DomainTest
{
    using System;
    using System.Linq;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Responses;
    using SmallSteps.Domain.Trackers;
    using Xunit;

    public class DomainWorkflowTest
    {
        [Fact]
        public void CompleteTrackerWorkflow_ShouldRecordAndManageResponses()
        {
            // 1. Create a tracker.
            Tracker tracker = new("Daily habits");

            // 2. Define its fields.
            FieldDefinition exerciseField = new(
                "Did I exercise?",
                FieldType.Checkbox,
                0);

            FieldDefinition sleepField = new(
                "How long did I sleep?",
                FieldType.Duration,
                1);

            FieldDefinition dayField = new(
                "How was my day?",
                FieldType.Text,
                2);

            // 3. Add the fields to the tracker.
            tracker.AddField(exerciseField);
            tracker.AddField(sleepField);
            tracker.AddField(dayField);

            Assert.Equal(3, tracker.Fields.Count);

            // 4. Create an entry for this tracker.
            DateTimeOffset date = new(
                2026, 11, 7, 10, 0, 0,
                TimeSpan.FromHours(-5));

            Entry entry = new(tracker.Id, date);

            Assert.Equal(tracker.Id, entry.TrackerId);
            Assert.Equal(TimeSpan.Zero, entry.Date.Offset);

            // 5. Create one response for each field.
            foreach (FieldDefinition field in tracker.Fields)
            {
                Response response = new(
                    entry,
                    tracker,
                    field);

                entry.AddResponse(response);
            }

            Assert.Equal(3, entry.Responses.Count);

            // 6. Retrieve the responses using their field identities.
            Response exerciseResponse = entry.Responses.Single(
                response => response.FieldDefinitionId == exerciseField.Id);

            Response sleepResponse = entry.Responses.Single(
                response => response.FieldDefinitionId == sleepField.Id);

            Response dayResponse = entry.Responses.Single(
                response => response.FieldDefinitionId == dayField.Id);

            // 7. Verify that all responses are initially unanswered.
            Assert.False(exerciseResponse.HasValue);
            Assert.False(sleepResponse.HasValue);
            Assert.False(dayResponse.HasValue);

            // 8. Assign answers using their corresponding value types.
            exerciseResponse.SetValue(true);
            sleepResponse.SetValue(480);
            dayResponse.SetValue("A very good day!");

            // 9. Verify the assigned answers.
            Assert.True(exerciseResponse.HasValue);
            Assert.True(sleepResponse.HasValue);
            Assert.True(dayResponse.HasValue);

            Assert.True(exerciseResponse.TryGetValue(out object? exerciseValue));
            Assert.True(sleepResponse.TryGetValue(out object? sleepValue));
            Assert.True(dayResponse.TryGetValue(out object? dayValue));

            Assert.IsType<bool>(exerciseValue);
            Assert.IsType<int>(sleepValue);
            Assert.IsType<string>(dayValue);

            Assert.Equal(true, exerciseValue);
            Assert.Equal(480, sleepValue);
            Assert.Equal("A very good day!", dayValue);

            // 10. Verify that each response references the correct objects.
            foreach (Response response in entry.Responses)
            {
                Assert.Equal(entry.Id, response.EntryId);
            }

            Assert.Equal(exerciseField.Id, exerciseResponse.FieldDefinitionId);
            Assert.Equal(sleepField.Id, sleepResponse.FieldDefinitionId);
            Assert.Equal(dayField.Id, dayResponse.FieldDefinitionId);

            Assert.Equal(FieldType.Checkbox, exerciseResponse.Type);
            Assert.Equal(FieldType.Duration, sleepResponse.Type);
            Assert.Equal(FieldType.Text, dayResponse.Type);

            // 11. Modify an existing answer.
            sleepResponse.SetValue(450);

            Assert.True(sleepResponse.TryGetValue(out sleepValue));
            Assert.Equal(450, sleepValue);

            // 12. Clear an answer without removing its response.
            dayResponse.Clear();

            Assert.False(dayResponse.HasValue);
            Assert.False(dayResponse.TryGetValue(out dayValue));
            Assert.Null(dayValue);

            Assert.Equal(3, entry.Responses.Count);

            // 13. Assign a new answer after clearing.
            dayResponse.SetValue("An even better day!");

            Assert.True(dayResponse.TryGetValue(out dayValue));
            Assert.Equal("An even better day!", dayValue);
        }
    }
}
