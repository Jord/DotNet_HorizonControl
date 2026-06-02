using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterModels.Enums
{
    /// <summary>
    /// Defines the types of applications in the suite
    /// </summary>
    public enum ApplicationType
    {
        /// <summary>
        /// Timesheet application type
        /// </summary>
        Timesheets = 1,

        /// <summary>
        /// Document Management System application type
        /// </summary>
        DMS = 2,

        /// <summary>
        /// Globals application type
        /// </summary>
        Globals = 3,


        /// <summary>
        /// Other application types
        /// </summary>
        Other = 99
    }

    /// <summary>
    /// Helper class for ApplicationType enum operations
    /// </summary>
    public static class ApplicationTypeHelper
    {
        /// <summary>
        /// Gets the display name for the application type
        /// </summary>
        public static string GetDisplayName(this ApplicationType applicationType)
        {
            return applicationType switch
            {
                ApplicationType.Timesheets => "Timesheets",
                ApplicationType.DMS => "DMS",
               ApplicationType.Globals => "Globals",
                ApplicationType.Other => "Other",
                _ => applicationType.ToString()
            };
        }

        /// <summary>
        /// Gets all application type display names
        /// </summary>
        public static List<string> GetAllApplicationTypes()
        {
            return Enum.GetValues(typeof(ApplicationType))
                       .Cast<ApplicationType>()
                       .Select(x => x.GetDisplayName())
                       .ToList();
        }

        /// <summary>
        /// Parses a string to ApplicationType enum
        /// </summary>
        public static ApplicationType? ParseApplicationType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Enum.TryParse<ApplicationType>(value, true, out var result))
                return result;

            return null;
        }
    }
}
