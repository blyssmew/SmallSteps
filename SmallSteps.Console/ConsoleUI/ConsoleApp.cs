// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the COPYING file in the project root for full license details.

namespace SmallSteps.Console.ConsoleUI
{
    using System;

    public class ConsoleApp
    {
        private readonly TrackerMenu _trackerMenu;
        private bool _isRunning = true;

        public ConsoleApp(TrackerMenu trackerMenu)
        {
            ArgumentNullException.ThrowIfNull(trackerMenu);

            _trackerMenu = trackerMenu;
        }

        public void Run()
        {
            while (_isRunning)
            {
                DisplayMainMenu();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _trackerMenu.Run();
                        break;

                    case "0":
                    case null:
                        _isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        private static void DisplayMainMenu()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("           SMALLSTEPS");
            Console.WriteLine("================================");
            Console.WriteLine();
            Console.WriteLine("1. Manage trackers");
            Console.WriteLine();
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Choose an option: ");
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
