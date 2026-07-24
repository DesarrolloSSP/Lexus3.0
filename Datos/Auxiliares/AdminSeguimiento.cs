using Lexus2._0.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lexus2_0.Datos.Auxiliares;

//CLASE AUXILIAR PARA EL REGISTRO DEL HISTÓRICO O SEGUIMIENTO DEL SISTEMA (CLASE AÚN EN DESARROLLO)
namespace Lexus2_0.Datos.Auxiliares
{
    public class AdminSeguimiento
    {
        public enum AccionRealizada
        {
            CREACION_EXPEDIENTE = 1,
            CREACION_ACTUACION = 2,
            EDICION_EXPEDIENTE_DETALLE = 4,
            REGISTRO_NUEVO_ORGANISMO = 5,
            EDICION_CATALOGO = 6,
            REGISTRO_NUEVA_PERSONA = 7,
            ABRIR_EXPEDIENTE = 8,
            CAMBIO_AREA = 9,
            CREACION_DOCUMENTO = 10,
            ASIGNACION_A_EXPEDIENTE = 11,
            EDICION_DOCUMENTO = 12
        }

        //  [idAccionRealizada]
        //,[fechaYhora]
        //,[tabla]
        //,[idRegistro]
        //,[cambiosRealizados]
        //,[usuario]
        //,[_fechaRegistro]

