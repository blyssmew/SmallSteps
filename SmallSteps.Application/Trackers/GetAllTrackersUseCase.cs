// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the COPYING file in the project root for full license details.

namespace SmallSteps.Application.Trackers
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Abstractions;
    using SmallSteps.Domain.Trackers;

    public class GetAllTrackersUseCase
    {
        private readonly ITrackerRepository _trackerRepository;

        public GetAllTrackersUseCase(ITrackerRepository trackerRepository)
        {
            ArgumentNullException.ThrowIfNull(trackerRepository);

            _trackerRepository = trackerRepository;
        }

        public IReadOnlyList<Tracker> Execute()
        {
            return _trackerRepository.GetAll();
        }
    }
}
