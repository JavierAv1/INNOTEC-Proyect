namespace ML;

public partial class Envio
{
    public int? IdEnvio { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Estado { get; set; }
    public string? Calle { get; set; }
    public string? Colonia { get; set; }
    public string? Municipio { get; set; }
    public int? Numero { get; set; }
    public int? IdCompra { get; set; }
    public int? UsuarioId { get; set; }
    public int? Status { get; set; }
    public int? IdPedido { get; set; } // Llave foránea para Pedido
    public Pedido? Pedido { get; set; } // Propiedad de navegación para Pedido
    public virtual Compra? IdCompraNavigation { get; set; } // Propiedad de navegación para Compra

    public List<Envio> Envios { get; set; }
}
