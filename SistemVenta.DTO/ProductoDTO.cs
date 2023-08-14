using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }

        public string? Nombre { get; set; }

        public int? IdCategoria { get; set; }

        public string? DescripcionCategoria { get; set; }

        public int? Stock { get; set; }

        public string? Precio { get; set; }//precio manejado como string,recibimos desde la interfa ocmo string pero pafra registrarlo nos aseguraremos de cambiar el tipo

        public int EsActivo { get; set; }
    }
}
