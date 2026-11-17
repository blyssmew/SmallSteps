// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the COPYING file in the project root for full license details.

namespace SmallSteps.Console.ConsoleUI
{
    public class ConsoleApp
    {
        private bool _isRunning = true;

        public void Run()
        {
            while (_isRunning)
            {
                DisplayMainMenu();

                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowTrackerMenu();
                        break;

                    case "0":
                        _isRunning = false;
                        break;

                    case null:
                        _isRunning = false;
                        break;

                    default:
                        System.Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }

            System.Console.WriteLine("Goodbye!");
        }

        private static void DisplayMainMenu()
        {
            System.Console.Clear();

            System.Console.WriteLine("================================");
            System.Console.WriteLine("           SMALLSTEPS");
            System.Console.WriteLine("================================");
            System.Console.WriteLine();
            System.Console.WriteLine("1. Manage trackers");
            System.Console.WriteLine();
            System.Console.WriteLine("0. Exit");
            System.Console.WriteLine();
            System.Console.Write("Choose an option: ");
        }

        private static void ShowTrackerMenu()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Tracker management will be implemented next.");
            Pause();
        }

        private static void Pause()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Press Enter to continue...");
            System.Console.ReadLine();
        }
    }
}
