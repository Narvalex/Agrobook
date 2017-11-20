using Agrobook.Domain.Common.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Common.Services
{
    // Basado en esta lista: https://es.wikipedia.org/wiki/Anexo:Municipios_de_Paraguay
    public static class DepartamentosDelParaguayProvider
    {
        private static DistritoComparer distritoComparer = new DistritoComparer();

        public static ICollection<Departamento> GetDepartamentos() => departamentos;

        public static bool UbicacionEsValida(UbicacionDepartamental ubicacion)
        {
            if (ubicacion == null) return false;
            var dto = departamentos.SingleOrDefault(x => x.Id == ubicacion.IdDepartamento);
            if (dto == null) return false;
            return dto.Distritos.Any(x => x.Id == ubicacion.IdDistrito);
        }

        private static readonly HashSet<Departamento> departamentos = new HashSet<Departamento>(new DepartamentoComparer())
        {
            new Departamento("distritoCapital", "Distrito Capital", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("asuncion", "Asunción")
            }
            .ToArray()),
            new Departamento("altoParaguay", "Alto Paraguay", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("bahiaNegra", "Bahía Negra"),
                new Distrito("carmeloPeralta", "Carmelo Peralta"),
                new Distrito("puertoCasado", "Puerto Casado"),
                new Distrito("fuerteOlimpo", "Fuerte Olimpo")
            }
            .ToArray()),
            new Departamento("altoParana", "Alto Paraná", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("ciudadDelEste", "Ciudad del Este"),
                new Distrito("doctorJuanLeonMallorquin", "Doctor Juan León Mallorquín"),
                new Distrito("doctorRaulPena", "Roctor Raúl Peña"),
                new Distrito("domingoMartinezDeIrala", "Domingo Martínez de Irala"),
                new Distrito("hernandarias", "Hernandarias"),
                new Distrito("iruna", "Iruña"),
                new Distrito("itakyry", "Itakyry"),
                new Distrito("juanEmilioOleary", "Juan Emilio O'Leary"),
                new Distrito("losCedrales", "Los Cedrales"),
                new Distrito("mbaracayu", "Mbaracayú"),
                new Distrito("mingaGuazu", "Minga Guazú"),
                new Distrito("mingaPora", "Minga Porá"),
                new Distrito("naranjal", "Naranjal"),
                new Distrito("nacunday", "Ñacunday"),
                new Distrito("presidenteFranco", "Presidente Franco"),
                new Distrito("sanAlberto", "San Alberto"),
                new Distrito("sanCristobal", "San Cristóbal"),
                new Distrito("santaFeDelParana", "Santa Fe del Paraná"),
                new Distrito("santaRita", "Santa Rita"),
                new Distrito("santaRosaDelMonday", "Santa Rosa del Monday"),
                new Distrito("tavapy", "Tavapy"),
                new Distrito("yguazu", "Yguazú"),
            }
            .ToArray()),
            new Departamento("amambay", "Amambay", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("bellaVista", "Bella Vista"),
                new Distrito("capitanBado", "Capitán Bado"),
                new Distrito("pedroJuanCaballero", "Pedro Juan Caballero"),
                new Distrito("zanjaPyta", "Zanja Pytá"),
                new Distrito("karapai", "Karapaí")
            }
            .ToArray()),
            new Departamento("boqueron", "Boquerón", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("filadelfia", "Filadelfia"),
                new Distrito("lomaPlata", "Loma Plata"),
                new Distrito("mariscalEstigarribia", "Mariscal Estigarribia")
            }
            .ToArray()),
            new Departamento("caaguazu", "Caaguazú", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("caaguazu", "Caaguazú"),
                new Distrito("carayao", "Carayaó"),
                new Distrito("coronelOviedo", "Coronel Oviedo"),
                new Distrito("doctorCecilioBaez", "Doctor Cecilio Báez"),
                new Distrito("doctorJEulogioEstigarribia", "Doctor J. Eulogio Estigarribia"),
                new Distrito("doctorJuanManuelFrutos", "Doctor Juan Manuel Frutos"),
                new Distrito("joseDomingoOcampos", "José Domingo Ocampos"),
                new Distrito("laPastora", "La Pastora"),
                new Distrito("mcalFranciscoSLopez", "Mcal. Francisco S. López"),
                new Distrito("nuevaLondres", "Nueva Londres"),
                new Distrito("nuevaToledo", "Nueva Toledo"),
                new Distrito("raulArsenioOviedo", "Raúl Arsenio Oviedo"),
                new Distrito("repatriacion", "Repatriación"),
                new Distrito("riTresCorrales", "R. I. Tres Corrales"),
                new Distrito("sanJoaquin", "San Joaquín"),
                new Distrito("sanJoseDeLosArroyos", "San José de los Arroyos"),
                new Distrito("santaRosaDelMutuy", "Santa Rosa del Mbutuy"),
                new Distrito("simonBolivar", "Simón Bolívar"),
                new Distrito("tempbiapora", "Tembiaporá"),
                new Distrito("tresDeFebrero", "Tres de Febrero"),
                new Distrito("vaqueria", "Vaquería"),
                new Distrito("yhu", "Yhú"),
            }
            .ToArray()),
            new Departamento("caazapa", "Caazapá", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("3deMayo", "3 de Mayo"),
                new Distrito("abai", "Abaí"),
                new Distrito("buenaVista", "Buena Vista"),
                new Distrito("caazapa", "Caazapá"),
                new Distrito("doctorMoisesSBertoni", "Doctor Moisés S. Bertoni"),
                new Distrito("fulgencioYegros", "Fulgencio Yegros"),
                new Distrito("generalHiginioMorinigo", "General Higinio Morínigo"),
                new Distrito("maciel", "Maciel"),
                new Distrito("sanJuanNepomuceno", "San Juan Nepomuceno"),
                new Distrito("tavai", "Tavaí"),
                new Distrito("yuty", "Yuty"),
            }
            .ToArray()),
            new Departamento("canindeyu", "Canindeyú", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("corpusChristi", "Corpus Christi"),
                new Distrito("curuguaty", "Curuguaty"),
                new Distrito("gralFranciscoCaballeroAlvarez", "Gral. Francisco Caballero Álvarez"),
                new Distrito("itanara", "Itanará"),
                new Distrito("katuete", "Katueté"),
                new Distrito("laPaloma", "La Paloma"),
                new Distrito("maracana", "Maracaná"),
                new Distrito("nuevaEsperanza", "Nueva Esperanza"),
                new Distrito("saltoDelGuaira", "Salto del Guairá"),
                new Distrito("villaYgatimi", "Villa Ygatimí"),
                new Distrito("yasyCanhy", "Yasy Cañy"),
                new Distrito("ybyrarovana", "Ybyrarovaná"),
                new Distrito("ypehju", "Ypejhú"),
                new Distrito("ybyPyta", "Yby Pytá"),
            }
            .ToArray()),
            new Departamento("central", "Central", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("aregua", "Aregua"),
                new Distrito("capiata", "Capiatá"),
                new Distrito("fernandoDeLaMora", "Fernando de la Mora"),
                new Distrito("guarambare", "Guarambaré"),
                new Distrito("ita", "Itá"),
                new Distrito("itaugua", "Itauguá"),
                new Distrito("julianAugustoSaldivar", "Julián Augusto Saldívar"),
                new Distrito("lambare", "Lambaré"),
                new Distrito("limpio", "Limpio"),
                new Distrito("luque", "Luque"),
                new Distrito("marianoRoqueAlonso", "Mariano Roque Alonso"),
                new Distrito("nemby", "Ñemby"),
                new Distrito("nuevaItalia", "Nueva Italia"),
                new Distrito("sanAntonio", "San Antonio"),
                new Distrito("sanLorenzo", "San Lorenzo"),
                new Distrito("villaElisa", "Villa Elisa"),
                new Distrito("villeta", "Villeta"),
                new Distrito("ypacarai", "Ypacaraí"),
                new Distrito("ypane", "Ypané"),
            }
            .ToArray()),
            new Departamento("concepcion", "Concepción", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("arroyito", "Arroytito"),
                new Distrito("azotey", "Azotey"),
                new Distrito("belen", "Belén"),
                new Distrito("concepcion", "Concepción"),
                new Distrito("horqueta", "Horqueta"),
                new Distrito("loreto", "Loreto"),
                new Distrito("sanCarlosDelApa", "San Carlos del Apa"),
                new Distrito("sanLazaro", "San Lázaro"),
                new Distrito("ybyYau", "Yby Yaú"),
                new Distrito("sargentoJoseFelixLopez", "Sargento José Félix López"),
                new Distrito("sanAlfredo", "San Alfredo"),
                new Distrito("pasoBarreto", "Paso Barreto")
            }
            .ToArray()),
            new Departamento("cordillera", "Cordillera", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("altos", "Altos"),
                new Distrito("arroyosYEsteros", "Arroyos y Esteros"),
                new Distrito("atyra", "Atyrá"),
                new Distrito("caacupe", "Caacupé"),
                new Distrito("caraguatay", "Caraguatay"),
                new Distrito("emboscada", "Emboscada"),
                new Distrito("eusebioAyala", "Eusebio Ayala"),
                new Distrito("islaPucu", "Isla Pucú"),
                new Distrito("itacurubiDeLaCoordillera", "Itacurubí de la Cordillera"),
                new Distrito("juanDeMena", "Juan de Mena"),
                new Distrito("lomaGrande", "Loma Grande"),
                new Distrito("mbocayatyDelYhaguy", "Mbocayaty del Yhaguy"),
                new Distrito("nuevaColombia", "Nueva Colombia"),
                new Distrito("piribebuy", "Piribebuy"),
                new Distrito("primeroDeMarzo", "Primero de Marzo"),
                new Distrito("sanBernardino", "San Bernardino"),
                new Distrito("sanJoseObrero", "San José Obrero"),
                new Distrito("santaElena", "Santa Elena"),
                new Distrito("tobati", "Tobatí"),
                new Distrito("valenzuela", "Valenzuela")
            }
            .ToArray()),
            new Departamento("guaira", "Guairá", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("borja", "Borja"),
                new Distrito("coloniaIndependencia", "Colonia Independencia"),
                new Distrito("coronelMartinez", "Coronel Martínez"),
                new Distrito("drBottrell", "Dr. Bottrell"),
                new Distrito("fassardi", "Fassardi"),
                new Distrito("felixPerezCardozo", "Félix Pérez Cardozo"),
                new Distrito("garay", "Garay"),
                new Distrito("itape", "Itapé"),
                new Distrito("iturbe", "Iturbe"),
                new Distrito("mbocayaty", "Mbocayaty"),
                new Distrito("natalicioTalavera", "Natalicio Talavera"),
                new Distrito("nhumi", "Ñumí"),
                new Distrito("pasoYobai", "Paso Yobái"),
                new Distrito("sanSalvador", "San Salvador"),
                new Distrito("tebicuary", "Tebicuary"),
                new Distrito("troche", "Troche"),
                new Distrito("villarrica", "Villarrica"),
                new Distrito("yataity", "Yataity")
            }
            .ToArray()),
            new Departamento("itapua", "Itapúa", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("altoVera", "Alto Verá"),
                new Distrito("bellaVista", "Bella Vista"),
                new Distrito("cambyreta", "Cambyretá"),
                new Distrito("capitanMeza", "Capitán Meza"),
                new Distrito("capitanMiranda", "Capitán Miranda"),
                new Distrito("carlosAntonioLopez", "Carlos Antonio López"),
                new Distrito("carmenDelParana", "Carmen del Paraná"),
                new Distrito("coronelBogado", "Coronel Bogado"),
                new Distrito("edelira", "Edelira"),
                new Distrito("ecarnacion", "Encarnación"),
                new Distrito("fram", "Fram"),
                new Distrito("gerenalArtigas", "General Artigas"),
                new Distrito("generalDelgado", "General Delgado"),
                new Distrito("hohenau", "Hohenau"),
                new Distrito("itapuaPoty", "Itapúa Poty"),
                new Distrito("jesus", "Jesús"),
                new Distrito("laPaz", "La Paz"),
                new Distrito("joseLeandroOviedo", "José Leandro Oviedo"),
                new Distrito("mayorOtanho", "Mayor Otaño"),
                new Distrito("natalio", "Natalio"),
                new Distrito("nuevaAlborada", "Nueva Alborada"),
                new Distrito("obligado", "Obligado"),
                new Distrito("pirapo", "Pirapo"),
                new Distrito("sanCosmeYDamian", "San Cosme y Damián"),
                new Distrito("sanJuanDelParana", "San Juan del Paraná"),
                new Distrito("sanRafaelDelParana", "San Rafael del Paraná"),
                new Distrito("tomasRomeroPereira", "Tomás Romero Pereira"),
                new Distrito("trinidad", "Trinidad"),
                new Distrito("yatytay", "Yatytay")
            }
            .ToArray()),
            new Departamento("misiones", "Misiones", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("ayolas", "Ayolas"),
                new Distrito("sanIgnacio", "San Ignacio"),
                new Distrito("sanJuanBautista", "San Juan Bautista"),
                new Distrito("sanMiguel", "San Miguel"),
                new Distrito("sanPatricio", "San Patricio"),
                new Distrito("santaMaria", "Santa María"),
                new Distrito("santaRosa", "Santa Rosa"),
                new Distrito("santiago", "Santiago"),
                new Distrito("villaFlorida", "Villa Florida"),
                new Distrito("yabebyry", "Yabebyry"),
            }
            .ToArray()),
            new Departamento("neembucu", "Ñeembucu", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("alberdi", "Alberdi"),
                new Distrito("cerrito", "Cerrito"),
                new Distrito("desmochados", "Desmochados"),
                new Distrito("generalJoseEduvigisDiaz", "General José Eduvigis Díaz"),
                new Distrito("guazuCua", "Guazú Cuá"),
                new Distrito("humaita", "Humaitá"),
                new Distrito("islaUmbu", "Isla Umbú"),
                new Distrito("laureles", "Laureles"),
                new Distrito("mayorJoseJMartinez", "Mayor José J. Martínez"),
                new Distrito("pasoDePatria", "Paso de Patria"),
                new Distrito("pilar", "Pilar"),
                new Distrito("sanJuanBautistaDeNeembucu", "San Juan Bautista de Ñeembucú"),
                new Distrito("tacuaras", "Tacuaras"),
                new Distrito("villaFranca", "Villa Franca"),
                new Distrito("villabin", "Villabín"),
                new Distrito("villaOliva", "Villa Oliva"),
            }
            .ToArray()),
            new Departamento("paraguari", "Paraguarí", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("acahay", "Acahay"),
                new Distrito("cappcucu", "Caapucú"),
                new Distrito("carapegua", "Carapeguá"),
                new Distrito("escobar", "Escobar"),
                new Distrito("gralBernardinoCaballero", "Gral. Bernardino Caballero"),
                new Distrito("laColmena", "La Colmena"),
                new Distrito("mariaAntonia", "María Antonia"),
                new Distrito("mbuyapey", "Mbuyapey"),
                new Distrito("paraguari", "Paraguarí"),
                new Distrito("pirayu", "Pirayú"),
                new Distrito("quiindy", "Quiindy"),
                new Distrito("quyquyho", "Quyquyhó"),
                new Distrito("sanRoqueGonzalezDeSantaCruz", "San Roque González de Santa Cruz"),
                new Distrito("sapucai", "Sapucai"),
                new Distrito("tebicuarymi", "Tebicuarymí"),
                new Distrito("yaguaron", "Yaguarón"),
                new Distrito("ybycui", "Ybycuí"),
                new Distrito("ybytymi", "Yvytymí")
            }
            .ToArray()),
            new Departamento("presidenteHayes", "Presidente Hayes", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("benjaminAceval", "Benjamín Aceval"),
                new Distrito("drJoseFalcon", "Dr. José Falcón"),
                new Distrito("generalJoseMariaBruguez", "General José María Bruguez"),
                new Distrito("nanawa", "Nanawa"),
                new Distrito("puertoPinasco", "Puerto Pinazco"),
                new Distrito("tteIralaFernandez", "Tte. Irala Fernández"),
                new Distrito("estebanMartinez", "Esteban Martínez"),
                new Distrito("villaHayes", "Villa Hayes"),
            }
            .ToArray()),
            new Departamento("sanPedro", "San Pedro", new HashSet<Distrito>(distritoComparer)
            {
                new Distrito("antequera", "Antequera"),
                new Distrito("capiibary", "Capiibary"),
                new Distrito("chore", "Choré"),
                new Distrito("generalElizardoAquino", "General Elizardo Aquino"),
                new Distrito("generalIsidoro", "General Isidoro Resquín"),
                new Distrito("guayaibi", "Guayaibí"),
                new Distrito("itacurubiDelRosario", "Itacurubí del Rosario"),
                new Distrito("liberacion", "Liberación"),
                new Distrito("lima", "Lima"),
                new Distrito("nuevaGermania", "Nueva Germania"),
                new Distrito("sanEstanislao", "San Estanislao"),
                new Distrito("sanPablo", "San Pablo"),
                new Distrito("sanPedroDelYcuamandiyu", "San Pedro del Ycuamandiyú"),
                new Distrito("sanVicentePancholo", "San Vicente Pancholo"),
                new Distrito("santaRosaDelAguaray", "Santa Rosa del Aguaray"),
                new Distrito("tacuati", "Tacuatí"),
                new Distrito("union", "Unión"),
                new Distrito("veinticincoDeDiciembre", "Venticinco de Diciembre"),
                new Distrito("villaDelRosario", "Villa del Rosario"),
                new Distrito("yataityDelNorte", "Yataity del Norte"),
                new Distrito("yrybycua", "Yrybucuá del Norte")
            }
            .ToArray()),
        };

    }

    internal class DepartamentoComparer : IEqualityComparer<Departamento>
    {
        public bool Equals(Departamento x, Departamento y) => x.Id == y.Id;

        public int GetHashCode(Departamento obj) => obj.Id.GetHashCode();
    }

    internal class DistritoComparer : IEqualityComparer<Distrito>
    {
        public bool Equals(Distrito x, Distrito y) => x.Id == y.Id;

        public int GetHashCode(Distrito obj) => obj.Id.GetHashCode();
    }
}
