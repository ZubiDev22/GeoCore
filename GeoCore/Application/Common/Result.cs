namespace GeoCore.Application.Common;

public class ResultError
{
    public string Code { get; }
    public string Message { get; }

    public ResultError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}

public class ValidationError : ResultError
{
    public ValidationError(string message) : base("ValidationError", message) { }
}

public class NotFoundError : ResultError
{
    public NotFoundError(string message) : base("NotFound", message) { }
}

public class BusinessRuleError : ResultError
{
    public BusinessRuleError(string message) : base("BusinessRuleViolation", message) { }
}

public class DataAccessError : ResultError
{
    public DataAccessError(string message) : base("DataAccessError", message) { }
}

public class UnexpectedError : ResultError
{
    public UnexpectedError(string message) : base("UnexpectedError", message) { }
}

public class Result
{
    public bool IsSuccess { get; }
    public ResultError? Error { get; }
    public static Result Success() => new Result(true, null);
    public static Result Failure(ResultError error) => new Result(false, error);
    protected Result(bool isSuccess, ResultError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
}

public class Result<T> : Result
{
    public T? Value { get; }
    public static Result<T> Success(T value) => new Result<T>(true, value, null);
    public static new Result<T> Failure(ResultError error) => new Result<T>(false, default, error);
    private Result(bool isSuccess, T? value, ResultError? error) : base(isSuccess, error)
    {
        Value = value;
    }
}
