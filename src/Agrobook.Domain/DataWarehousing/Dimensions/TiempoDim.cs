﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

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
    }

    public class TiempoDimMap : EntityTypeConfiguration<TiempoDim>
    {
        public TiempoDimMap()
        {
            this.HasKey(e => e.Sid);

            this.Property(x => x.IdTiempo)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            this.ToTable("TiempoDims");
        }
    }
}
