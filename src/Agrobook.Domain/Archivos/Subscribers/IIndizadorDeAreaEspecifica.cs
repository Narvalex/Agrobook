namespace Agrobook.Domain.Archivos.Subscribers
{
    /// <summary>
    /// Representa a un indizador de un area especifica, como el del grupo de contratos
    /// </summary>
    public interface IIndizadorDeAreaEspecifica
    {
        AgrobookDbContext AgregarAlIndice(AgrobookDbContext context, string idColeccion);
        AgrobookDbContext EliminarDelIndice(AgrobookDbContext context, string idColeccion);
        AgrobookDbContext RestaurarEnElIndice(AgrobookDbContext context, string idColeccion);
    }
}
