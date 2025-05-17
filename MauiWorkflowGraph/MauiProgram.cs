using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace MauiWorkflowGraph
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .RegisterServices()
                .RegisterViewModels()
                .RegisterViews()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            //mauiAppBuilder.Services.AddTransient<SearchPageWithHandlerViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {

            //mauiAppBuilder.Services.AddTransient<WorkflowGraphPageViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            //mauiAppBuilder.Services.AddTransient<WorkflowGraphPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterHandlers(this MauiAppBuilder mauiAppBuilder)
        {
           // mauiAppBuilder.UseMauiCommunityToolkitMarkup();
            return mauiAppBuilder;
        }

    }
}
