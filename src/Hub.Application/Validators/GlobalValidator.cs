using FluentValidation;

namespace Hub.Application.Validators;

public class GlobalValidator
{
    public static string RequiredField(string field) => $"{field}.Campo obrigatório.";
    public static string InvalidField(string field) => $"{field}. Campo inválido.";
    public static string MaxLength(string field, int length) => $"{field}.Tamanho/Valor máximo excedido. Máximo de {length} caracteres.";
    public static string MinLength(string field, int length) => $"{field}.Tamanho/Valor mínimo não atingido. Mínimo de {length} caracteres.";
    public static string InvalidEmailFormat() => "E-mail inválido.";
    public static string InvalidDateTimeFormat() => "Data inválida. Use o formato dd/MM/yyyy HH:mm";
    public static string InvalidDateFormat() => "Data inválida. Use o formato dd/MM/yyyy";
}

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .MinimumLength(6).WithMessage(GlobalValidator.MinLength("Senha", 6))
            .MaximumLength(16).WithMessage(GlobalValidator.MaxLength("Senha", 16))
            // Has uppercase letter, lowercase letter, number and special character
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,16}$")
                .WithMessage("Senha inválida. Use uma senha com pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.");
    }
}