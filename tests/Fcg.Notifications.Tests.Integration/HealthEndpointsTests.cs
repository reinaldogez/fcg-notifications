using System.Net;
using Fcg.Notifications.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fcg.Notifications.Tests.Integration;

[Collection("Health")]
public class HealthEndpointsTests(HealthFixture fixture)
{
    // Endereço de Redis inalcançável: o check fica Unhealthy sem travar nem demorar.
    private const string RedisMorto =
        "localhost:6390,abortConnect=false,connectTimeout=500,connectRetry=0";

    [Fact]
    public async Task DeveReportarLiveSempre()
    {
        using NotificationsApiFactory factory = new(RedisMorto);
        using HttpClient client = factory.CreateClient();

        HttpResponseMessage resposta = await client.GetAsync("/health/live");

        // O live é só "processo vivo": não checa dependência, então passa mesmo sem Redis.
        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeveReportarReadySaudavelIgnorandoBrokerForaDoAr()
    {
        // Redis acessível, broker apontado para endereço inerte: o ready ignora o broker.
        using NotificationsApiFactory factory = new(fixture.RedisConnection);
        using HttpClient client = factory.CreateClient();

        HttpResponseMessage resposta = await client.GetAsync("/health/ready");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeveDegradarReadyQuandoRedisInacessivel()
    {
        using NotificationsApiFactory factory = new(RedisMorto);
        using HttpClient client = factory.CreateClient();

        HttpResponseMessage ready = await client.GetAsync("/health/ready");
        HttpResponseMessage live = await client.GetAsync("/health/live");

        // Redis é dependência dura: sem ele o ready degrada, mas o processo segue vivo.
        ready.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        live.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
