using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Constants
{
    public static class NullValue
    {
        public const string INVALID_COMMENT_NULL_VALUE_MESSAGE = "Cannot add null comment";
        public const string INVALID_MATCH_NULL_VALUE_MESSAGE = "Cannot add null match";
        public const string INVALID_SPORT_NULL_VALUE_MESSAGE = "Cannot add null sport";
        public const string INVALID_TEAM_NULL_VALUE_MESSAGE = "Cannot add null team";
        public const string INVALID_USER_NULL_VALUE_MESSAGE = "Cannot add null user";
        public const string INVALID_NO_TOKEN_MESSAGE = "Token invalid or missing, must set user session before using logic";
    }

    public static class MatchId
    {
        public const string MATCH_ID_NOT_EXIST_MESSAGE = "Match id does not exist";
    }

    public static class UserNotFound
    {
        public const string USER_NOT_FOUND_MESSAGE = "User deleted or not exist";
        public const string USER_ID_NOT_FOUND_MESSAGE = "User Id does not exist";
    }

    public static class SportNotFound
    {
        public const string SPORT_NOT_FOUND_MESSAGE = "Id does not match any existing sports";
    }

    public static class TeamNotFound
    {
        public const string TEAM_ID_NOT_FOUND_MESSAGE = "Id does not match any existing teams";
    }

    public static class FavoriteNotFound
    {
        public const string FAVORITE_NOT_FOUND_MESSAGE = "Favorite not exist";
    }
    public static class SessionValidation
    {
        public const string TOKEN_NOT_EXIST_MESSAGE = "Session token does not exist";
    }

    public static class UniqueSport
    {
        public const string DUPLICATE_SPORT_MESSAGE = "Sport already exist";
    }

    public static class UniqueFavorite
    {
        public const string UNIQUE_FAVORITE_MESSAGE = "Favorite already exist";
    }

    public static class UniqueTeam
    {
        public const string DUPLICATE_TEAM_IN_SPORT_MESSAGE = "Team already in sport";
    }

    public static class UniqueUsername
    {
        public const string DUPLICATE_USERNAME_MESSAGE = "Username already exist";
    }

    public static class AdminException
    {
        public const string NON_ADMIN_EXCEPTION_MESSAGE = "You are not an admin.";
    }


}
