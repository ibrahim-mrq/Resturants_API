﻿using System.ComponentModel.DataAnnotations;

namespace Resturants.DTO.Requests
{
    public class LoginRequest
    {
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
