using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// A common exception class for all exceptions from the <c>Blazor.WebIDL</c> library.
/// </summary>
public partial class WebIDLException : Exception
{

    ///RWM: Source - https://github.com/MindscapeHQ/raygun4blazor/pull/25#discussion_r1694840772
    [GeneratedRegex(@"(?<functionName>.+)@(?<fileName>.+):(?<lineNumber>\d+):(?<columnNumber>\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex StackFrameRegex();

    private readonly string? jsStackTrace;

    /// <summary>
    /// Returns the stack trace as a string.
    /// </summary>
    /// <remarks>
    /// If no stack trace is available, null is returned. The stack is prepended with the JS stack trace if there is any.
    /// </remarks>
    public override string? StackTrace => 
        // @robertmclaws: Doing it this way prevents unnecessary whitespace from being added to the trace, breaking tests.
        string.IsNullOrWhiteSpace(jsStackTrace) && string.IsNullOrWhiteSpace(base.StackTrace) ?
        null :
        $"{jsStackTrace}{(!string.IsNullOrWhiteSpace(base.StackTrace) ? "\n" : null)}{base.StackTrace}";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// A <see cref="List{StackFrame}" /> containing the <see cref="StackTrace"/> parsed as a series of <see cref="StackFrame">StackFrames</see>.
    /// </returns>
    public List<StackFrame?> GetStackFrames()
    {
        if (StackTrace is null) return [];

        return StackTrace
            .Split('\n')
            .Where(frame => !string.IsNullOrWhiteSpace(frame))
            .Select(frame =>
            {
                Match match = StackFrameRegex().Match(frame);
                return !match.Success ?
                    null :
                    new StackFrame(match.Groups["fileName"].Value, int.Parse(match.Groups["lineNumber"].Value), int.Parse(match.Groups["columnNumber"].Value));
            })
            .Where(c => c is not null)
            .ToList();
    }

    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jsStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public WebIDLException(string message, string? jsStackTrace = default, Exception? innerException = null) : base(message, innerException)
    {
        this.jsStackTrace = jsStackTrace;
    }
}
