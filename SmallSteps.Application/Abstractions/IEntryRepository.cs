// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Application.Abstractions
{
    using SmallSteps.Domain.Entries;
    using System.Collections.Generic;

    public interface IEntryRepository
    {
        void Add(Entry entry);

        IReadOnlyList<Entry> GetByTrackerId(Guid trackerId);
    }
}
