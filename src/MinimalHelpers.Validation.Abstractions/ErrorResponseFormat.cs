namespace MinimalHelpers.Validation;

/// <summary>
/// Specifies the format of the error response returned by validation filters.
/// </summary>
/// <remarks>
/// Use <see cref="Default"/> to return errors as a dictionary, or <see cref="List"/> to return errors as a flat list. See <see href="https://github.com/marcominerva/MinimalHelpers">project documentation</see> for more details.
/// </remarks>
public enum ErrorResponseFormat
{
    /// <summary>
    /// The default error response format as a dictionary of field names to error messages.
    /// </summary>
    Default,

    /// <summary>
    /// The error response format as a flat list of error messages.
    /// </summary>
    List
}
