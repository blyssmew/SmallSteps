// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the COPYING file in the project root for full license details.

namespace SmallSteps.Console.ConsoleUI
{
    using System;
    using System.Collections.Generic;
    using SmallSteps.Application.Trackers;
    using SmallSteps.Domain.Fields;
    using SmallSteps.Domain.Trackers;

    public class TrackerMenu
    {
        private readonly CreateTrackerUseCase _createTrackerUseCase;
        private readonly GetAllTrackersUseCase _getAllTrackersUseCase;

        public TrackerMenu(CreateTrackerUseCase createTrackerUseCase, GetAllTrackersUseCase getAllTrackersUseCase)
        {
            ArgumentNullException.ThrowIfNull(createTrackerUseCase);
            ArgumentNullException.ThrowIfNull(getAllTrackersUseCase);

            _createTrackerUseCase = createTrackerUseCase;
            _getAllTrackersUseCase = getAllTrackersUseCase;
        }

        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                DisplayMenu();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayTrackerList();
                        break;

                    case "2":
                        CreateTracker();
                        break;

                    case "0":
                    case null:
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private static void DisplayMenu()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("         MANAGE TRACKERS");
            Console.WriteLine("================================");
            Console.WriteLine();
            Console.WriteLine("1. View trackers");
            Console.WriteLine("2. Create a tracker");
            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("Choose an option: ");
        }

        private void DisplayTrackerList()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("           MY TRACKERS");
            Console.WriteLine("================================");
            Console.WriteLine();

            IReadOnlyList<Tracker> trackers = _getAllTrackersUseCase.Execute();

            if (trackers.Count == 0)
            {
                Console.WriteLine("You don't have any trackers yet.");
            }
            else
            {
                for (int i = 0; i < trackers.Count; i++)
                {
                    Tracker tracker = trackers[i];

                    Console.WriteLine($"{i + 1}. {tracker.Name}");

                    foreach (FieldDefinition field in tracker.Fields)
                    {
                        Console.WriteLine($"   - {field.Name} ({field.Type})");
                    }

                    Console.WriteLine();
                }
            }

            Pause();
        }

        private void CreateTracker()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("          NEW TRACKER");
            Console.WriteLine("================================");
            Console.WriteLine();

            Console.Write("Tracker name: ");

            string? trackerName = Console.ReadLine();

            if (trackerName is null)
                return;

            List<FieldDefinition> fields = [];

            Console.WriteLine();
            Console.WriteLine("Add fields to your tracker.");
            Console.WriteLine("Leave the field name empty when finished.");
            Console.WriteLine();

            while (true)
            {
                Console.Write($"Field {fields.Count + 1} name: ");

                string? fieldName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(fieldName))
                    break;

                if (!TryReadFieldType(out FieldType fieldType))
                {
                    Console.WriteLine("Tracker creation cancelled.");
                    Pause();
                    return;
                }

                try
                {
                    FieldDefinition field = new(fieldName, fieldType, fields.Count);

                    fields.Add(field);

                    Console.WriteLine($"Added: {field.Name} ({field.Type})");
                    Console.WriteLine();
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine($"Invalid field: {exception.Message}");
                    Console.WriteLine();
                }
            }

            if (fields.Count == 0)
            {
                Console.WriteLine("A tracker must have at least one field.");
                Pause();
                return;
            }

            try
            {
                Tracker tracker = _createTrackerUseCase.Execute(
                    trackerName,
                    fields);

                Console.WriteLine();
                Console.WriteLine($"Tracker '{tracker.Name}' created successfully!");
                Console.WriteLine($"Fields: {tracker.Fields.Count}");
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine();
                Console.WriteLine($"Could not create tracker: {exception.Message}");
            }

            Pause();
        }

        private static bool TryReadFieldType(out FieldType fieldType)
        {
            fieldType = default;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Field type:");
                Console.WriteLine("1. Checkbox");
                Console.WriteLine("2. Duration");
                Console.WriteLine("3. Text");
                Console.WriteLine("0. Cancel tracker creation");
                Console.WriteLine();
                Console.Write("Choose a type: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        fieldType = FieldType.Checkbox;
                        return true;

                    case "2":
                        fieldType = FieldType.Duration;
                        return true;

                    case "3":
                        fieldType = FieldType.Text;
                        return true;

                    case "0":
                    case null:
                        return false;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
