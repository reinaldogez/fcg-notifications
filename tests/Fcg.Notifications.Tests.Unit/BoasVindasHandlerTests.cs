using Fcg.Notifications.Application.BoasVindas;
using Fcg.Notifications.Application.ConfirmacaoCompra;
using Fcg.Notifications.Application.RecusaCompra;
using FluentAssertions;
using Microsoft.Extensions.Logging.Testing;

namespace Fcg.Notifications.Tests.Unit;

public class BoasVindasHandlerTests
{
    [Fact]
    public async Task DeveLogarEmailDeBoasVindasComNomeEEmail()
    {
        FakeLogger<EnviarBoasVindasHandler> logger = new();
        EnviarBoasVindasHandler handler = new(logger);

        await handler.HandleAsync(
            new BoasVindasCommand("ana@fcg.com", "Ana"),
            CancellationToken.None
        );

        FakeLogRecord record = logger.Collector.GetSnapshot().Single();
        record.Message.Should().Contain("Ana").And.Contain("ana@fcg.com");
    }

    [Fact]
    public async Task DeveIncluirPrefixoEmailEmTodasAsMensagens()
    {
        FakeLogger<EnviarBoasVindasHandler> boasVindasLogger = new();
        FakeLogger<EnviarConfirmacaoHandler> confirmacaoLogger = new();
        FakeLogger<EnviarRecusaHandler> recusaLogger = new();
        var pedidoId = Guid.NewGuid();

        await new EnviarBoasVindasHandler(boasVindasLogger).HandleAsync(
            new BoasVindasCommand("ana@fcg.com", "Ana"),
            CancellationToken.None
        );
        await new EnviarConfirmacaoHandler(confirmacaoLogger).HandleAsync(
            new ConfirmacaoCompraCommand("ana@fcg.com", "Ana", "Hades", pedidoId),
            CancellationToken.None
        );
        await new EnviarRecusaHandler(recusaLogger).HandleAsync(
            new RecusaCompraCommand("ana@fcg.com", "Ana", "Hades", pedidoId, "Cartão recusado"),
            CancellationToken.None
        );

        boasVindasLogger.Collector.GetSnapshot().Single().Message.Should().StartWith("[EMAIL]");
        confirmacaoLogger.Collector.GetSnapshot().Single().Message.Should().StartWith("[EMAIL]");
        recusaLogger.Collector.GetSnapshot().Single().Message.Should().StartWith("[EMAIL]");
    }
}
