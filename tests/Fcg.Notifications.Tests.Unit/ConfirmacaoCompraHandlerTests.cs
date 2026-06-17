using Fcg.Notifications.Application.ConfirmacaoCompra;
using FluentAssertions;
using Microsoft.Extensions.Logging.Testing;

namespace Fcg.Notifications.Tests.Unit;

public class ConfirmacaoCompraHandlerTests
{
    [Fact]
    public async Task DeveLogarConfirmacaoComNomeDoJogoEPedido()
    {
        FakeLogger<EnviarConfirmacaoHandler> logger = new();
        EnviarConfirmacaoHandler handler = new(logger);
        var pedidoId = Guid.NewGuid();

        await handler.HandleAsync(
            new ConfirmacaoCompraCommand("ana@fcg.com", "Ana", "Hades", pedidoId),
            CancellationToken.None
        );

        FakeLogRecord record = logger.Collector.GetSnapshot().Single();
        record.Message.Should().Contain("Hades").And.Contain(pedidoId.ToString());
    }
}
