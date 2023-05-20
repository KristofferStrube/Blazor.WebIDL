﻿using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;
using static System.Text.Json.JsonSerializer;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// 
/// </summary>
public abstract class ErrorHandlingJSInterop
{
    internal static IJSObjectReference? Helper;

    /// <summary>
    /// A dictionary that maps from Error names to a creator method that takes the name, message, and inner exception and creates a new WebIDLException. Can be used to add handlers for additional JS error types.
    /// <br />
    /// The default value is <see cref="ErrorMappers.Default"/>.
    /// </summary>
    public Dictionary<string, Func<string, string, Exception, WebIDLException>> ErrorMapper { get; set; } = ErrorMappers.Default;

    /// <summary>
    /// Unpacks the custom structure that we have packaged the exception in.
    /// </summary>
    /// <param name="exception">The exception that should contain the name and the message of the exception separated by a colon.</param>
    /// <returns>Returns the name and message of the exception. If the exception message was not in the right format it returns null instead.</returns>
    internal static Error? UnpackMessageOfExeption(JSException exception)
    {
        return Deserialize<Error>(exception.Message[..^9].Trim());
    }

    /// <summary>
    /// Maps a given name, message and JSException to a typed <see cref="WebIDLException"/>.
    /// </summary>
    /// <param name="error">The details of the original error.</param>
    /// <param name="exception">The exception that will be parsed as the inner exception for the returned exception.</param>
    /// <returns>Returns one of the types that derive from the <see cref="WebIDLException"/> type or returns a <see cref="WebIDLException"/> if the type was not one of the supported types.</returns>
    internal static WebIDLException MapToWebIDLException(Error error, JSException exception, Dictionary<string, Func<string, string, Exception, WebIDLException>> errorMapper)
    {
        if (errorMapper.TryGetValue(error.name, out Func<string, string, Exception, WebIDLException>? creator))
        {
            return creator(error.name, error.message, exception);
        }
        else
        {
            return new WebIDLException($"{error.name}: \"{error.message}\"", exception);
        }
    }

    /// <summary>
    /// Changes the returned value to a <see cref="IErrorHandlingJSInProcessObjectReference"/> if the given <paramref name="value"/> is a <see cref="IJSInProcessObjectReference"/>
    /// <br />
    /// or changes the returned value to a <see cref="IErrorHandlingJSObjectReference"/> if the given <paramref name="value"/> is a <see cref="IJSObjectReference"/>.
    /// </summary>
    /// <typeparam name="TValue">The return type.</typeparam>
    /// <param name="value">The returned value.</param>
    internal static TValue ConstructErrorHandlingInstanceIfJSObjectReference<TValue>(TValue value)
    {
        if (value is IJSInProcessObjectReference jSInProcessReference)
        {
            ErrorHandlingJSInProcessObjectReference errorHandlingResult = new ErrorHandlingJSInProcessObjectReference(jSInProcessReference);
            if (errorHandlingResult is TValue matchingTValue)
            {
                return matchingTValue;
            }
        }
        else if (value is IJSObjectReference jSReference)
        {
            ErrorHandlingJSObjectReference errorHandlingResult = new ErrorHandlingJSObjectReference(jSReference);
            if (errorHandlingResult is TValue matchingTValue)
            {
                return matchingTValue;
            }
        }
        return value;
    }

    internal record Error(string name, string message);
}