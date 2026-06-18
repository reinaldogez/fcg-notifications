using Fcg.Contracts.Events;
using Fcg.Notifications.Tests.Integration.Fixtures;
using FluentAssertions;
using MassTransit;

namespace Fcg.Notifications.Tests.Integration;

[Collection("Resiliencia")]
public class ResilienciaTests(ResilienciaFixture fixture)
{
    [Fact]
    public async Task DeveDevolverMensagemQuandoRedisIndisponivel()
    {
        string token = Guid.NewGuid().ToString("N");
        UserCreatedEvent evt = new()
        {
            UserId = Guid.NewGuid(),
            Email = $"{token}@fcg.com",
            Name = "Ana",
        };

        await fixture.Bus.Publish(
            evt,
            context => context.MessageId = Guid.NewGuid(),
            CancellationToken.None
        );

        // Janela folgada: cobre o custo de primeira publicação (bus recém-iniciado, sob carga de
        // Docker) somado ao retry curto antes do fault. O resultado em si é determinístico.
        bool faltou = await fixture.EsperarFaultAsync(TimeSpan.FromSeconds(45));

        // Redis fora ⇒ o consumer lança e o MassTransit não dá ACK (NACK/redelivery), em vez de
        // processar às cegas: nenhum efeito [EMAIL] é emitido para a mensagem.
        faltou.Should().BeTrue("o consumer deve falhar quando o Redis está indisponível");
        fixture
            .LogsComToken(token)
            .Should()
            .BeEmpty("sem Redis o handler não roda, então não há efeito de notificação");
    }
}
