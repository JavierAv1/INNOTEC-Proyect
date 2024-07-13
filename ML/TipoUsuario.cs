using System;
using System.Collections.Generic;

namespace DL;

public partial class TipoUsuario
{
    public int IdTipousuario { get; set; }

    public string? TipoUsuario1 { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
