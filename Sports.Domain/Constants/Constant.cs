using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Constants
{
   
    public static class EmptyField
    {
        public const string EMPTY_TEXT_MESSAGE = "Texto no puede ser vacio.";
        public const string EMPTY_NAME_MESSAGE = "Nombre no puede ser vacio.";
        public const string EMPTY_LASTNAME_MESSAGE = "Apellido no puede ser vacio.";
        public const string EMPTY_USERNAME_MESSAGE = "Usuario no puede ser vacio.";
        public const string EMPTY_PASSWORD_MESSAGE = "Contrasena no puede ser vacia.";
        public const string EMPTY_EMAIL_MESSAGE = "Email no puede ser vacio.";
    }

    public static class EmptySport
    {
        public const string EMPTY_SPORT_MESSAGE = "El partido debe tener un deporte.";
    }

    public static class InvalidCompetitorAmount
    {
        public const string INVALID_COMPETITORS_AMOUNT_MESSAGE = "El encuentro debe tener cuantos competidores requiera el deporte";
        public const string INVALID_ONE_COMPETITOR_MESSAGE = "El deporte debe tener al menos 2 competidores.";

    }
    public static class EmptyComment
    {
        public const string EMPTY_COMMENT = "El comentario no puede ser vacio.";
    }

    public static class EmptyUser
    {
        public const string EMPTY_USER = "Se debe incluir un usuario.";
    }
    public static class CompetitorValidation
    {
        public const string COMPETITOR_NOT_EXIST_IN_SPORT_MESSAGE = "El competidor no existe en el deporte elegido";
        public const string COMPETITOR_EMPTY = "El competidor no puede ser vacio";
        public const string COMPETITOR_INVALID_SCORE = "El puntaje del competidor no puede ser negativo";
    }

    public static class UniqueCompetitor
    {
        public const string DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE = "El competidor ya existe en el deporte elegido";
    }

    public static class CompetitorVersus
    {
        public const string INVALID_COMPETITOR_VERSUS_MESSAGE = "Los competidores de un encuentro deben ser distintos entre si";
       
    }
    

    public static class EmailUserValidation
    {
        public const string INVALID_EMAIL_FORMAT_MESSAGE = "Formato de email invalido.";
    }

    public static class AuthenticationValidation
    {
        public const string INVALID_PASSWORD_MESSAGE = "Contrasena invalida.";
    }

    public static class MatchDateFormat
    {
        public const string INVALID_DATE_FORMAT_MESSAGE = "No se pueden crear partidos en fechas pasadas";
    }
}
