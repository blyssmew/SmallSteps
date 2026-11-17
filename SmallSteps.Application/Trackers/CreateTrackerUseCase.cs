// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Application.Trackers
{
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Trackers;

    public class CreateTrackerUseCase
    {
        private readonly ITrackerRepository _trackerRepository;

        public CreateTrackerUseCase(ITrackerRepository trackerRepository)
        {
            ArgumentNullException.ThrowIfNull(trackerRepository);

            _trackerRepository = trackerRepository;
        }

        public Tracker Execute(string name, IReadOnlyList<FieldDefinition> fields)
        {
            ArgumentNullException.ThrowIfNull(fields);

            Tracker tracker = new(name);

            foreach (FieldDefinition field in fields)
            {
                tracker.AddField(field);
            }

            _trackerRepository.Add(tracker);

            return tracker;
        }
    }
}
