using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class SesionDTO  //DTO QUE GUARDA LA SESION DEL USUARIO QUE SE HA LOGEADO
    {
        public int IdUsuario { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Correo { get; set; }

       

        public string? RolDescripcion { get; set; }
    }
}
