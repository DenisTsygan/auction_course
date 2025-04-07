namespace SothbeysKillerApi.Exceptions;

public class LotExceptionValidation(string field, string description) : Exception
{
    public string Field { get; } = field;
    public string Description { get; } = description;
}