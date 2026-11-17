// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the COPYING file in the project root for full license details.

namespace SmallSteps.Console
{
    using SmallSteps.Application.Trackers;
    using SmallSteps.Console.ConsoleUI;
    using SmallSteps.Console.Storage;

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Repositories
            InMemoryTrackerRepository trackerRepository = new();

            // UseCases
            CreateTrackerUseCase createTrackerUseCase = new(trackerRepository);
            GetAllTrackersUseCase getAllTrackersUseCase = new(trackerRepository);

            // Console UI
            TrackerMenu trackerMenu = new(createTrackerUseCase, getAllTrackersUseCase);

            ConsoleApp app = new(trackerMenu);
            app.Run();
        }
    }
}
