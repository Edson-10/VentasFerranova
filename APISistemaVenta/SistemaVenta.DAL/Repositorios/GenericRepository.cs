using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.DBContex;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SistemaVenta.DAL.Repositorios
{
    public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
    {
        private readonly DbventaContext _dbcontex;

        public GenericRepository(DbventaContext dbcontex)
        {
            _dbcontex = dbcontex;
        }

        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                TModelo modelo = await _dbcontex.Set<TModelo>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch {
                throw;
            }

        }

        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _dbcontex.Set<TModelo>().Add(modelo);
                await _dbcontex.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _dbcontex.Set<TModelo>().Update(modelo);
                await _dbcontex.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbcontex.Set<TModelo>().Remove(modelo);
                await _dbcontex.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> queryModelo = filtro == null ?_dbcontex.Set<TModelo>():_dbcontex.Set<TModelo>().Where(filtro);
                return queryModelo;
            }
            catch
            {
                throw;
            }
        }

        public Task Consultar(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
