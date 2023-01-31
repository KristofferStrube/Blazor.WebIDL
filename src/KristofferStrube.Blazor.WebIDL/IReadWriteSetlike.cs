namespace KristofferStrube.Blazor.WebIDL;

public interface IReadWriteSetlike<T> : IReadonlySetlike<T> where T : IJSWrapper
{
}
