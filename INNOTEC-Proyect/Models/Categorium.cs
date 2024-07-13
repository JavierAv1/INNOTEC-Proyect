using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ML;

public partial class Categorium
{
    [JsonProperty("categoriaId")]
    public int IdCategoria { get; set; }

    [JsonProperty("categoriaNombre")]
    public string? Nombre { get; set; }

    [JsonProperty("descripcion")]
    public string? Descripcion { get; set; }

    [JsonIgnore]  // Ignorar si no viene del JSON
    public int IdDepartamento { get; set; }

    [JsonIgnore]  // Ignorar si no viene del JSON
    public Departamento? IdDepartamentoNavigation { get; set; }

    [JsonProperty("productos")]
    public List<Producto> Productos { get; set; } = new List<Producto>();

    [JsonProperty("subcategorias")]
    public List<Subcategorium> Subcategoria { get; set; } = new List<Subcategorium>();
}
