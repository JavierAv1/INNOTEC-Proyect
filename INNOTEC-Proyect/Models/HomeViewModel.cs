using ML;

namespace INNOTEC_Proyect.Models
{
    public class HomeViewModel
    {
        public List<Departamento> MenuItems { get; set; }
        public List<Producto> Productos { get; set; }
        public ML.Producto Producto { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
