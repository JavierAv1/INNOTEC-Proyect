using System;
using System.Collections.Generic;

namespace DL;

public partial class Subcategorium
{
    public int IdSubcategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
