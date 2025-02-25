﻿using chess4connect.DTOs;
using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Connect;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;
using chess4connect.DTOs.Games;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.Games;

namespace chess4connect.Services
{
    public class RoomService
    {
        private readonly IServiceProvider _serviceProvider;


        private List<ChessRoom> chessRooms = new List<ChessRoom>();
        private List<ConnectRoom> connectRooms = new List<ConnectRoom>();

        public RoomService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1Hadler, WebSocketHandler player2Handler = null)
        {

            if (gamemode == GameType.Chess)
            {
                var room = new ChessRoom(player1Hadler, player2Handler,
                    new ChessGame(DateTime.Now,
                    new ChessBoard()
                    {
                        StartTurnDateTime = DateTime.Now,
                    }));

                room.Player1Id = player1Hadler.Id;

                if (player2Handler != null)
                    room.Player2Id = player2Handler.Id;

                chessRooms.Add(room);

                await room.SendChessRoom();

            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ConnectRoom(player1Hadler, player2Handler,
                   new ConnectGame(DateTime.Now,
                   new ConnectBoard()
                   {
                       StartTurnDateTime = DateTime.Now
                   }));


                room.Player1Id = player1Hadler.Id;

                if (player2Handler != null)
                    room.Player2Id = player2Handler.Id;

                connectRooms.Add(room);

                await room.SendConnectRoom();

            }

        }

        public async Task MessageHandler(int userId, string message)
        {
            SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);


            switch (recived.Type)
            {
                case SocketCommunicationType.CHAT:
                    ChessRoom chessRoom = GetChessRoomByUserId(userId);

                    if(chessRoom != null)
                    {
                        await chessRoom.SendChatMessage(message, userId);
                    }
                    else 
                    {
                        await GetConnectRoomByUserId(userId).SendChatMessage(message, userId);
                    }
                    break;

                case SocketCommunicationType.CHESS_MOVEMENTS:
                    await GetChessRoomByUserId(userId).MessageHandler(message);
                    break;

                case SocketCommunicationType.CONNECT4_MOVEMENTS:
                    await GetConnectRoomByUserId(userId).MessageHandler(message);
                    break;

                //Si el compañero se desconecta, elimina la sala y envía mensaje de victoria al usuario
                case SocketCommunicationType.CONNECTION:
                    ConnectionSocketMessage<ConnectionModel> connectioMessage = JsonSerializer.Deserialize<ConnectionSocketMessage<ConnectionModel>>(message);

                    if (connectioMessage.Data.Type == ConnectionType.Disconnected)
                    {
                        ChessRoom chessRoom1 = GetChessRoomByUserId(userId);

                        if (chessRoom1 != null)
                        {
                            await chessRoom1.SaveGame(_serviceProvider, GameResult.WIN);
                            chessRooms.Remove(chessRoom1);
                        }

                        else
                        {
                            ConnectRoom connectRoom = GetConnectRoomByUserId(userId);

                            if (connectRoom != null)
                            {
                                await connectRoom.SaveGame(_serviceProvider, GameResult.WIN);
                                connectRooms.Remove(connectRoom);
                            }

                        }
                    }

                    break;
                case SocketCommunicationType.DRAW_REQUEST:

                    ChessRoom room = GetChessRoomByUserId(userId);

                    if (room != null)
                    {
                        if (await room.NewDrawRequest())
                        {
                            await room.SaveGame(_serviceProvider,GameResult.DRAW);
                        }
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);

                        if (connectRoom != null)
                        {
                            if (await room.NewDrawRequest())
                            {
                                await room.SaveGame(_serviceProvider, GameResult.DRAW);
                            }
                        }

                    }


                    break;
                case SocketCommunicationType.REMATCH_REQUEST:

                    ChessRoom rematchRoom = GetChessRoomByUserId(userId);

                    if (rematchRoom != null)
                    {
                        if (await rematchRoom.NewRematchRequest())
                        {
                            rematchRoom.Game = new ChessGame(DateTime.Now,
                                                new ChessBoard()
                                                {
                                                    StartTurnDateTime = DateTime.Now,
                                                });


                            await rematchRoom.SendChessRoom();

                        }
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);

                        if (connectRoom != null)
                        {
                            if (await connectRoom.NewDrawRequest())
                            {
                                connectRoom.Game = new ConnectGame(DateTime.Now,
                                   new ConnectBoard()
                                   {
                                       StartTurnDateTime = DateTime.Now
                                   });

                                await connectRoom.SendConnectRoom();

                            }




                        }

                    }


                    break;

                case SocketCommunicationType.LEAVE_GAME:

                    ChessRoom leaveRoom = GetChessRoomByUserId(userId);

                    if (leaveRoom != null)
                    {
                        await leaveRoom.Surrender(userId, _serviceProvider);
                    }

                    else
                    {
                        ConnectRoom connectRoom = GetConnectRoomByUserId(userId);
                        await connectRoom.Surrender(userId, _serviceProvider);

                    }


                    break;
            }


        }



        public ChessRoom GetChessRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Handler.Id == userId || r.Player2Handler.Id == userId);
        }
        public ConnectRoom GetConnectRoomByUserId(int userId)
        {
            return connectRooms.FirstOrDefault(r => r.Player1Handler.Id == userId || r.Player2Handler.Id == userId);
        }





    }
}
