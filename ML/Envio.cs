using System;
using System.Collections.Generic;

namespace DL;

public partial class Envio
{
    public int IdEnvio { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Estado { get; set; }

    public string? Calle { get; set; }

    public string? Colonia { get; set; }

    public string? Municipio { get; set; }

    public int? Numero { get; set; }

    public int? IdCompra { get; set; }

    public int? Status { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }
}
