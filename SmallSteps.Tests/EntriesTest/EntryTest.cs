
// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

namespace SmallSteps.Tests.EntriesTest
{
    using System;
    using System.Globalization;
    using SmallSteps.Domain.Entries;
    using Xunit;

    public class EntryTest
    {
        private static DateTimeOffset CreateDate()
        {
            return new DateTimeOffset(
                2026, 11, 6, 13, 30, 0,
                TimeSpan.FromHours(-5));
        }

        // Constructor

        [Fact]
        public void Constructor_ShouldGenerateNonEmptyId()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            Assert.NotEqual(Guid.Empty, entry.Id);
        }

        [Fact]
        public void Constructor_ShouldGenerateUniqueIds()
        {
            Guid trackerId = Guid.NewGuid();
            DateTimeOffset date = CreateDate();

            Entry first = new(trackerId, date);
            Entry second = new(trackerId, date);

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public void Constructor_ShouldPreserveTrackerId()
        {
            Guid trackerId = Guid.NewGuid();

            Entry entry = new(trackerId, CreateDate());

            Assert.Equal(trackerId, entry.TrackerId);
        }

        [Fact]
        public void Constructor_ShouldRejectEmptyTrackerId()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new Entry(Guid.Empty, CreateDate()));

            Assert.Equal("trackerId", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldNormalizeDateToUtc()
        {
            DateTimeOffset inputDate = CreateDate();

            Entry entry = new(Guid.NewGuid(), inputDate);

            Assert.Equal(inputDate.ToUniversalTime(), entry.Date);
        }

        [Fact]
        public void Constructor_ShouldPreserveDateInstant()
        {
            DateTimeOffset inputDate = CreateDate();

            Entry entry = new(Guid.NewGuid(), inputDate);

            Assert.Equal(inputDate.UtcDateTime, entry.Date.UtcDateTime);
        }

        [Fact]
        public void Constructor_ShouldSetDateOffsetToZero()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            Assert.Equal(TimeSpan.Zero, entry.Date.Offset);
        }

        [Fact]
        public void Constructor_ShouldPreserveDateTimePrecision()
        {
            DateTimeOffset inputDate = CreateDate().AddTicks(1234567);

            Entry entry = new(Guid.NewGuid(), inputDate);

            Assert.Equal(inputDate.UtcDateTime, entry.Date.UtcDateTime);
        }

        // Date formatting

        [Fact]
        public void Date_ShouldBeRepresentableAsUtcIso8601()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            string formattedDate = entry.Date.UtcDateTime.ToString("O", CultureInfo.InvariantCulture);

            Assert.Equal("2026-11-06T18:30:00.0000000Z", formattedDate);
        }

        [Fact]
        public void Date_ShouldHaveUtcDateTimeKind()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            Assert.Equal(DateTimeKind.Utc, entry.Date.UtcDateTime.Kind);
        }

        // ChangeDate

        [Fact]
        public void ChangeDate_ShouldUpdateDate()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());
            DateTimeOffset newDate = CreateDate().AddDays(1);

            entry.ChangeDate(newDate);

            Assert.Equal(newDate.ToUniversalTime(), entry.Date);
        }

        [Fact]
        public void ChangeDate_ShouldNormalizeDateToUtc()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            DateTimeOffset newDate = new(
                2026, 11, 7,
                10, 0, 0,
                TimeSpan.FromHours(2));

            entry.ChangeDate(newDate);

            Assert.Equal(TimeSpan.Zero, entry.Date.Offset);
            Assert.Equal(newDate.ToUniversalTime(), entry.Date);
        }

        [Fact]
        public void ChangeDate_ShouldPreserveDateInstant()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            DateTimeOffset newDate = CreateDate().AddHours(4);

            entry.ChangeDate(newDate);

            Assert.Equal(newDate.UtcDateTime, entry.Date.UtcDateTime);
        }

        [Fact]
        public void ChangeDate_ShouldPreserveId()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());
            Guid originalId = entry.Id;

            entry.ChangeDate(CreateDate().AddDays(1));

            Assert.Equal(originalId, entry.Id);
        }

        [Fact]
        public void ChangeDate_ShouldPreserveTrackerId()
        {
            Guid trackerId = Guid.NewGuid();
            Entry entry = new(trackerId, CreateDate());

            entry.ChangeDate(CreateDate().AddDays(1));

            Assert.Equal(trackerId, entry.TrackerId);
        }

        [Fact]
        public void ChangeDate_ShouldAllowMultipleChanges()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            DateTimeOffset secondDate = CreateDate().AddDays(1);
            DateTimeOffset thirdDate = CreateDate().AddDays(2);

            entry.ChangeDate(secondDate);
            entry.ChangeDate(thirdDate);

            Assert.Equal(thirdDate.ToUniversalTime(), entry.Date);
        }

        [Fact]
        public void ChangeDate_ShouldAllowSameDate()
        {
            DateTimeOffset date = CreateDate();
            Entry entry = new(Guid.NewGuid(), date);

            entry.ChangeDate(date);

            Assert.Equal(date.ToUniversalTime(), entry.Date);
        }

        [Fact]
        public void ChangeDate_ShouldKeepDateInUtc()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            entry.ChangeDate(CreateDate().AddDays(1));

            Assert.Equal(TimeSpan.Zero, entry.Date.Offset);
        }

        [Fact]
        public void ChangeDate_ShouldUpdateUtcIso8601Representation()
        {
            Entry entry = new(Guid.NewGuid(), CreateDate());

            DateTimeOffset newDate = CreateDate().AddDays(1);
            entry.ChangeDate(newDate);

            string formattedDate = entry.Date.UtcDateTime.ToString("O", CultureInfo.InvariantCulture);

            Assert.EndsWith("Z", formattedDate);
        }

        // Independence

        [Fact]
        public void Entries_ShouldHaveIndependentDates()
        {
            Guid trackerId = Guid.NewGuid();

            Entry first = new(trackerId, CreateDate());
            Entry second = new(trackerId, CreateDate().AddDays(1));

            first.ChangeDate(CreateDate().AddDays(2));

            Assert.NotEqual(first.Date, second.Date);
        }

        [Fact]
        public void Entries_ShouldHaveIndependentIds()
        {
            Guid trackerId = Guid.NewGuid();
            DateTimeOffset date = CreateDate();

            Entry first = new(trackerId, date);
            Entry second = new(trackerId, date);

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public void ChangeDate_ShouldNotAffectAnotherEntry()
        {
            Guid trackerId = Guid.NewGuid();
            DateTimeOffset originalDate = CreateDate();

            Entry first = new(trackerId, originalDate);
            Entry second = new(trackerId, originalDate);

            first.ChangeDate(originalDate.AddDays(1));

            Assert.Equal(originalDate.ToUniversalTime(), second.Date);
        }

        [Fact]
        public void Entries_ShouldAllowDifferentTrackers()
        {
            Guid firstTrackerId = Guid.NewGuid();
            Guid secondTrackerId = Guid.NewGuid();

            Entry first = new(firstTrackerId, CreateDate());
            Entry second = new(secondTrackerId, CreateDate());

            Assert.NotEqual(first.TrackerId, second.TrackerId);
            Assert.NotEqual(first.Id, second.Id);
        }
    }
}
