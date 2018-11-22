using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Constants
{
    public static class NullValue
    {
        public const string INVALID_COMMENT_NULL_VALUE_MESSAGE = "No se puede agregar un comentario null";
        public const string INVALID_MATCH_NULL_VALUE_MESSAGE = "No se puede agregar un encuentro null";
        public const string INVALID_SPORT_NULL_VALUE_MESSAGE = "No se puede agregar un deporte null";
        public const string INVALID_COMPETITOR_NULL_VALUE_MESSAGE = "No se puede agregar un competidor null";
        public const string INVALID_USER_NULL_VALUE_MESSAGE = "No se puede agregar un usuario null";
        public const string INVALID_NO_TOKEN_MESSAGE = "Token invalido o inexistente, debes iniciar sesion";
    }

    public static class MatchId
    {
        public const string MATCH_ID_NOT_EXIST_MESSAGE = "Partido invalido o inexistente seleccionado";
    }

    public static class UserNotFound
    {
        public const string USER_NOT_FOUND_MESSAGE = "Usuario no encontrado";
        public const string USER_ID_NOT_FOUND_MESSAGE = "Usuario invalido o inexistente seleccionado";
    }

    public static class SportNotFound
    {
        public const string SPORT_NOT_FOUND_MESSAGE = "Deporte invalido o inexistente seleccionado";
    }

    public static class CompetitorNotFound
    {
        public const string COMPETITOR_ID_NOT_FOUND_MESSAGE = "Competidor invalido o inexistente seleccionado";
    }

    public static class FavoriteNotFound
    {
        public const string FAVORITE_NOT_FOUND_MESSAGE = "El usuario no tiene competidores favoritos";
    }
    public static class SessionValidation
    {
        public const string TOKEN_NOT_EXIST_MESSAGE = "El token de sesion no existe";
    }
    public static class MatchValidation
    {
        public const string COMPETITOR_ALREADY_PLAYING = "Partido invalido, el competidor ya juega ese dia.";
        public const string COMPETITOR_DOESNT_PLAY = "El competidor no tiene partidos jugados o por jugar";
        public const string SPORT_DIDNT_PLAY = "El deporte no tiene encuentros finalizados";
    }


    public static class UniqueSport
    {
        public const string DUPLICATE_SPORT_MESSAGE = "El deporte ya existe";
    }

    public static class UniqueFavorite
    {
        public const string UNIQUE_FAVORITE_MESSAGE = "El competidor ya fue marcado como favorito";
    }

    public static class UniqueCompetitor
    {
        public const string DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE = "El competidor ya existe en el deporte";
    }

    public static class UniqueUsername
    {
        public const string DUPLICATE_USERNAME_MESSAGE = "El nombre de usuario ya existe";
    }

    public static class AdminException
    {
        public const string NON_ADMIN_EXCEPTION_MESSAGE = "Permisos invalidos. No eres administrador";
    }

    public static class AccessValidation
    {
        public const string INVALID_ACCESS_MESSAGE = "No se pudo acceder a la base de datos";
        public const string UNKNOWN_ERROR_MESSAGE = "Ocurrio un error inesperado";
    }

    public static class FixtureValidation
    {
        public const string INVALID_FIXTURE_PATH = "Ruta de implementacion de fixtures invalida";
        public const string MISSING_FIXTURE_STRATEGIES = "No hay implementaciones de fixtures cargadas en el sistema";
        public const string FAILING_FIXTURE_STRATEGY = "La implementacion de fixture es defectuosa";
        public const string EMPTY_SPORTS = "El deporte no existe";
    }
}
