using Fcg.Notifications.Application.BoasVindas;
using Fcg.Notifications.Application.ConfirmacaoCompra;
using Fcg.Notifications.Application.RecusaCompra;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.Notifications.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<EnviarBoasVindasHandler>();
        services.AddTransient<EnviarConfirmacaoHandler>();
        services.AddTransient<EnviarRecusaHandler>();
        return services;
    }
}
