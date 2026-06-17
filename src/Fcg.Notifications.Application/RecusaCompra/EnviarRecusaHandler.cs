using Microsoft.Extensions.Logging;

namespace Fcg.Notifications.Application.RecusaCompra;

public class EnviarRecusaHandler(ILogger<EnviarRecusaHandler> logger)
{
    public Task HandleAsync(RecusaCompraCommand command, CancellationToken ct)
    {
        logger.LogInformation(
            "[EMAIL] Recusa de compra\nPara: {Email}\nOlá, {Nome}! Sua compra do jogo {NomeJogo} (pedido {PedidoId}) foi recusada.\nMotivo: {Motivo}",
            command.Email,
            command.Nome,
            command.NomeJogo,
            command.PedidoId,
            command.Motivo
        );
        return Task.CompletedTask;
    }
}
