﻿using chess4connect.Models.Database.Entities;
using Microsoft.AspNetCore.Authorization;

namespace chess4connect.Services
{
    public class FriendshipService
    {
        private readonly UnitOfWork _unitOfWork;

        public FriendshipService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetAllUsersByNickname(string nickName)
        {
            return await _unitOfWork.UserRepository.GetUsersByUserName(nickName);
        }

        public async Task<List<User>> GetAllUserFriends(int userId)
        {
            User user = await _unitOfWork.UserRepository.GetUserById(userId);

            return user.Friends.ToList();
            
        }

        public async Task<Friendship> requestFriendship(int userId, int friendId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            User friend = await _unitOfWork.UserRepository.GetByIdAsync(friendId);


            Friendship request = new Friendship
            {
                UserId = user.Id,
                FriendId = friend.Id,
                State = Enums.FriendshipState.Pending
            };

            await _unitOfWork.FriendshipRepository.InsertAsync(request);

            
            await _unitOfWork.SaveAsync();
            return request;
        }

        public async Task<List<Friendship>> GetAllRequestsByUserId (int userId)
        {
            return await _unitOfWork.FriendshipRepository.GetAllPendingFriendshipsByUserId(userId);
        }

        public async Task<List<User>> acceptFriendship(int userId, int friendId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            User friend = await _unitOfWork.UserRepository.GetByIdAsync(friendId);

            Friendship pendingFriendship = await _unitOfWork.FriendshipRepository.GetFriendshipRequestedByUser(friend.Id, user.Id);

            if (pendingFriendship == null)
            {
                throw new InvalidOperationException("No se encontró la solicitud de amistad.");
            }

            pendingFriendship.State = Enums.FriendshipState.Accepted;
            _unitOfWork.FriendshipRepository.Update(pendingFriendship);

            user.Friends.Add(friend);
            friend.Friends.Add(user);
            
            await _unitOfWork.SaveAsync();

            return user.Friends;
        }
    
        public async Task DeleteFriendshipRequest(int userId, int friendId)
        {
            Friendship friendship = await _unitOfWork.FriendshipRepository.GetFriendshipRequestedByUser(userId, friendId);

            if (friendship != null)
            {
                _unitOfWork.FriendshipRepository.Delete(friendship);
            }

            await _unitOfWork.SaveAsync();
        }
    
    
    }
}
