﻿namespace ML;

public partial class TipoUsuario
{
    public int IdTipousuario { get; set; }

    public string? TipoUsuario1 { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
