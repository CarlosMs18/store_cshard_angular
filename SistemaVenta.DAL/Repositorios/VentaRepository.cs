using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta> , IVentaRepository
    {

        private readonly DbventaContext _dbcontext;

        public VentaRepository(DbventaContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
           Venta ventaGenerada = new Venta();

            using (var transaction = _dbcontext.Database.BeginTransaction()) //empezandoa  declarara la
 //transaccion si ocurre algo que regrese todo a como estaba antes ya que al insertar una venta afectaremos otras columnas
            {
                try
                {  //restaremos stock
                    foreach (DetalleVenta dv in modelo.DetalleVenta) 
                    {
                        Producto producto_encontrado = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _dbcontext.Productos.Update(producto_encontrado);
                    }

                    await _dbcontext.SaveChangesAsync();

                    //ahora generaremos un numero de documento
                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    //AHORA FORMATO DEL NUMERO DE CUENTA

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));  //especificar cuantos 0 tendra;
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);

                    modelo.NumeroDocumento = numeroVenta;
                    await _dbcontext.Venta.AddAsync(modelo);
                    await _dbcontext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit(); //todo OK la transaccion que se realize
                }
                catch
                {
                    transaction.Rollback();//reestablece todo si es que hubo un error todo lo vuelve a comoe staba antes de todo
                    throw;
                }
                return ventaGenerada;

            }
        }
    }
}
