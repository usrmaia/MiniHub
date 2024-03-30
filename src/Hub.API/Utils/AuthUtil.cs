using Hub.Domain.Exceptions;
using System.Net;

namespace Hub.API.Utils;

public class AuthUtil
{
    public static string ExtractTokenFromHeader(string auth)
    {
        string[] headerParts = auth.Split(' ');

        if (headerParts.Length != 2 || !headerParts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            throw new AppException("Cabeçalho de autorização inválido!", HttpStatusCode.BadRequest);

        return headerParts[1];
    }
}
