namespace Agrobook.Domain.Archivos.Services
{
    public class ProductorDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class ArchivoDto
    {
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Fecha { get; set; }
        public string Desc { get; set; }
    }
}
