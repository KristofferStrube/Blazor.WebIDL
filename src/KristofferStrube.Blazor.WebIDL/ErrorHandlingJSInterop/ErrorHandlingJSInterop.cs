using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// The base class for all Error Handling JSInterop in Blazor.WebIDL.
/// </summary>
public abstract class ErrorHandlingJSInterop
{
    internal static IJSObjectReference? Helper;

    /// <summary>
    /// Indicated whether the `ErrorHandlingJSInterop` has been setup.
    /// </summary>
    public static bool ErrorHandlingJSInteropHasBeenSetup => Helper is not null;

    /// <summary>
    /// A dictionary that maps from error names to a creator method that takes the name, message, stack trace, and inner exception and creates a new <see cref="WebIDLException"/>. Can be used to add handlers for additional JS error types.
    /// <br />
    /// The default value is <see cref="ErrorMappers.Default"/>.
    /// </summary>
    public Dictionary<string, Func<JSError, WebIDLException>> ErrorMapper { get; set; } = new(ErrorMappers.Default);

    /// <summary>
    /// A property that can be used to get extra properties from all errors.
    /// </summary>
    public string[]? ExtraErrorProperties { get; set; }

    /// <summary>
    /// Unpacks the custom structure that we have packaged the exception in.
    /// </summary>
    /// <param name="exception">The exception that should contain the name, message, and optional stack trace of the error in a JSON format in its <see cref="Exception.Message"/>.</param>
    /// <returns>Returns a <see cref="JSError"/> that contains the name, message, and stack. If the exception message was not in the right format it returns null instead.</returns>
    internal static JSError? UnpackMessageOfExeption(JSException exception)
    {
        if (exception.Message.Length < 10)
        {
            return null;
        }
        try
        {
            JSError? jSError = Deserialize<JSError?>(exception.Message[..^9].Trim());
            if (jSError is not null)
            {
                jSError.InnerException = exception;
            }
            return jSError;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// Maps a given name, message and JSException to a typed <see cref="WebIDLException"/>.
    /// </summary>
    /// <param name="error">The details of the original error.</param>
    /// <param name="exception">The exception that will be parsed as the inner exception for the returned exception.</param>
    /// <returns>Returns one of the types that derive from the <see cref="WebIDLException"/> type or returns a <see cref="WebIDLException"/> if the type was not one of the supported types.</returns>
    internal WebIDLException MapToWebIDLException(JSError error, JSException exception)
    {
        if (ErrorMapper.TryGetValue(error.Name, out Func<JSError, WebIDLException>? creator))
        {
            return creator(error);
        }
        else
        {
            return new WebIDLException($"{error.Name}: \"{error.Message}\"", error.Stack, exception);
        }
    }

    /// <summary>
    /// Changes the returned value to a <see cref="IErrorHandlingJSInProcessObjectReference"/> if the given <paramref name="value"/> is a <see cref="IJSInProcessObjectReference"/>
    /// <br />
    /// or changes the returned value to a <see cref="IErrorHandlingJSObjectReference"/> if the given <paramref name="value"/> is a <see cref="IJSObjectReference"/>.
    /// </summary>
    /// <typeparam name="TValue">The return type.</typeparam>
    /// <param name="value">The returned value.</param>
    internal static TValue ConstructErrorHandlingInstanceIfJSInProcessObjectReference<TValue>(TValue value)
    {
        if (value is IJSInProcessObjectReference jSInProcessReference)
        {
            ErrorHandlingJSInProcessObjectReference errorHandlingResult = new(jSInProcessReference);
            if (errorHandlingResult is TValue matchingTValue)
            {
                return matchingTValue;
            }
        }
        return value;
    }

    /// <summary>
    /// Changes the returned value to a <see cref="IErrorHandlingJSInProcessObjectReference"/> if the given <paramref name="value"/> is a <see cref="IJSInProcessObjectReference"/>
    /// <br />
    /// or changes the returned value to a <see cref="IErrorHandlingJSObjectReference"/> if the given <paramref name="value"/> is a <see cref="IJSObjectReference"/>.
    /// </summary>
    /// <typeparam name="TValue">The return type.</typeparam>
    /// <param name="value">The returned value.</param>
    /// <param name="jSRuntime">The <see cref="IJSRuntime"/>.</param>
    internal static TValue ConstructErrorHandlingInstanceIfJSObjectReference<TValue>(IJSRuntime jSRuntime, TValue value)
    {
        if (value is IJSObjectReference jSReference)
        {
            ErrorHandlingJSObjectReference errorHandlingResult = new(jSRuntime, jSReference);
            if (errorHandlingResult is TValue matchingTValue)
            {
                return matchingTValue;
            }
        }
        return value;
    }
}
