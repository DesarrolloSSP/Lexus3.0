using Lexus2._0.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Lexus2_0.Datos.Auxiliares
{
    public class AdminActuacion
    {
        //public static int IDACTUACION_RESPUESTA = 1; //RESPUESTA (LA ACTUACIÓN HA SIDO RESPONDIDA Y LA FECHA DE TÉRMINO DEJA DE TENER EFECTO)
        //public static int IDACTUACION_RESOLUCION = 2; //RESOLUCIÓN : Indicado como el último paso dentro de un proceso jurídico (el que demanda debería pasar a una instacina mayor si quiere continuar el proceso)
        public enum ACTUACIONES {
            RECEPCION = 1,
            EN_TRAMITE = 2,
            RESPUESTA_TRAMITE = 3,
            RESPUESTA = 4,
            REQUERIMIENTO_DE_RESPUESTA = 5,
            DE_CONOCIMIENTO = 6
        };

        /*PROPUESTAS DE COMPORTAMIENTO (NADIE LO HA DEJADO CLARO ¬¬)
         * 
         * A) CASO IDEAL:                   (RECEPCIÓN => RESPUESTA)
         * 1.- LLEGA UN NUEVO OFICIO, ABRE UNA FECHA DE TÉRMINO
         * 2.- SE RESPONDE EL OFICIO, SE ELIMINA LA FECHA DE TÉRMINO
         * 
         * B)  CASO ANORMAL, PERO POSIBLE:   (RECEPCIÓN => RECEPCIÓN => RESPUESTA) 
         * 1.- LLEGA UN NUEVO OFICIO, ABRE UNA FECHA DE TÉRMINO
         * 2.- COMO SIGUE SIN RESPONDERSE, LLEGA UN NUEVO OFICIO (EL PRIMERO EN LLEGAR ASENTARÍA EL TÉRMINO)
         * 3.- EN UNA SOLA RESPUESTA, SE PUEDEN PONER TODOS LOS DOCUMENTOS DE RESPUESTA.
         * 
         * C)  CASO DE TRAMITACIÓN DE AUXILIOS DE LA FUERZA PÚBLICA Ó TAFP:     (RECEPCIÓN => EN TRÁMITE => RESPUESTA DEL TRÁMITE => RESPUESTA)
         * 1.- LLEGA UN NUEVO OFICIO, ABRE UNA FECHA DE TÉRMINO
         * 2.- DEPENDIENDO DE QUÉ ASUNTO, SE TRAMITA A OTRAS ÁREAS, 'CONGELANDO' LA FECHA DE TÉRMINO (ESTO ES, AUNQUE SE HAYA VENCIDO, 
         *     NO CUENTA EN LOS INFORMES NI NOTIFICACIONES)
         * 3.- SE RECIBE LA RESPUESTA DEL ÁREA A QUIEN SE LE TRAMITÓ
         * 4.- SE RESPONDE EL OFICIO INICIAL, SE ELIMINA EL TÉRMINO.
         * 
         * D)  CASO ANORMAL, PERO POSIBLE DE TAFP            (RECEPCIÓN => EN TRÁMITE => RECEPCIÓN => REQUERIMIENTO DE RESPUESTA => 
         *     RESPUESTA DEL TRÁMITE => RESPUESTA
         * 1.- LLEGA UN NUEVO OFICIO, ABRE UNA FECHA DE TÉRMINO
         * 2.- DEPENDIENDO DE QUÉ ASUNTO, SE TRAMITA A OTRAS ÁREAS, 'CONGELANDO' LA FECHA DE TÉRMINO
         * 3.- LLEGA UN NUEVO OFICIO PUES NO HA HABIDO RESPUESTA, LA FECHA DE TÉRMINO SIGUE CONGELADA.
         * 4.- SE REDACTA UN OFICIO AL ÁREA QUE SE LE TRAMITÓ EL ORIGINAL.
         * 5.- SE RECIBE LA RESPUESTA DEL ÁREA A QUIEN SE LE TRAMITÓ
         * 6.- EN UNA SOLA RESPUESTA, SE PUEDEN PONER TODOS LOS DOCUMENTOS DE RESPUESTA DEL OFICIO INICIAL
         */

        public string getHexPorActuacion(catActuaciones actuacion)
        {
            string hexadecimal = "#000";
            if (actuacion != null && (actuacion.rojo != null && actuacion.verde != null && actuacion.azul != null))
            {
                Color color = Color.FromArgb((int)actuacion.rojo, (int)actuacion.verde, (int)actuacion.azul);
                hexadecimal = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            }
            return hexadecimal;
        }

        public string getHexResaltadoPorActuacion(catActuaciones actuacion)
        {
            string hexadecimal = "#000";
            if (actuacion != null && (actuacion.rojo != null && actuacion.verde != null && actuacion.azul != null))
            {
                double nuevoR = ((int)actuacion.rojo) * 1.3;
                if (nuevoR > 255)
                    nuevoR = 255;
                double nuevoV = ((int)actuacion.verde) * 1.3;
                if (nuevoV > 255)
                    nuevoV = 255;
                double nuevoA = ((int)actuacion.azul) * 1.3;
                if (nuevoA > 255)
                    nuevoA = 255;
                Color color = Color.FromArgb((int)nuevoR, (int)nuevoV, (int)nuevoA);
                hexadecimal = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            }
            return hexadecimal;
        }
        
    }
}