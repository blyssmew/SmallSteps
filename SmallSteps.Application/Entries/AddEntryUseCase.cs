// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Application.Entries
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Entries;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Responses;
    using SmallSteps.Domain.Trackers;

    public class AddEntryUseCase
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEntryRepository _entryRepository;

        public AddEntryUseCase(ITrackerRepository trackerRepository, IEntryRepository entryRepository)
        {
            ArgumentNullException.ThrowIfNull(trackerRepository);
            ArgumentNullException.ThrowIfNull(entryRepository);

            _trackerRepository = trackerRepository;
            _entryRepository = entryRepository;
        }

        public Entry Execute(Guid trackerId, DateTimeOffset date, IReadOnlyDictionary<Guid, object> answers)
        {
            ArgumentNullException.ThrowIfNull(answers);

            Tracker tracker = _trackerRepository.GetById(trackerId) ?? throw new KeyNotFoundException("The tracker was not found.");

            // Verify that every provided answer belongs to a field of the tracker.
            foreach (Guid fieldId in answers.Keys)
            {
                bool fieldExists = false;

                foreach (FieldDefinition field in tracker.Fields)
                {
                    if (field.Id == fieldId)
                    {
                        fieldExists = true;
                        break;
                    }
                }

                if (!fieldExists)
                {
                    throw new ArgumentException("An answer was provided for a field that does not belong to this tracker.", nameof(answers));
                }
            }

            Entry entry = new(tracker.Id, date);

            // Create one response for every field in the tracker.
            foreach (FieldDefinition field in tracker.Fields)
            {
                Response response = new(entry, tracker, field);

                // Set the answer if one was provided.
                if (answers.TryGetValue(field.Id, out var value))
                {
                    response.SetValue(value);
                }

                entry.AddResponse(response);
            }

            _entryRepository.Add(entry);

            return entry;
        }
    }
}
