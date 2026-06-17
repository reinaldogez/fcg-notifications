using Microsoft.Extensions.Logging;

namespace Fcg.Notifications.Application.ConfirmacaoCompra;

public class EnviarConfirmacaoHandler(ILogger<EnviarConfirmacaoHandler> logger)
{
    public Task HandleAsync(ConfirmacaoCompraCommand command, CancellationToken ct)
    {
        logger.LogInformation(
            "[EMAIL] Confirmação de compra\nPara: {Email}\nOlá, {Nome}! Sua compra do jogo {NomeJogo} (pedido {PedidoId}) foi confirmada.",
            command.Email,
            command.Nome,
            command.NomeJogo,
            command.PedidoId
        );
        return Task.CompletedTask;
    }
}
