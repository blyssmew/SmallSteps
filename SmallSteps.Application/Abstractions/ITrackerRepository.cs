// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Application.Abstractions
{
    using System;
    using SmallSteps.Domain.Trackers;

    public interface ITrackerRepository
    {
        void Add(Tracker tracker);

        Tracker? GetById(Guid id);

        IReadOnlyList<Tracker> GetAll();
    }
}
