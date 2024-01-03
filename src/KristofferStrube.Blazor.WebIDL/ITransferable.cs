namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Transferable objects support being transferred across agents.
/// Transferring is effectively recreating the object while sharing a reference to the underlying data and then detaching the object being transferred.
/// </summary>
/// <remarks><see href="https://html.spec.whatwg.org/multipage/structured-data.html#transferable-objects">See the API definition here</see>.</remarks>
public interface ITransferable : IJSWrapper
{
}
