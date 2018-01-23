using System;
using System.Collections.Generic;

namespace Agrobook.Domain.DataWarehousing.DAOs.DTOs
{
    public class ServicioDeAp
    {
        public string Organizacion { get; set; }
        public int Año { get; set; }
        public string Contrato { get; set; }
        public string Productor { get; set; }
        public string Parcela { get; set; }
        public decimal Ha { get; set; }
        public decimal? PrecioPorHa { get; set; }
        public decimal? PrecioTotal { get; set; }
        public DateTime Fecha { get; set; }
    }
}
