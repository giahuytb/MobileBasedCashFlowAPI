﻿namespace MobileBaseCashFlowGameAPI.ViewModels
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = true;

    }
}
