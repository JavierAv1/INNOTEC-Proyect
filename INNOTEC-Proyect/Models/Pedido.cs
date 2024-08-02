using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCompra { get; set; }
        public DateTime FechaPedido { get; set; }
        public string EstadoPedido { get; set; }

        // Navegación a la entidad Compra
        public Compra Compra { get; set; }
        // Navegación a la entidad Envio
        public List<Envio> Envios { get; set; }

        public int UsuarioId { get; set; }
    }


}
