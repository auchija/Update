using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using update.Domain.Interfaces;
using update.Domain.Exceptions;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Acceso a datos genérico; no entrega IQueryable fuera de Infrastructure.</summary>
public class GenericRepository<T>(ApplicationDbContext contexto) : IRepository<T> where T : class, IEntidadValidable
{
    protected readonly ApplicationDbContext Contexto = contexto;
    protected DbSet<T> Conjunto => Contexto.Set<T>();

    public async Task<IReadOnlyList<T>> ListarAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
        => await BuscarAsync(_ => true, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyList<T>> BuscarAsync(Expression<Func<T, bool>> criterio, int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        if (pagina < 1 || pagina > 100000 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ExcepcionDominio("La página debe estar entre 1 y 100000; el tamaño, entre 1 y 100.");
        IQueryable<T> consulta = Conjunto.AsNoTracking().Where(criterio);
        var clave = Contexto.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!;
        // Orden determinista por la PK completa; evita paginación sin ORDER BY.
        var primera = true;
        foreach (var propiedad in clave.Properties)
        {
            var parametro = Expression.Parameter(typeof(T), "entidad");
            var acceso = Expression.Property(parametro, propiedad.Name);
            var selector = Expression.Lambda(acceso, parametro);
            var metodo = primera ? "OrderBy" : "ThenBy";
            var expresion = Expression.Call(typeof(Queryable), metodo, [typeof(T), propiedad.ClrType], consulta.Expression, Expression.Quote(selector));
            consulta = consulta.Provider.CreateQuery<T>(expresion);
            primera = false;
        }
        return await consulta.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToListAsync(cancellationToken);
    }

    public async Task<T?> ObtenerPorClaveAsync(object[] claves, CancellationToken cancellationToken = default)
    {
        var propiedades = Contexto.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties;
        if (claves.Length != propiedades.Count) throw new ExcepcionDominio("La clave tiene un número incorrecto de componentes.");
        var parametro = Expression.Parameter(typeof(T), "entidad");
        Expression? condicion = null;
        for (var i = 0; i < propiedades.Count; i++)
        {
            var propiedad = propiedades[i];
            if (!propiedad.ClrType.IsInstanceOfType(claves[i])) throw new ExcepcionDominio("El tipo de la clave es incorrecto.");
            var igualdad = Expression.Equal(Expression.Property(parametro, propiedad.Name), Expression.Constant(claves[i], propiedad.ClrType));
            condicion = condicion is null ? igualdad : Expression.AndAlso(condicion, igualdad);
        }
        var criterio = Expression.Lambda<Func<T, bool>>(condicion!, parametro);
        // FindAsync puede devolver entidades rastreadas saltándose el filtro de borrado lógico.
        return await Conjunto.FirstOrDefaultAsync(criterio, cancellationToken);
    }

    public async Task AgregarAsync(T entidad, CancellationToken cancellationToken = default)
    {
        entidad.Validar();
        await Conjunto.AddAsync(entidad, cancellationToken);
        await Contexto.SaveChangesAsync(cancellationToken);
    }
    public async Task ActualizarAsync(T entidad, CancellationToken cancellationToken = default)
    {
        entidad.Validar();
        Contexto.Entry(entidad).State = EntityState.Modified;
        await Contexto.SaveChangesAsync(cancellationToken);
    }
    public async Task EliminarAsync(T entidad, CancellationToken cancellationToken = default)
    {
        Conjunto.Remove(entidad);
        await Contexto.SaveChangesAsync(cancellationToken);
    }
}
