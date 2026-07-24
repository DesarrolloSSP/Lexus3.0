using Lexus2._0.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

//CLASE AUXILIAR PARA EL CONTROL Y FUNCIONAMIENTO DE LOS 'TÉRMINOS' O FECHAS Y HORAS LÍMITE EN QUE SE DEBE CONCLUIR UN EXPEDIENTE O ACCIÓN (CLASE AÚN EN DESARROLLO)
namespace Lexus2_0.Datos.Auxiliares
{
    public class AdminTermino
    {
        public enum EtiquetaTermino
        {
            HORA = 1,
            DIA = 2,
            SEMANA = 3,
            MES = 4,
            ANIO = 5
        }
        public enum TipoDia
        {
            NATURAL = 0,
            LABORAL = 1
        }
        public enum SEMAFORO
        {
            VACÍO = 4,
            ROJO = 1,
            AMARILLO = 2,
            VERDE = 3,
            ATENDIDO = 5,
            TEMP_SUSPENDIDO = 6
        }
        public string[] rutaImgSemaforo = new string[] {
            "",
            "~/Images/Iconos/Semaforo/semRojo.png",
            "~/Images/Iconos/Semaforo/semAmarillo.png",
            "~/Images/Iconos/Semaforo/semVerde.png",
            "~/Images/Iconos/Semaforo/semVacio.png",
            "~/Images/Iconos/Semaforo/semAtendido.png",
            "~/Images/Iconos/Semaforo/semBloqueado.png"
        };

