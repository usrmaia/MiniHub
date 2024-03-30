﻿namespace Hub.API.InputModels;

public class UserRoleIM
{
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}

public class UpdatePasswordIM
{
    public string OldPassword { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
