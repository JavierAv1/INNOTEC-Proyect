namespace ML;

public partial class Usuario
{
    public int? UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaDeNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? UserName { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public int TipoUsuarioIdTipousuario { get; set; }

    public byte[]? FotoDePerfil { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual TipoUsuario TipoUsuarioIdTipousuarioNavigation { get; set; } = null!;
}
