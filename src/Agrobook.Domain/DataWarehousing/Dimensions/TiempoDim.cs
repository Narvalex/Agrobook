using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class TiempoDim
    {
        public int Sid { get; set; }
        public string IdTiempo { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public int Hora { get; set; }
        public int Minuto { get; set; }

        public static string GetIdTiempoFromDateTime(DateTime dateTime) => dateTime.ToString("DDMMYYhhmm");

        public static TiempoDim New(DateTime dateTime)
            => new TiempoDim
            {
                IdTiempo = GetIdTiempoFromDateTime(dateTime),
                Año = dateTime.Year,
                Mes = dateTime.Month,
                Dia = dateTime.Day,
                Hora = dateTime.Hour,
                Minuto = dateTime.Minute
            };

        public static TiempoDim GetOrAddTiempo(DateTime fecha, IDbSet<TiempoDim> dbSet)
        {
            var idTiempo = GetIdTiempoFromDateTime(fecha);
            var tiempo = dbSet.SingleOrDefault(x => x.IdTiempo == idTiempo);
            if (tiempo == null)
            {
                tiempo = New(fecha);
                dbSet.Add(tiempo);
            }

            return tiempo;
        }
    }

    public class TiempoDimMap : EntityTypeConfiguration<TiempoDim>
    {
        public TiempoDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("TiempoDims");
        }
    }
}
