using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.DBContex;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {

        private readonly DbventaContext _dbContext;

        public VentaRepository(DbventaContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var trasaction = _dbContext.Database.BeginTransaction())
            {
                try {

                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {

                        Producto producto_econtrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();

                        producto_econtrado.Stock = producto_econtrado.Stock - dv.Cantidad;
                        _dbContext.Productos.Update(producto_econtrado);
                    }
                    await _dbContext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbContext.NumeroDocumentos.First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    _dbContext.NumeroDocumentos.Update(correlativo);
                    await _dbContext.SaveChangesAsync();

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0",CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    //00001
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos);

                    modelo.NumeroDocumento = numeroVenta;

                    await _dbContext.Venta.AddRangeAsync(modelo) ;
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    trasaction.Commit();



                    

                }catch {

                    trasaction.Rollback();
                    throw;

                }

                return ventaGenerada;

            }

            


        }
    }
}
