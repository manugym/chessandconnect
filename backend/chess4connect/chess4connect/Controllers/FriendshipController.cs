﻿using chess4connect.Mappers;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private FriendshipService _friendshipService;
        private FriendMapper _friendMapper;
        private UnitOfWork _unitOfWork;

        public FriendshipController(FriendshipService friendshipService, FriendMapper friendMapper, UnitOfWork unitOfWork)
        {
            _friendshipService = friendshipService;
            _friendMapper = friendMapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet ("user")]
        public async Task<IEnumerable<FriendDto>> getUser(string nickName)
        {
            IEnumerable<User> users = await _unitOfWork.UserRepository.GetUsersByUserName(nickName);

            IEnumerable<FriendDto> usersDtos = _friendMapper.ToDto(users);

            return usersDtos;
        }

        [Authorize]
        [HttpGet("friends")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetAllFriends()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            List<User> friends = await _friendshipService.GetAllUserFriends(userIdInt);
        
            IEnumerable<FriendDto> friendDtos = _friendMapper.ToDto(friends);

            return Ok(friendDtos);
        }

        [Authorize]
        [HttpPost ("request")]
        public async Task<ActionResult<Friendship>> requestFriendship (string friendNickname)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            Friendship friendship = await _friendshipService.requestFriendship(userIdInt, friendNickname);
           
            return friendship;
        }

        [Authorize]
        [HttpPost ("accept")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> acceptRequest (string friendNickname)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }
            List<User> friends = await _friendshipService.acceptFriendship(userIdInt, friendNickname);

            IEnumerable<FriendDto> friendDtos = _friendMapper.ToDto(friends);

            return Ok(friendDtos);

        }

    }

}
