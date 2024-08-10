using Newtonsoft.Json;

namespace ML;

public partial class Subcategorium
{
    [JsonProperty("subcategoriaId")]
    public int IdSubcategoria { get; set; }

    [JsonProperty("subcategoriaNombre")]
    public string? Nombre { get; set; }

    [JsonProperty("descripcion")]
    public string? Descripcion { get; set; }

    [JsonIgnore]  // Ignorar si no viene del JSON
    public int IdCategoria { get; set; }

    [JsonIgnore]  // Ignorar si no viene del JSON
    public Categorium? IdCategoriaNavigation { get; set; }

    [JsonProperty("productos")]
    public List<Producto> Productos { get; set; } = new List<Producto>();
}
