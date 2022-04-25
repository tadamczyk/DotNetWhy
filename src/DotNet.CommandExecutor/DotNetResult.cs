namespace DotNet.CommandExecutor;

/// <summary>
/// The result model of the DotNet process performed.
/// </summary>
public sealed class DotNetResult
{
    /// <summary>
    /// An output received as a result of the DotNet process performed.
    /// </summary>
    public string Output { get; }

    /// <summary>
    /// An errors received as a result of the DotNet process performed.
    /// </summary>
    public string Errors { get; }

    /// <summary>
    /// A boolean value indicating if the DotNet process performed was successful.
    /// </summary>
    public bool IsSuccess => Status is (int) Enums.Status.Success;

    private int Status { get; }

    private DotNetResult(string output, string errors, int status) =>
        (Output, Errors, Status) = (output, errors, status);

    internal static DotNetResult Create(string output, string errors, int status) =>
        new(output, errors, status);
}