using Lexus2._0.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Providers.Entities;
using System.Web.Security;

namespace Lexus2_0.Datos.Auxiliares
{
    public class AdminPersonas
    {
        public static int ID_ABOGADO_ASIGNADO = 1;
        public static string R_SYSADMIN = "SYSADMIN";
        public static string R_OFICIALIA = "Oficialia";
        public static string R_DIR_GEN_JUR = "Dirección General Jurídica";
        public static string R_ADMIN_DH = "Administrador Derechos Humanos";
        public static string R_ADMIN_CONT = "Administrador Contencioso";
        public static string R_ADMIN_ESTLEG = "Administrador Estudios Legislativos";
        public static string R_ADMIN_FUERZAP = "Administrador Fuerza Pública";
        public static string R_SUP_DH = "Supervisor Derechos Humanos";
        public static string R_SUP_CONT = "Supervisor Contencioso";
        public static string R_SUP_ESTLEG = "Supervisor Estudios Legislativos";
        public static string R_SUP_FUERZAP = "Supervisor Fuerza Pública";
        public static string R_ADMIN_ENLACE_ADMIN = "Administrador Enlace Administrativo en la DGJ";
        public static string R_OFICIALIA_DELEGACION_JURIDICA = "Oficialia de la Delegacion Juridica";

        /*
        63	Departamento de lo Contencioso, Amparos y Administrativo
        67	Departamento de Defensa de lo Derechos Humanos
        70	Departamento de Estudios Legislativos
        73	Departamento de Tramitación de Auxilios de la Fuerza Pública
       */

       /*
       (No column name)	idFigura
       25696	19	REMITENTE
       (No column name)	idFigura
       24614	18	DESTINATARIO
       (No column name)	idFigura
       557	15		PROMOVIENTE
       (No column name)	idFigura
       143	8		QUEJOSO
       */
        public enum FIGURAS_CLAVE { PERSONA_RECIBE = 17, DESTINATARIO = 18, REMITENTE = 19, PROMOVIENTE = 15, QUEJOSO = 8 };
        /// <summary>
        /// Método que busca el ID del área a la que pertenece. Retorna 0 si es Oficialía, Dirección y SysAdmin
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="nivelJerarquico"></param>
        /// <returns></returns>
        public int idAreaOrganizacional(string rol, out int nivelJerarquico)
        {
            nivelJerarquico = 0;
            if (rol.Equals(R_SYSADMIN))
                return 0;
            else if (rol.Equals(R_OFICIALIA))
                return 0;//return -1;
            else if (rol.Equals(R_DIR_GEN_JUR))
                return 0;
            else if (rol.Equals(R_ADMIN_DH))
            {
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_DERECHOS_HUMANOS;
            }
            else if (rol.Equals(R_SUP_DH))
            {
                nivelJerarquico = 1;
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_DERECHOS_HUMANOS;
            }
            else if (rol.Equals(R_ADMIN_CONT))
            {
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_CONTENCIOSO;
            }
            else if (rol.Equals(R_SUP_CONT))
            {
                nivelJerarquico = 1;
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_CONTENCIOSO;
            }
            else if (rol.Equals(R_ADMIN_ESTLEG))
            {
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ESTUDIOS_LEGISLATIVOS;
            }
            else if (rol.Equals(R_SUP_ESTLEG))
            {
                nivelJerarquico = 1;
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ESTUDIOS_LEGISLATIVOS;
            }
            else if (rol.Equals(R_ADMIN_FUERZAP))
            {
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_TRAMITACION_FUERZA_PUBLICA;
            }
            else if (rol.Equals(R_SUP_FUERZAP))
            {
                nivelJerarquico = 1;
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_TRAMITACION_FUERZA_PUBLICA;
            }
            else if (rol.Equals(R_ADMIN_ENLACE_ADMIN))
            {
                return (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ENLACE_ADMIN_DGJ;
            }
            else if (rol.Equals(R_OFICIALIA_DELEGACION_JURIDICA))
            {
                return (int)AdminOrganismos.AREAS_JURIDICO.DELEGACION_JURIDICA_SUBSE;
            }
            else
                return -2;
        }
        public string[] rolesPorAreaOrganizacional(int idArea)
        {
            string[] roles = new string[] { };
            switch (idArea)
            {
                case 0: roles = new string[] { R_SYSADMIN }; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_DERECHOS_HUMANOS: roles = new string[] { R_ADMIN_DH, R_SUP_DH }; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_CONTENCIOSO: roles = new string[] { R_ADMIN_CONT, R_SUP_CONT }; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ESTUDIOS_LEGISLATIVOS: roles = new string[] { R_ADMIN_ESTLEG, R_SUP_ESTLEG }; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_TRAMITACION_FUERZA_PUBLICA: roles = new string[] { R_ADMIN_FUERZAP, R_SUP_FUERZAP }; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DEPTO_ENLACE_ADMIN_DGJ: roles = new string[] { R_ADMIN_ENLACE_ADMIN }; break;
                case (int)AdminOrganismos.AREAS_JURIDICO.DELEGACION_JURIDICA_SUBSE: roles = new string[] { R_OFICIALIA_DELEGACION_JURIDICA }; break;
            }
            return roles;
        }
        public bool puedeCrearExpedientes(string rol)
        {
            bool puede = true;
            if (rol.Equals(R_SUP_DH) || rol.Equals(R_SUP_CONT) || rol.Equals(R_SUP_ESTLEG) || rol.Equals(R_SUP_FUERZAP))
                puede = false;
            return puede;
        }
        public string textoPersonaPorUsuario(string userName)
        {
            string mensaje = "";
            if (userName.Equals("GUARDIA"))
            {
                //ES IMPOSIBLE POR EL MOMENTO SABER QUIÉN USÓ LA CUENTA DE LA GUARDIA A MENOS QUE SE CRUCE INFORMACIÓN CON EL ÁREA SOBRE QUIENES TUVIERON GUARDIAS????
                mensaje = "[" + userName + "]";
            }
            else
            {
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                tbDatosSistema datoSistema = ctx.tbDatosSistema.Where(ds => ds.usuarioMmshp.Equals(userName)).FirstOrDefault();
                if (datoSistema != null)
                {
                    if (!string.IsNullOrEmpty(datoSistema.tbPersonas.nombreCompleto))
                        mensaje = datoSistema.tbPersonas.nombreCompleto + " <b>[" + userName + "]</b>";
                    else
                        mensaje = datoSistema.tbPersonas.nombre + " " + datoSistema.tbPersonas.apellidoPaterno + " " + datoSistema.tbPersonas.apellidoMaterno + " <b>[" + userName + "]</b>";
                }
            }
            return mensaje.ToUpper();
        }
        public static string NombrePersonaUsuario(string userName, out int idPersona)
        {
            try
            {
                string mensaje = "";
                idPersona = -1;
                if (!userName.Equals("GUARDIA"))
                {
                    BDJuridicoEntities ctx = new BDJuridicoEntities();
                    var datos = ctx.tbDatosSistema.Where(d => d.usuarioMmshp.Equals(userName) && d.tbPersonas != null).Select(d => new { d.tbPersonas.nombreCompleto, d.tbPersonas.id }).FirstOrDefault();
                    if (datos != null)
                    {
                        mensaje = datos.nombreCompleto;
                        idPersona = datos.id;
                    }
                }
                return mensaje.ToUpper();
            }
            catch (Exception EX)
            {

                throw;
            }

        }
    }
}