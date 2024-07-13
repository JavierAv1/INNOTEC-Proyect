using System;
using System.Collections.Generic;

namespace ML;

public partial class Compra
{
    public int IdCompra { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public DateOnly? FechaDeCompra { get; set; }

    public int? Idusuario { get; set; }

    public int? Idproducto { get; set; }

    public int? Cantidad { get; set; }

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual Producto? IdproductoNavigation { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }
}
