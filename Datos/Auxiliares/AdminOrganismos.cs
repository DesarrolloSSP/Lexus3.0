using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lexus2_0.Datos.Auxiliares
{
    public class AdminOrganismos
    {
        /*
        63	Departamento de lo Contencioso, Amparos y Administrativo
        67	Departamento de Defensa de lo Derechos Humanos
        70	Departamento de Estudios Legislativos
        73	Departamento de Tramitación de Auxilios de la Fuerza Pública
        
        450	ENLACE ADMINISTRATIVO EN LA DIRECCIÓN GENERAL JURÍDICA

        76	Delegación Jurídica con la Sebsecretaría de Seguridad Pública "A"
        id	nombre
        77	Oficina de Arrestos, Traslados, Aprehensiones y Auxilios de la Fuerza Pública
        id	nombre
        79	Oficina de Derechos Humanos y Amparos
        id	nombre
        81	Oficina de Abogados en Delegaciones

         */
        public enum AREAS_JURIDICO
        {
            DEPTO_CONTENCIOSO = 63,
            DEPTO_DERECHOS_HUMANOS = 67,
            DEPTO_ESTUDIOS_LEGISLATIVOS = 70,
            DEPTO_TRAMITACION_FUERZA_PUBLICA = 73,
            DEPTO_ENLACE_ADMIN_DGJ = 450,
            DELEGACION_JURIDICA_SUBSE = 76,
            DIR_GEN_JUR = 61
        }

        public static string NOMENCLATURA_DEPTO_CONTENCIOSO = "CAA";
        public static string NOMENCLATURA_DEPTO_DERECHOS_HUMANOS = "DDH";
        public static string NOMENCLATURA_DEPTO_ESTUDIOS_LEGISLATIVOS = "EL";
        public static string NOMENCLATURA_DEPTO_TRAMITACION_FUERZA_PUBLICA = "TAFP";
        public static string NOMENCLATURA_DEPTO_ENLACE_ADMINISTRATIVO_DGJ = "EADGJ";
        public static string NOMENCLATURA_DELJUR_SUBSECRETARIA = "DJS";
        public static string NOMENCLATURA_DIR_GEN_JUR = "DIR";

        public static bool tieneNombrePropio(string jerarquia)
        {
            bool tiene = false;
            if (jerarquia.IndexOf("/5/") == 0 ||
                jerarquia.IndexOf("/6/") == 0 ||
                jerarquia.IndexOf("/7/") == 0 ||
                jerarquia.IndexOf("/8/") == 0 ||
                jerarquia.IndexOf("/20/") == 0)
                tiene = true;
            return tiene;
        }

        public string obtenerNomenclatura(int idArea)
        {
            string nomenclatura = "";
            switch (idArea)
            {
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_DERECHOS_HUMANOS: nomenclatura = NOMENCLATURA_DEPTO_DERECHOS_HUMANOS; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_CONTENCIOSO: nomenclatura = NOMENCLATURA_DEPTO_CONTENCIOSO; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ESTUDIOS_LEGISLATIVOS: nomenclatura = NOMENCLATURA_DEPTO_ESTUDIOS_LEGISLATIVOS; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_TRAMITACION_FUERZA_PUBLICA: nomenclatura = NOMENCLATURA_DEPTO_TRAMITACION_FUERZA_PUBLICA; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ENLACE_ADMIN_DGJ: nomenclatura = NOMENCLATURA_DEPTO_ENLACE_ADMINISTRATIVO_DGJ; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DELEGACION_JURIDICA_SUBSE: nomenclatura = NOMENCLATURA_DELJUR_SUBSECRETARIA; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DIR_GEN_JUR: nomenclatura = NOMENCLATURA_DIR_GEN_JUR; break;
                default: break;
            }
            return nomenclatura;
        }
    }
}