﻿namespace Mango.Services.AuthAPI.Models.Dtos
{
    public record LoginRequestDto
    {
        public string UserName { get; init; }
        public string UserPassword { get; init; }
    }
}