        /// <summary>
        /// Obtiene la fecha de término a partir de una fecha inicial, el tipo de término seleccionado y el tipo de día (natural-Se sumarán los días, laboral-no se considerarán festivos y sabado/domingo)
        /// </summary>
        /// <param name="fechaInicial">La fecha inicial a la que se le 'sumarán' los días.</param>
        /// <param name="idTermino">El identificador del tipo de término</param>
        /// <param name="tipoDia">El identificador del tipo de día en el enum de la página (0 Natural, 1 Laboral)</param>
        public DateTime obtenerFecha(DateTime fechaInicial, int idTermino, int tipoDia)
        {
            DateTime fechaTermino = fechaInicial;
            BDJuridicoEntities ctx = new BDJuridicoEntities();
            catTerminos terminoSeleccionado = ctx.catTerminos.Find(idTermino);
            int cantidad = Convert.ToInt32(terminoSeleccionado.cantidad);
            int cantidadCFDS;
            switch (terminoSeleccionado.idEtiquetaTiempo)
            {
                case (int)EtiquetaTermino.HORA:
                    if (tipoDia == (int)TipoDia.NATURAL)
                        fechaTermino = fechaTermino.AddHours(cantidad);
                    else
                    {
                        cantidadCFDS = diasConFinesDeSemana(fechaInicial, fechaTermino.AddHours(cantidad));
                        fechaTermino = fechaInicial.AddDays(cantidadCFDS);
                        if (fechaInicial.AddDays(cantidadCFDS) == fechaInicial)
                            fechaTermino = fechaTermino.AddHours(cantidad);
                    }
                    break;
                case (int)EtiquetaTermino.DIA:
                    if (tipoDia == (int)TipoDia.NATURAL)
                        fechaTermino = fechaTermino.AddDays(cantidad);
                    else
                    {
                        cantidadCFDS = diasConFinesDeSemana(fechaInicial, fechaTermino.AddDays(cantidad));
                        fechaTermino = fechaInicial.AddDays(cantidadCFDS);
                    }
                    break;
                case (int)EtiquetaTermino.SEMANA:
                    if (tipoDia == (int)TipoDia.NATURAL)
                        fechaTermino = fechaTermino.AddDays(cantidad * 7);
                    else
                    {
                        cantidadCFDS = diasConFinesDeSemana(fechaInicial, fechaTermino.AddDays(cantidad * 7));
                        fechaTermino = fechaInicial.AddDays(cantidadCFDS);
                    }
                    break;
                case (int)EtiquetaTermino.MES:
                    if (tipoDia == (int)TipoDia.NATURAL)
                        fechaTermino = fechaTermino.AddMonths(cantidad);
                    else
                    {
                        cantidadCFDS = diasConFinesDeSemana(fechaInicial, fechaTermino.AddMonths(cantidad));
                        fechaTermino = fechaInicial.AddDays(cantidadCFDS);
                    }
                    break;
                case (int)EtiquetaTermino.ANIO:
                    if (tipoDia == (int)TipoDia.NATURAL)
                        fechaTermino = fechaTermino.AddYears(cantidad);
                    else
                    {
                        cantidadCFDS = diasConFinesDeSemana(fechaInicial, fechaTermino.AddYears(cantidad));
                        fechaTermino = fechaInicial.AddDays(cantidadCFDS);
                    }
                    break;
            }
            fechaTermino = fechaSinFestivoNiFinDeSemana(fechaTermino);
            return fechaTermino;
        }
        public bool terminoValido(DateTime fecha)
        {
            BDJuridicoEntities ctx = new BDJuridicoEntities();
            catDiasInhabiles presenteDiaInhabil = ctx.catDiasInhabiles.Where(di => di.fechaInicio <= fecha.Date && di.fechaFin >= fecha.Date).FirstOrDefault();
            if (presenteDiaInhabil != null || (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        /// <summary>
        /// Verifica si la fecha es día festivo (del calendario del sistema) o es de fin de semana (sábado o domingo). De serlo, obtiene la siguiente fecha laboral. En caso de que no, devuelve la misma fecha ingresada.
        /// </summary>
        /// <param name="fecha">Fecha en la cual iniciar</param>
        public DateTime fechaSinFestivoNiFinDeSemana(DateTime fecha)
        {
            DateTime fechaNueva = fecha;
            BDJuridicoEntities ctx = new BDJuridicoEntities();
            catDiasInhabiles presenteDiaInhabil = ctx.catDiasInhabiles.Where(di => di.fechaInicio.Value <= fechaNueva.Date && di.fechaFin.Value >= fechaNueva.Date).FirstOrDefault();
            while (presenteDiaInhabil != null || (fechaNueva.DayOfWeek == DayOfWeek.Saturday || fechaNueva.DayOfWeek == DayOfWeek.Sunday))
            {
                if (presenteDiaInhabil != null)
                {
                    //Es un día que se contempla en días inhabiles, por lo que se sumarán días hasta el siguiente día hábil
                    while (presenteDiaInhabil.fechaFin.Value.Date >= fechaNueva.Date)
                    {
                        fechaNueva = fechaNueva.AddDays(1);
                    }
                }
                else
                {
                    //El día no está contemplado como inhabil, sin embargo se debe buscar al siguiente día hábil
                    while (fechaNueva.DayOfWeek == DayOfWeek.Saturday || fechaNueva.DayOfWeek == DayOfWeek.Sunday)
                    {
                        fechaNueva = fechaNueva.AddDays(1);
                    }
                }
                presenteDiaInhabil = ctx.catDiasInhabiles.Where(di => di.fechaInicio <= fechaNueva.Date && di.fechaFin >= fechaNueva.Date).FirstOrDefault();
            }
            return fechaNueva;
        }
        /// <summary>
        /// OBTIENE LOS DÍAS TOTALES CONTENIDOS ENTRE DOS FECHAS, CON LA CONDICIONANTE DE 'SUMAR' LOS FINES DE SEMANA, ES DECIR, EN VEZ DE TOMAR DÍAS NATURALES, SE CONSIDERAN ADEMÁS DÍAS FESTIVOS Y FINES DE SEMANA.
        /// </summary>
        /// <param name="fechaInicial">Fecha en la cual iniciar</param>
        /// <param name="fechaFinal">Fecha 'final' o de la cual tomar la diferencia</param>
        protected int diasConFinesDeSemana(DateTime fechaInicial, DateTime fechaFinal)
        {
            int dias = 0;
            TimeSpan diferencia = fechaFinal - fechaInicial;
            int i = 0;
            int diasDiferencia = diferencia.Days;
            while (diasDiferencia >= 0)
            {
                if (fechaInicial.DayOfWeek != DayOfWeek.Saturday && fechaInicial.DayOfWeek != DayOfWeek.Sunday)
                    diasDiferencia--;
                dias++;
                fechaInicial = fechaInicial.AddDays(1);
            }
            dias--;
            return dias;
        }
        /// <summary>
        /// Obtiene la notificación en texto en relación a lo que falta de tiempo con respecto a la fecha de término, al igual que los colores de la misma
        /// </summary>
        /// <param name="fechaInicial">La fecha inicial (término)</param>
        /// <param name="fechaActual">La fecha con la que se comparará la fecha inicial</param>
        /// <param name="mensaje">Variable donde se almacenará el texto de vuelta</param>
        /// <param name="rojo">Variable donde se almacenará el color rojo (RGB)</param>
        /// <param name="verde">Variable donde se almacenará el color verde (RGB)</param>
        /// <param name="azul">Variable donde se almacenará el color azul (RGB)</param>
        /// <param name="semaforo">Variable donde se almacenará el número del semáforo (-1 rojo, 0 amarillo y 1 verde)</param>
        /// <returns>
        /// 2 - La diferencia fue en días, 1 - La diferencia fue en horas, 0 - Se pasó de tiempo, -1 el proceso no se llevó a cabo
        /// </returns>
        public void obtenerTextoTiempoTermino(DateTime fechaInicial, out string mensaje, out int rojo, out int verde, out int azul, out int semaforo)
        {
            DateTime fechaActual = DateTime.Now;
            DateTime manana = fechaSinFestivoNiFinDeSemana(fechaActual.AddDays(1));
            DateTime tresDias = fechaSinFestivoNiFinDeSemana(manana.AddDays(2));
            if (fechaInicial != null)
            {
                mensaje = "QUEDAN ";
                TimeSpan diferencia = fechaInicial - fechaActual;
                if (diferencia.Days > 0)
                {
                    if (diferencia.Days == 1)
                        mensaje += diferencia.Days + " DÍA";
                    else
                        mensaje += diferencia.Days + " DÍAS";
                }
                else if (diferencia.Days <= 0)
                {
                    //PUEDE QUE SEAN SÓLO HORAS DE DIFERENCIA
                    if (diferencia.Hours > 0)
                    {
                        if (diferencia.Hours == 1)
                            mensaje += diferencia.Hours + " HORA";
                        else
                            mensaje += diferencia.Hours + " HORAS";
                    }
                    else
                    {
                        //PUEDE QUE LE QUEDEN UNOS MINUTOS
                        if (diferencia.Minutes > 0)
                        {
                            if (diferencia.Minutes == 1)
                                mensaje += diferencia.Minutes + " MINUTO";
                            else
                                mensaje += diferencia.Minutes + " MINUTOS";
                        }
                        else
                        {
                            //SE PASÓ DE TIEMPO
                            if (diferencia.Hours == 0)
                            {
                                mensaje = "EL TÉRMINO FINALIZÓ HACE " + Math.Abs(diferencia.Minutes) + " MINUTO(S)";
                            }
                            else if (diferencia.Hours >= -24)
                            {
                                mensaje = "EL TÉRMINO FINALIZÓ HACE " + Math.Abs(diferencia.Hours) + " HORA(S) Y " + Math.Abs(diferencia.Minutes) + " MINUTO(S)";
                            }
                            else
                                mensaje = "EL TÉRMINO FINALIZÓ HACE " + Math.Abs(diferencia.Days) + " DÍA(S) Y " + Math.Abs(diferencia.Hours) + " HORA(S)";
                        }
                    }
                }
                if (fechaInicial <= manana)
                {
                    //Está a 24 horas laborales de vencer o ya está vencido
                    rojo = 217;
                    verde = 83;
                    azul = 79;
                    semaforo = (int)SEMAFORO.ROJO;
                }
                else if (fechaInicial > manana && fechaInicial <= tresDias)
                {
                    //Está entre 24 y 72 horas laborales de vencer
                    rojo = 240;
                    verde = 173;
                    azul = 78;
                    semaforo = (int)SEMAFORO.AMARILLO;
                }
                else if (fechaInicial > tresDias)
                {
                    //Está a más de 72 horas laborales de vencer
                    rojo = 121;
                    verde = 150;
                    azul = 61;
                    semaforo = (int)SEMAFORO.VERDE;
                }
                else
                {
                    rojo = 0;
                    verde = 0;
                    azul = 0;
                    semaforo = (int)SEMAFORO.VACÍO;
                }
            }
            else
            {
                rojo = 0;
                verde = 0;
                azul = 0;
                semaforo = (int)SEMAFORO.VACÍO;
                mensaje = "";
            }
        }
        /// <summary>
        /// Función simple que suma el límite de días para la respuesta del expediente cuando se selecciona que no se requiere una fecha de término.
        /// </summary>
        /// <param name="fecha">La fecha de recepción del documento</param>
        /// <returns>
        /// La nueva fecha límite para la respuesta
        /// </returns>
        public DateTime fechaSinTermino(DateTime fecha)
        {
            //HASTA EL MOMENTO NO SE CONTEMPLA SI CAE O NO EN FIN DE SEMANA O FESTIVO. QUIZÁ TE VEA POR AQUÍ PARA IMPLEMENTARLO MÁS TEMPRANO QUE TARDE XD (03/03/17).
            //fecha = fecha.AddDays(45);
            //BUENO, AL FINAL COMENTARON QUE SON DÍAS LABORALES...ASÍ QUE
            int numDias = diasConFinesDeSemana(fecha, fecha.AddDays(45));
            fecha = fecha.AddDays(numDias);
            return fecha;
        }
    }
}