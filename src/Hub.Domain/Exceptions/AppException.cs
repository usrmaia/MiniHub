using System.Net;

namespace Hub.Domain.Exceptions;

public class AppException : Exception
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; } = DateTime.Now;
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

    public AppException() : base() { }

    public AppException(string message, HttpStatusCode statusCode) : base(message) =>
        StatusCode = statusCode;

    public AppException(string id, string message, HttpStatusCode statusCode) : base(message) =>
        (Id, StatusCode) = (id, statusCode);
}
