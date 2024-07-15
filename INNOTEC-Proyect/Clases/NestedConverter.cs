using ML;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace INNOTEC_Proyect.Clases
{
    class NestedConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Departamento) || objectType == typeof(Categorium) || objectType == typeof(Subcategorium) || objectType == typeof(Producto);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            if (objectType == typeof(Producto))
            {
                int productId = item["idProductos"]?.ToObject<int>() ?? 0;
                string productName = item["nombre"]?.ToString() ?? "Nombre no disponible";
                string productDescription = item["descripcionDelProducto"]?.ToString();
                int productPrice = item["precio"]?.ToObject<int>() ?? 0;
                int? productQuantity = item["cantidad"]?.ToObject<int?>();
                byte[] productImage = item["imagenDelProducto"]?.ToObject<byte[]>();

                return new Producto
                {
                    IdProductos = productId,
                    Nombre = productName,
                    DescripcionDelProducto = productDescription,
                    Precio = productPrice,
                    Cantidad = productQuantity,
                    ImagenDelProducto = productImage,
                };
            }
            else if (objectType == typeof(Departamento))
            {
                return new Departamento
                {
                    IdDepartamento = item["departamentoId"].ToObject<int>(),
                    Nombre = item["departamentoNombre"].ToString(),
                    Productos = item["productos"].Select(p => DeserializeProducto(p)).ToList(),
                    Categoria = item["categorias"].ToObject<List<Categorium>>(serializer)
                };
            }
            else if (objectType == typeof(Categorium))
            {
                return new Categorium
                {
                    IdCategoria = item["categoriaId"].ToObject<int>(),
                    Nombre = item["categoriaNombre"].ToString(),
                    Productos = item["productos"].Select(p => DeserializeProducto(p)).ToList(),
                    Subcategoria = item["subcategorias"].ToObject<List<Subcategorium>>(serializer)
                };
            }
            else if (objectType == typeof(Subcategorium))
            {
                return new Subcategorium
                {
                    IdSubcategoria = item["subcategoriaId"].ToObject<int>(),
                    Nombre = item["subcategoriaNombre"].ToString(),
                    Productos = item["productos"].Select(p => DeserializeProducto(p)).ToList()
                };
            }

            return null;


            throw new NotImplementedException("Unrecognized type");
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unneeded because we only read the JSON");
        }

        private Producto DeserializeProducto(JToken item)
        {
            return new Producto
            {
                IdProductos = item["productoId"]?.ToObject<int>() ?? 0,
                Nombre = item["nombre"]?.ToString() ?? "Nombre no disponible",
            };
        }
    }


}

