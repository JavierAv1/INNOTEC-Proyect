using Newtonsoft.Json;

namespace ML
{
    public class Producto
    {
        [JsonProperty("idProductos")]
        public int? IdProductos { get; set; }

        [JsonProperty("nombre")]
        public string? Nombre { get; set; }

        [JsonProperty("descripcionDelProducto")]
        public string? DescripcionDelProducto { get; set; }

        [JsonProperty("precio")]
        public int? Precio { get; set; }

        [JsonProperty("cantidad")]
        public int? Cantidad { get; set; }

        [JsonProperty("imagenDelProducto")]
        public byte[]? ImagenDelProducto { get; set; }

        [JsonProperty("idDepartamento")]
        public int? IdDepartamento { get; set; }

        [JsonProperty("idCategoria")]
        public int? IdCategoria { get; set; }

        [JsonProperty("idSubcategoria")]
        public int? IdSubcategoria { get; set; }

        [JsonProperty("idProveedor")]
        public int? IdProveedor { get; set; }

        // Propiedades de navegación opcionales
        public Departamento? Departamento { get; set; }
        public Categorium? Categoria { get; set; }
        public Subcategorium? Subcategoria { get; set; }
        public Proveedor? Proveedor { get; set; }

        ////Extras
        public List<Producto>? Productos { get; set; }
        public Categorium? Categorium { get; set; }
        public Subcategorium? Subcategorium { get; set; }

    }
}
