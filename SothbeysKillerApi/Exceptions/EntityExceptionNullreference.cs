namespace SothbeysKillerApi.Exceptions;

public class EntityExceptionNullreference(string entity) : Exception
{
    public string Entity { get; } = entity;
}