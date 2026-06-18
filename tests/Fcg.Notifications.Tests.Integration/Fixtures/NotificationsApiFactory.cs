using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Fcg.Notifications.Tests.Integration.Fixtures;

// Sobe o host real do Api (Program) em memória. A config é injetada por variável de ambiente
// porque o Program lê Redis:Connection/RabbitMq:* na composição, antes de Build() — só as fontes
// que o CreateBuilder já inclui (env vars) chegam a essa leitura. O broker aponta para um endereço
// inerte de propósito: prova que o /health/ready não depende dele.
public sealed class NotificationsApiFactory : WebApplicationFactory<Program>
{
    public NotificationsApiFactory(string redisConnection)
    {
        Environment.SetEnvironmentVariable("Redis__Connection", redisConnection);
        Environment.SetEnvironmentVariable("RabbitMq__Host", "localhost");
        Environment.SetEnvironmentVariable("RabbitMq__Username", "guest");
        Environment.SetEnvironmentVariable("RabbitMq__Password", "guest");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.UseEnvironment("Testing");

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }

        Environment.SetEnvironmentVariable("Redis__Connection", null);
        Environment.SetEnvironmentVariable("RabbitMq__Host", null);
        Environment.SetEnvironmentVariable("RabbitMq__Username", null);
        Environment.SetEnvironmentVariable("RabbitMq__Password", null);
    }
}
