﻿using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
﻿using chess4connect.Mappers;
using Microsoft.AspNetCore.Mvc;
using chess4connect.Models.SocketComunication.MessageTypes;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private UserService _userService;
        private SmartSearch _smartSearch;
        private UserMapper _userMapper;

        public UserController(UserService userService, SmartSearch smartSearch, UserMapper userMapper) 
        { 
            _userService = userService;
            _smartSearch = smartSearch;
            _userMapper = userMapper;
        }

        [HttpGet]
        public async Task<UserAfterLoginDto> GetAuthenticatedUser()
        {
            //Si no es una usuario autenticado termina la ejecución
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }

            User user = await _userService.GetUserById(Int32.Parse(userId));

            return _userMapper.ToDto(user);

        }
        
        [HttpGet("searchUser")]
        public List<UserAfterLoginDto> getAllUsers([FromQuery] string query)
        {
            List<UserAfterLoginDto> userList = _userService.GetUsers().Result;
            return _smartSearch.Search(query, userList).ToList();
        }


        [HttpGet("friends")]
        public async Task<List<FriendModel>> GetAllFriends()
        {
            //Si no es una usuario autenticado termina la ejecución
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }

            return await _userService.GetAllFriendsWithState(Int32.Parse(userId));

        }

        [HttpPost("deleteFriend")]
        public async Task DeleteFriendFriends([FromQuery] int friendId)
        {
            //Si no es una usuario autenticado termina la ejecución
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _userService.DeleteFriend(Int32.Parse(userId), friendId);


        }


    }
}