        public void guardarCambioArea(string idExpediente, DateTime fecha, string usuario, string cambiosRealizados)
        {
            BDJuridicoEntities ctx = new BDJuridicoEntities();
            Historico nuevoHistorico = new Historico();
            nuevoHistorico.idAccionRealizada = (int)AccionRealizada.CAMBIO_AREA; //8;//8 PA LAS PRUEBAS //(int)AccionRealizada.CAMBIO_AREA; PAL REAL
            nuevoHistorico.fechaYhora = fecha;
            nuevoHistorico.tabla = "tbExpedientes";
            nuevoHistorico.idRegistro = idExpediente;
            nuevoHistorico.cambiosRealizados = cambiosRealizados;
            nuevoHistorico.usuario = usuario;
            nuevoHistorico.C_fechaRegistro = DateTime.Now;
            ctx.Historico.Add(nuevoHistorico);
            ctx.SaveChanges();
        }
        public void guardarSeguimientoEdicionExpediente(tbExpedientes expedienteAnterior, tbExpedientes expedienteNuevo, DateTime fecha, string usuario)
        {
            try
            {
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                string cambios = "";
                if (expedienteAnterior.idOrganismoOrigen != expedienteNuevo.idOrganismoOrigen)
                {
                    cambios += "Organismo Origen Anterior = '" + expedienteAnterior.idOrganismoOrigen + "' - Organismo Origen Nuevo = '" + expedienteNuevo.idOrganismoOrigen + "'|";
                }
                if (expedienteAnterior.idOrganismoDestino != expedienteNuevo.idOrganismoDestino)
                {
                    cambios += "Organismo Destino Anterior = '" + expedienteAnterior.idOrganismoDestino + "' - Organismo Destino Nuevo = '" + expedienteNuevo.idOrganismoDestino + "'|";
                }
                if (!expedienteAnterior.asunto.Equals(expedienteNuevo.asunto))
                {
                    cambios += "Asunto Anterior = '" + expedienteAnterior.asunto + "' - Asunto Nuevo = '" + expedienteNuevo.asunto + "'|";
                }
                if (expedienteAnterior.idEjercicio != expedienteNuevo.idEjercicio)
                {
                    cambios += "Ejecicio Anterior = '" + expedienteAnterior.idEjercicio + "' - Ejercicio Nuevo = '" + expedienteNuevo.idEjercicio + "'|";
                }
                //if (expedienteAnterior.tbExpedPersonas.Count() != expedienteNuevo.tbExpedPersonas.Count())
                //{
                    if (expedienteNuevo.tbExpedPersonas.Count() == 0)
                    {
                        //BORRARON TODAS LAS PERSONAS, POR LO QUE SE DEBEN GUARDAR LAS RELACIONES ANTERIORES
                        cambios += "ELIMINACIÓN DE REGISTROS DE PERSONAS RELACIONADAS: ";
                        foreach (tbExpedPersonas relExpedPers in expedienteAnterior.tbExpedPersonas)
                        {
                            tbPersonas persona = ctx.tbPersonas.Find(relExpedPers.idPersona);
                            catFiguras figura = ctx.catFiguras.Find(relExpedPers.idFigura);
                            if (persona != null && figura != null)
                            {
                                cambios += "ID_tbExpedPersonas = '" + relExpedPers.id + "' - ID_tbPersonas = '" + persona.id + "' - NOMBRE = '" + persona.nombre + " " + persona.apellidoPaterno + " " + persona.apellidoMaterno + "' - ID_catFiguras = '" + figura.id + "' - FIGURA = '" + figura.figura + "'/";
                            }
                        }
                        cambios += "|";
                    }
                    else
                    {
                        //VERIFICAR LAS RELACIONES DEL LISTADO NUEVO
                        List<tbExpedPersonas> listaTemporalNueva = expedienteNuevo.tbExpedPersonas.ToList();
                        List<tbExpedPersonas> listaTemporalAnterior = expedienteAnterior.tbExpedPersonas.ToList();
                        foreach (tbExpedPersonas relExpedPers in expedienteAnterior.tbExpedPersonas)
                        {
                            tbExpedPersonas relConservada = listaTemporalNueva.Where(temp => temp.idPersona == relExpedPers.idPersona && temp.idFigura == relExpedPers.idFigura).FirstOrDefault();
                            if (relConservada != null)
                            {
                                listaTemporalNueva.Remove(relConservada);
                                listaTemporalAnterior.Remove(relConservada);
                            }
                        }
                        if (listaTemporalAnterior.Count > 0)
                        {
                            //BORRARON ALGUNAS RELACIONES
                            cambios += "ELIMINACIÓN DE REGISTROS DE PERSONAS RELACIONADAS: ";
                            foreach (tbExpedPersonas relExpedPers in listaTemporalAnterior)
                            {
                                //cambios += "ID = '" + relExpedPers.id + "' - NOMBRE = '" + relExpedPers.tbPersonas.nombre + " " + relExpedPers.tbPersonas.apellidoPaterno + " " + relExpedPers.tbPersonas.apellidoMaterno + "' - FIGURA = '" + relExpedPers.catFiguras.figura + "'/";
                                tbPersonas persona = ctx.tbPersonas.Find(relExpedPers.idPersona);
                                catFiguras figura = ctx.catFiguras.Find(relExpedPers.idFigura);
                                if (persona != null && figura != null)
                                {
                                    cambios += "ID_tbExpedPersonas = '" + relExpedPers.id + "' - ID_tbPersonas = '" + persona.id + "' - NOMBRE = '" + persona.nombre + " " + persona.apellidoPaterno + " " + persona.apellidoMaterno + "' - ID_catFiguras = '" + figura.id + "' - FIGURA = '" + figura.figura + "'/";
                                }
                            }
                            cambios += "|";
                        }
                        if (listaTemporalNueva.Count > 0)
                        {
                            //AGREGARON ALGUNAS RELACIONES
                            cambios += "REGISTROS NUEVOS DE PERSONAS RELACIONADAS: ";
                            foreach (tbExpedPersonas relExpedPers in listaTemporalNueva)
                            {
                                //cambios += "ID = '" + relExpedPers.id + "' - NOMBRE = '" + relExpedPers.tbPersonas.nombre + " " + relExpedPers.tbPersonas.apellidoPaterno + " " + relExpedPers.tbPersonas.apellidoMaterno + "' - FIGURA = '" + relExpedPers.catFiguras.figura + "'/";
                                tbPersonas persona = ctx.tbPersonas.Find(relExpedPers.idPersona);
                                catFiguras figura = ctx.catFiguras.Find(relExpedPers.idFigura);
                                if (persona != null && figura != null)
                                {
                                    cambios += "ID_tbExpedPersonas = '" + relExpedPers.id + "' - ID_tbPersonas = '" + persona.id + "' - NOMBRE = '" + persona.nombre + " " + persona.apellidoPaterno + " " + persona.apellidoMaterno + "' - ID_catFiguras = '" + figura.id + "' - FIGURA = '" + figura.figura + "'/";
                                }
                            }
                            cambios += "|";
                        }
                    }
                //}
                Historico nuevoHist = new Historico();
                nuevoHist.idAccionRealizada = (int)AccionRealizada.EDICION_EXPEDIENTE_DETALLE;
                nuevoHist.tabla = "tbExpedientes";
                nuevoHist.idRegistro = expedienteNuevo.id.ToString();
                nuevoHist.fechaYhora = fecha;
                nuevoHist.cambiosRealizados = cambios;
                nuevoHist.usuario = usuario;
                nuevoHist.C_fechaRegistro = DateTime.Now;
                //BDJuridicoEntities ctx = new BDJuridicoEntities();
                ctx.Historico.Add(nuevoHist);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public void guardarSegimientoCreacion(AccionRealizada accion, string tabla, string id, string nuevoValor, DateTime fecha, string usuario)
        {
            try
            {
                Historico nuevoHist = new Historico();
                nuevoHist.idAccionRealizada = (int)accion;
                nuevoHist.tabla = tabla;
                nuevoHist.idRegistro = id;
                nuevoHist.cambiosRealizados = nuevoValor;
                nuevoHist.fechaYhora = fecha;
                nuevoHist.usuario = usuario;
                nuevoHist.C_fechaRegistro = DateTime.Now;
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                ctx.Historico.Add(nuevoHist);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }
        public void guardarAperturaDelExpediente(string id, DateTime fecha, string usuario, string idDocto, string numDocto)
        {
            try
            {
                Historico nuevoHist = new Historico();
                nuevoHist.idAccionRealizada = (int)AccionRealizada.ABRIR_EXPEDIENTE;
                nuevoHist.tabla = "tbExpediente";
                nuevoHist.idRegistro = id;
                nuevoHist.cambiosRealizados = "VISUALIZACIÓN DEL EXPEDIENTE" + (idDocto != null ? (" | DOCUMENTO SELECCIONADO = '" + numDocto + "' - ID = '" + idDocto + "'") : "");
                nuevoHist.fechaYhora = fecha;
                nuevoHist.usuario = usuario;
                nuevoHist.C_fechaRegistro = DateTime.Now;
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                ctx.Historico.Add(nuevoHist);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public static void GuardaModificacionDelDocumento(string idDocto, AccionRealizada Accion, DateTime fecha, string usuario, string texto, string numeroDocto)
        {
            try
            {
                Historico nuevoHist = new Historico();
                nuevoHist.idAccionRealizada = (int)Accion;
                nuevoHist.tabla = "tbExpedActDocumentos";
                nuevoHist.idRegistro = idDocto;
                nuevoHist.cambiosRealizados = $"SE HA MODIFICADO EL DOCUMENTO CON NÚMERO: {numeroDocto} CON LO SIGUIENTE: {texto}";
                nuevoHist.fechaYhora = fecha;
                nuevoHist.usuario = usuario;
                nuevoHist.C_fechaRegistro = DateTime.Now;
                BDJuridicoEntities ctx = new BDJuridicoEntities();
                ctx.Historico.Add(nuevoHist);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}