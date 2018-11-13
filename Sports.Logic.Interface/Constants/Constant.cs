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
        public const string INVALID_COMPETITOR_NULL_VALUE_MESSAGE = "Cannot add null competitor";
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

    public static class CompetitorNotFound
    {
        public const string COMPETITOR_ID_NOT_FOUND_MESSAGE = "Id does not match any existing competitors";
    }

    public static class FavoriteNotFound
    {
        public const string FAVORITE_NOT_FOUND_MESSAGE = "Favorite not exist";
    }
    public static class SessionValidation
    {
        public const string TOKEN_NOT_EXIST_MESSAGE = "Session token does not exist";
    }
    public static class MatchValidation
    {
        public const string COMPETITOR_ALREADY_PLAYING = "Competitor already plays that day.";
        public const string COMPETITOR_DOESNT_PLAY = "Competitor has no matches";
        public const string SPORT_DIDNT_PLAY = "Sport has no finished matches";
    }


    public static class UniqueSport
    {
        public const string DUPLICATE_SPORT_MESSAGE = "Sport already exist";
    }

    public static class UniqueFavorite
    {
        public const string UNIQUE_FAVORITE_MESSAGE = "Favorite already exist";
    }

    public static class UniqueCompetitor
    {
        public const string DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE = "Competitor already in sport";
    }

    public static class UniqueUsername
    {
        public const string DUPLICATE_USERNAME_MESSAGE = "Username already exist";
    }

    public static class AdminException
    {
        public const string NON_ADMIN_EXCEPTION_MESSAGE = "You are not an admin.";
    }

    public static class AccessValidation
    {
        public const string INVALID_ACCESS_MESSAGE = "Could not reach database.";
        public const string UNKNOWN_ERROR_MESSAGE = "An unexpected error ocuured.";
    }

    public static class FixtureValidation
    {
        public const string INVALID_FIXTURE_PATH = "Invalid fixture implementation path";
        public const string MISSING_FIXTURE_STRATEGIES = "No strategies are imported";
        public const string FAILING_FIXTURE_STRATEGY = "Fixture generation strategy is failing";
        public const string EMPTY_SPORTS = "List of sports is empty";
    }
}
