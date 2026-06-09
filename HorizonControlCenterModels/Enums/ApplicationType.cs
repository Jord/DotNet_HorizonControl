using System;
using System.Collections.Generic;
using System.Linq;

namespace HorizonControlCenterModels.Enums
{
    /// <summary>
    /// Defines the types of applications in the suite
    /// </summary>
    public static class ApplicationType
    {
        /// <summary>
        /// Timesheet application type
        /// </summary>
        public const string Timesheets = "timesheets";

        /// <summary>
        /// Document Management System application type
        /// </summary>
        public const string DMS = "dms";

        /// <summary>
        /// Globals application type
        /// </summary>
        public const string Globals = "globals";

        /// <summary>
        /// Other application types
        /// </summary>
        public const string Other = "other";

        /// <summary>
        /// Gets all available application types
        /// </summary>
        public static List<string> GetAll()
        {
            return new List<string>
            {
                Timesheets,
                DMS,
                Globals,
                Other
            };
        }

        /// <summary>
        /// Validates if the given value is a valid application type
        /// </summary>
        public static bool IsValid(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return GetAll().Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Normalizes the application type to lowercase
        /// </summary>
        public static string? Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var match = GetAll().FirstOrDefault(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
            return match;
        }

        /// <summary>
        /// Tries to infer application type from application name
        /// </summary>
        public static string? InferFromName(string? applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                return null;

            var match = GetAll().FirstOrDefault(x => 
                applicationName.Contains(x, StringComparison.OrdinalIgnoreCase));
            return match;
        }
    }
}
