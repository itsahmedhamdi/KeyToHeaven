﻿using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Hosting;
using Microcharts.Maui;

namespace KeyToHeaven
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}
