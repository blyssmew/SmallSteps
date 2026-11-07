
// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.EntriesTest
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Responses;
    using SmallSteps.Domain.Trackers;
    using Xunit;

    public class EntryResponseTest
    {
        private static (
            Tracker Tracker,
            Entry Entry,
            FieldDefinition Field)
            CreateSetup()
        {
            Tracker tracker = new Tracker("Test tracker");

            FieldDefinition field = new FieldDefinition(
                "Test field",
                FieldType.Text,
                0);

            tracker.AddField(field);

            Entry entry = new Entry(
                tracker.Id,
                DateTimeOffset.UtcNow);

            return (tracker, entry, field);
        }

        private static Response CreateResponse(
            Entry entry,
            Tracker tracker,
            FieldDefinition field)
        {
            return new Response(entry, tracker, field);
        }

        // Initial collection

        [Fact]
        public void Constructor_ShouldInitializeEmptyResponses()
        {
            var setup = CreateSetup();

            Assert.Empty(setup.Entry.Responses);
        }

        [Fact]
        public void Responses_ShouldReturnSameCollectionInstance()
        {
            var setup = CreateSetup();

            IReadOnlyList<Response> first = setup.Entry.Responses;
            IReadOnlyList<Response> second = setup.Entry.Responses;

            Assert.Same(first, second);
        }

        [Fact]
        public void Responses_ShouldReflectAddedResponses()
        {
            var setup = CreateSetup();

            IReadOnlyList<Response> responses = setup.Entry.Responses;

            Response response = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            setup.Entry.AddResponse(response);

            Assert.Single(responses);
            Assert.Same(response, responses[0]);
        }

        [Fact]
        public void Responses_ShouldBeReadOnly()
        {
            var setup = CreateSetup();

            IReadOnlyList<Response> responses = setup.Entry.Responses;

            Assert.True(responses is IList<Response>);

            IList<Response> mutableView = (IList<Response>)responses;

            Assert.True(mutableView.IsReadOnly);
        }

        [Fact]
        public void Responses_ShouldRejectExternalModification()
        {
            var setup = CreateSetup();

            IReadOnlyList<Response> responses = setup.Entry.Responses;

            IList<Response> mutableView = (IList<Response>)responses;

            Response response = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Assert.Throws<NotSupportedException>(
                () => mutableView.Add(response));
        }

        // AddResponse

        [Fact]
        public void AddResponse_ShouldAddResponse()
        {
            var setup = CreateSetup();

            Response response = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            setup.Entry.AddResponse(response);

            Assert.Single(setup.Entry.Responses);
            Assert.Same(response, setup.Entry.Responses[0]);
        }

        [Fact]
        public void AddResponse_ShouldRejectNull()
        {
            var setup = CreateSetup();

            Assert.Throws<ArgumentNullException>(
                () => setup.Entry.AddResponse(null!));
        }

        [Fact]
        public void AddResponse_ShouldRejectResponseFromAnotherEntry()
        {
            var setup = CreateSetup();

            Entry anotherEntry = new Entry(
                setup.Tracker.Id,
                DateTimeOffset.UtcNow);

            Response response = CreateResponse(
                anotherEntry,
                setup.Tracker,
                setup.Field);

            Assert.Throws<ArgumentException>(
                () => setup.Entry.AddResponse(response));

            Assert.Empty(setup.Entry.Responses);
        }

        [Fact]
        public void AddResponse_ShouldRejectDuplicateFieldResponse()
        {
            var setup = CreateSetup();

            Response first = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            setup.Entry.AddResponse(first);

            Assert.Throws<InvalidOperationException>(
                () => setup.Entry.AddResponse(second));

            Assert.Single(setup.Entry.Responses);
            Assert.Same(first, setup.Entry.Responses[0]);
        }

        [Fact]
        public void AddResponse_ShouldAllowDifferentFields()
        {
            var setup = CreateSetup();

            FieldDefinition secondField = new FieldDefinition(
                "Second field",
                FieldType.Checkbox,
                1);

            setup.Tracker.AddField(secondField);

            Response first = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = CreateResponse(
                setup.Entry,
                setup.Tracker,
                secondField);

            setup.Entry.AddResponse(first);
            setup.Entry.AddResponse(second);

            Assert.Equal(2, setup.Entry.Responses.Count);
        }

        [Fact]
        public void AddResponse_ShouldPreserveInsertionOrder()
        {
            var setup = CreateSetup();

            FieldDefinition secondField = new FieldDefinition(
                "Second field",
                FieldType.Checkbox,
                1);

            FieldDefinition thirdField = new FieldDefinition(
                "Third field",
                FieldType.Duration,
                2);

            setup.Tracker.AddField(secondField);
            setup.Tracker.AddField(thirdField);

            Response first = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = CreateResponse(
                setup.Entry,
                setup.Tracker,
                secondField);

            Response third = CreateResponse(
                setup.Entry,
                setup.Tracker,
                thirdField);

            setup.Entry.AddResponse(first);
            setup.Entry.AddResponse(second);
            setup.Entry.AddResponse(third);

            Assert.Same(first, setup.Entry.Responses[0]);
            Assert.Same(second, setup.Entry.Responses[1]);
            Assert.Same(third, setup.Entry.Responses[2]);
        }

        [Fact]
        public void AddResponse_ShouldPreserveExistingResponsesWhenAddingAnother()
        {
            var setup = CreateSetup();

            FieldDefinition secondField = new FieldDefinition(
                "Second field",
                FieldType.Checkbox,
                1);

            setup.Tracker.AddField(secondField);

            Response first = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            Response second = CreateResponse(
                setup.Entry,
                setup.Tracker,
                secondField);

            setup.Entry.AddResponse(first);
            setup.Entry.AddResponse(second);

            Assert.Contains(first, setup.Entry.Responses);
            Assert.Contains(second, setup.Entry.Responses);
        }

        // Independence

        [Fact]
        public void AddResponse_ShouldNotModifyAnotherEntry()
        {
            var setup = CreateSetup();

            Entry anotherEntry = new Entry(
                setup.Tracker.Id,
                DateTimeOffset.UtcNow);

            Response response = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            setup.Entry.AddResponse(response);

            Assert.Single(setup.Entry.Responses);
            Assert.Empty(anotherEntry.Responses);
        }

        [Fact]
        public void ChangeDate_ShouldPreserveResponses()
        {
            var setup = CreateSetup();

            Response response = CreateResponse(
                setup.Entry,
                setup.Tracker,
                setup.Field);

            setup.Entry.AddResponse(response);

            setup.Entry.ChangeDate(DateTimeOffset.UtcNow.AddDays(1));

            Assert.Single(setup.Entry.Responses);
            Assert.Same(response, setup.Entry.Responses[0]);
        }
    }
}
