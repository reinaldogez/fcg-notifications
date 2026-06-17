namespace Fcg.Notifications.Application.RecusaCompra;

public record RecusaCompraCommand(
    string Email,
    string Nome,
    string NomeJogo,
    Guid PedidoId,
    string Motivo
);
