﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class DepartamentoDim
    {
        public int Sid { get; set; }
        public string IdDepartamento { get; set; }
        public string Nombre { get; set; }
    }

    public class DepartamentoDimMap : EntityTypeConfiguration<DepartamentoDim>
    {
        public DepartamentoDimMap()
        {
            this.HasKey(e => e.Sid);

            this.Property(x => x.IdDepartamento)
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            this.ToTable("DepartamentoDims");
        }
    }
}
