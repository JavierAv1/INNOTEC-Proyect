using System;
using System.Collections.Generic;

namespace DL;

public partial class Producto
{
    public int IdProductos { get; set; }

    public string Nombre { get; set; } = null!;

    public string? DescripcionDelProducto { get; set; }

    public int Precio { get; set; }

    public int? Cantidad { get; set; }

    public byte[]? ImagenDelProducto { get; set; }

    public int IdProveedor { get; set; }

    public int IdDepartamento { get; set; }

    public int? IdCategoria { get; set; }

    public int? IdSubcategoria { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    public virtual Subcategorium? IdSubcategoriaNavigation { get; set; }
}
