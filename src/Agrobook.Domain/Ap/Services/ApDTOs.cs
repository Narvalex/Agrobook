﻿using System;
using System.Collections.Generic;

namespace Agrobook.Domain.Ap.Services
{
    public class ClienteDeApDto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Desc { get; set; }
        public string Tipo { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class OrgDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class OrgConContratosDto
    {
        public OrgDto Org { get; set; }
        public ContratoEntity[] Contratos { get; set; }
    }

    public class ProdDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
        public OrgDto[] Orgs { get; set; }
    }

    public class ServicioDto
    {
        public string Id { get; set; }
        public string IdContrato { get; set; }
        public bool EsAdenda { get; set; }
        public string IdContratoDeLaAdenda { get; set; }
        public string ContratoDisplay { get; set; }
        public string IdOrg { get; set; }
        public string OrgDisplay { get; set; }
        public string IdProd { get; set; }
        public string ProdDisplay { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }

        // Defaults
        public bool Eliminado { get; set; }
        // Parcela
        public string ParcelaId { get; set; }
        public string ParcelaDisplay { get; set; }
        public string Hectareas { get; set; }
        // Precio
        public bool TienePrecio { get; set; }
        public string PrecioTotal { get; set; }
        public string PrecioPorHectarea { get; set; }
    }

    public class ServicioParaDashboardDto
    {
        public string Id { get; set; }
        public string IdProd { get; set; }
        public string ProdDisplay { get; set; }
        public string ProdAvatarUrl { get; set; }
        public string OrgDisplay { get; set; }
        public string ParcelaDisplay { get; set; }
        public DateTime Fecha { get; set; }
    }

    // GROUP BY
    public class ContratoConServicios
    {
        public ContratoConServicios()
        {
            this.Servicios = new List<ServicioSlim>();
        }

        public string Id { get; set; }
        public string Display { get; set; }
        public decimal TotalHa { get; set; }
        public DateTime Fecha { get; set; }
        public bool Eliminado { get; set; }
        public ICollection<ServicioSlim> Servicios { get; set; }
    }

    public class ServicioSlim
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public bool Eliminado { get; set; }
        public decimal Hectareas { get; set; }
        public DateTime Fecha { get; set; }
        // Esto es solo para que desde la interfaz se pueda ir a la pagina del servicio
        public string IdProd { get; set; }
    }
}
