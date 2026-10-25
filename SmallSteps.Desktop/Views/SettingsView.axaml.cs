// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

using System;
using Avalonia.Controls;

namespace SmallSteps.Desktop.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private AppPalette _currentPalette = AppPalette.Pink;

    private void TogglePalette_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AppPalette[] appPalettes = Enum.GetValues<AppPalette>();

        int currentIndex = Array.IndexOf(appPalettes, _currentPalette);

        int nextIndex = (currentIndex + 1) % appPalettes.Length;

        _currentPalette = appPalettes[nextIndex];

        PaletteManager.SetPalette(_currentPalette);
    }
}
