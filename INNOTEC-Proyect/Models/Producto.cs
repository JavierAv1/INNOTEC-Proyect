using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ML;

public class Producto
{
    [JsonProperty("idProductos")]
    public int IdProductos { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("descripcionDelProducto")]
    public string DescripcionDelProducto { get; set; }

    [JsonProperty("precio")]
    public int Precio { get; set; }

    [JsonProperty("cantidad")]
    public int? Cantidad { get; set; }

    [JsonProperty("imagenDelProducto")]
    public byte[] ImagenDelProducto { get; set; }

    // Propiedades de navegación
    [JsonProperty("idDepartamento")]
    public Departamento Departamento { get; set; }

    [JsonProperty("idCategoria")]
    public Categorium Categoria { get; set; }

    [JsonProperty("idSubcategoria")]
    public Subcategorium Subcategoria { get; set; }

    [JsonProperty("idProveedor")]
    public Proveedor Proveedor { get; set; }

    //Extras
    public List<Producto> Productos { get; set; }
    public Categorium Categorium { get; set; }
    public Subcategorium Subcategorium { get; set; }
}
