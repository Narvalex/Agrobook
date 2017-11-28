using Agrobook.Domain.DataWarehousing.Dimensions;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Facts
{
    // Relantionships in EF: https://msdn.microsoft.com/en-us/library/jj591620(v=vs.113).aspx
    public class ServicioDeApFact
    {
        public int Sid { get; set; }

        public string IdServicio { get; set; }

        public virtual TiempoDim DimTiempo { get; set; }
        public virtual ContratoDim DimContrato { get; set; }
        public virtual ProductorDim DimProductor { get; set; }
        public virtual ParcelaDim DimParcela { get; set; }
        public virtual PrecioPorHaServicioApDim DimPrecioPorHaServicioAp { get; set; }
        public virtual DepartamentoDim DimDepartamento { get; set; }

        public decimal PrecioTotal { get; set; }
    }

    public class ServicioDeApFactMap : EntityTypeConfiguration<ServicioDeApFact>
    {
        public ServicioDeApFactMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("ServicioDeApFacts");

            this.HasOptional(e => e.DimTiempo);
            this.HasOptional(e => e.DimContrato);
            this.HasOptional(e => e.DimProductor);
            this.HasOptional(e => e.DimParcela);
            this.HasOptional(e => e.DimPrecioPorHaServicioAp);
            this.HasOptional(e => e.DimDepartamento);
        }
    }
}
