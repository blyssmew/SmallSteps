// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.
using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;

namespace SmallSteps.Desktop;

public enum AppPalette
{
    Blue,
    Pink,
    Beige,
    Purple
}

public static class PaletteManager
{
    private static ResourceInclude? _activePalette;

    private static readonly Dictionary<AppPalette, Uri> Palettes = new()
    {
        [AppPalette.Blue] = new("avares://SmallSteps.Desktop/Styles/Themes/Blue.axaml"),
        [AppPalette.Pink] = new("avares://SmallSteps.Desktop/Styles/Themes/Pink.axaml"),
        [AppPalette.Beige] = new("avares://SmallSteps.Desktop/Styles/Themes/Beige.axaml"),
        [AppPalette.Purple] = new("avares://SmallSteps.Desktop/Styles/Themes/Purple.axaml")
    };

    public static void SetPalette(Avalonia.Application app, AppPalette palette)
    {
        if (!Palettes.TryGetValue(palette, out var source)) throw new ArgumentOutOfRangeException(nameof(palette));

        if (_activePalette is not null) app.Resources.MergedDictionaries.Remove(_activePalette);

        _activePalette = new ResourceInclude(source)
        {
            Source = source
        };

        app.Resources.MergedDictionaries.Add(_activePalette);
    }

    public static void SetPalette(AppPalette palette)
    {
        var app = Avalonia.Application.Current ?? throw new InvalidOperationException("The application has not been initialized.");

        SetPalette(app, palette);
    }
}
