using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Constants
{
   
    public static class EmptyField
    {
        public const string EMPTY_TEXT_MESSAGE = "Text cannot be empty.";
        public const string EMPTY_NAME_MESSAGE = "Name cannot be empty.";
        public const string EMPTY_LASTNAME_MESSAGE = "Last name cannot be empty.";
        public const string EMPTY_USERNAME_MESSAGE = "Username cannot be empty.";
        public const string EMPTY_PASSWORD_MESSAGE = "Password cannot be empty.";
        public const string EMPTY_EMAIL_MESSAGE = "EMail cannot be empty.";
    }

    public static class EmptySport
    {
        public const string EMPTY_SPORT_MESSAGE = "Match should include a sport.";
    }

    public static class InvalidCompetitorAmount
    {
        public const string INVALID_COMPETITORS_AMOUNT_MESSAGE = "Match should have as many competitors as the Sport requires";
        public const string INVALID_ONE_COMPETITOR_MESSAGE = "Sport should include at least two competitors.";

    }
    public static class EmptyComment
    {
        public const string EMPTY_COMMENT = "Comment to add must be valid.";
    }

    public static class EmptyUser
    {
        public const string EMPTY_USER = "User must be included.";
    }
    public static class CompetitorValidation
    {
        public const string COMPETITOR_NOT_EXIST_IN_SPORT_MESSAGE = "Competitor does not exist in Sport";
    }

    public static class UniqueCompetitor
    {
        public const string DUPLICATE_COMPETITOR_IN_SPORT_MESSAGE = "Competitor already exist in Sport";
    }

    public static class CompetitorVersus
    {
        public const string INVALID_COMPETITOR_VERSUS_MESSAGE = "Invalid Match. Competitors must be different.";
       
    }

    public static class ImageCompetitorValidation
    {
        public const string INVALID_FILE_EXTENSION_MESSAGE = "Invalid file extension";
        public const string INVALID_FILE_PATH_MESSAGE = "Invalid file path";
    }

    public static class EmailUserValidation
    {
        public const string INVALID_EMAIL_FORMAT_MESSAGE = "Invalid EMail. Format is not correct.";
    }

    public static class AuthenticationValidation
    {
        public const string INVALID_PASSWORD_MESSAGE = "Invalid password.";
    }

    public static class MatchDateFormat
    {
        public const string INVALID_DATE_FORMAT_MESSAGE = "Cant set past matches";
    }
}
