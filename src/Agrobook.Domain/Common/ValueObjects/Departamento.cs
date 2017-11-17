namespace Agrobook.Domain.Common.ValueObjects
{
    public class Departamento
    {
        public Departamento(string id, string display, Distrito[] distritos)
        {
            this.Id = id;
            this.Display = display;
            this.Distritos = distritos;
        }

        public string Id { get; }
        public string Display { get; }
        public Distrito[] Distritos { get; }
    }

    public class Distrito
    {
        public Distrito(string id, string display)
        {
            this.Id = id;
            this.Display = display;
        }

        public string Id { get; }
        public string Display { get; }
    }
}
