﻿using chess4connect.DTOs;
using chess4connect.Models;
using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private AuthService _authService;

    public AuthController(AuthService authService) { 
        _authService = authService;
    }


    [HttpPost("signup")]
    public async Task<string> RegisterUserAsync([FromBody] UserSignUpDto newUser)
    {
        return await _authService.RegisterUser(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] LoginDto userLogin)
    {
        User user = await _authService.GetUserByCredentialAndPassword(userLogin.Credential, userLogin.Password);
        if (user != null)
        {
            string stringToken = _authService.ObtainToken(user);
            return Ok(stringToken);
        }
        else
        {
            return Unauthorized();
        }

    }


}
