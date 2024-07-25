using ML;

namespace INNOTEC_Proyect.Models
{
    public class HomeViewModel
    {
        public List<Departamento> MenuItems { get; set; }
        public List<Categorium> Categorias { get; set; }
        public List<Subcategorium> Subcategorias { get; set; }
        public List<Producto> Productos { get; set; }
        public List<Proveedor> Proveedores { get; set; }
        public ML.Producto Producto { get; set; }
        public string ActiveTab { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
