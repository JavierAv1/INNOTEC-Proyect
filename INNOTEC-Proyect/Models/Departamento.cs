using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ML;

public class Departamento
{
    [JsonProperty("departamentoId")]
    public int IdDepartamento { get; set; }

    [JsonProperty("departamentoNombre")]
    public string Nombre { get; set; }

    [JsonProperty("categorias")]
    public List<Categorium> Categoria { get; set; }

    [JsonProperty("productos")]
    public List<Producto> Productos { get; set; }
}
