namespace ModelLibrary.Enums;

/// <summary>
/// Specifies the different roles a user can have within the organization.
/// </summary>
public enum Role
{
    /// <summary>
    /// Represents a regular employee with base level privileges.
    /// </summary>
    Employee = 1,

    /// <summary>
    /// Represents a leader, who has elevated permissions over employees.
    /// </summary>
    Leader,

    /// <summary>
    /// Represents a director, who has higher-level administrative permissions.
    /// </summary>
    Director
}