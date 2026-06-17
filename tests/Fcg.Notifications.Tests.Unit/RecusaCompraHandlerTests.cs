using Fcg.Notifications.Application.RecusaCompra;
using FluentAssertions;
using Microsoft.Extensions.Logging.Testing;

namespace Fcg.Notifications.Tests.Unit;

public class RecusaCompraHandlerTests
{
    [Fact]
    public async Task DeveLogarRecusaComMotivo()
    {
        FakeLogger<EnviarRecusaHandler> logger = new();
        EnviarRecusaHandler handler = new(logger);
        var pedidoId = Guid.NewGuid();

        await handler.HandleAsync(
            new RecusaCompraCommand("ana@fcg.com", "Ana", "Hades", pedidoId, "Cartão recusado"),
            CancellationToken.None
        );

        FakeLogRecord record = logger.Collector.GetSnapshot().Single();
        record
            .Message.Should()
            .Contain("Hades")
            .And.Contain(pedidoId.ToString())
            .And.Contain("Cartão recusado");
    }
}
