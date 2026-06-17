namespace Fcg.Notifications.Application.ConfirmacaoCompra;

public record ConfirmacaoCompraCommand(string Email, string Nome, string NomeJogo, Guid PedidoId);
