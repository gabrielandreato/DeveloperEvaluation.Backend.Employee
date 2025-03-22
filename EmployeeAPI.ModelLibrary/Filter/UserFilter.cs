using ModelLibrary.Common;
using ModelLibrary.Enums;

namespace ModelLibrary.Filter
{
    /// <summary>
    /// Represents the filter criteria used to query user data in the system.
    /// </summary>
    public class UserFilter: DefaultQueryParameter
    {
        /// <summary>
        /// Gets or sets the username to filter by.
        /// Allows filtering of users with a specific username.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address to filter by.
        /// Allows filtering of users with a specific email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the document number to filter by.
        /// Allows filtering of users with a specific document number.
        /// </summary>
        public string? DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the manager ID to filter by.
        /// Allows filtering of users managed by a specific manager.
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the user role to filter by.
        /// Allows filtering of users with a specific role in the organization.
        /// </summary>
        public Role? Role { get; set; }
    }
}