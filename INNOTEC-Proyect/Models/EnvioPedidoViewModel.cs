using ML;

namespace INNOTEC_Proyect.Models
{
    public class EnvioPedidoViewModel
    {
        public Envio Envio { get; set; }
        public int IdCompra { get; set; }
        public decimal Total { get; set; }
        public Pedido Pedido { get; set; }

        public List<Envio> Envios { get; set; }
        public List<Pedido> Pedidos { get; set; } 
    }
}
