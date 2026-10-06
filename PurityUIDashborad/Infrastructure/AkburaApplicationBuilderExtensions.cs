using Akbura.Engine;
using Avalonia;


namespace PurityUIDashborad.Infrastructure;

public static class AkburaApplicationBuilderExtensions
{
    public static AppBuilder UseAkburaApplication(this AppBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);


        builder = builder.UseAkbura(akbura =>
        {
            // The designer needs Akbura, but should not instantiate app services.
            if (!Avalonia.Controls.Design.IsDesignMode)
            {
                akbura.WithServiceProvider(AppServices.Current.ServiceProvider);
            }
        });

        return builder.WithInterFont();
    }
}
