﻿using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Models.Games.Connect;

public class ConnectRoom: BaseRoom
{
    public ConnectGame Game { get; set; }

    public ConnectRoom(WebSocketHandler player1Handler, WebSocketHandler player2Handler, ConnectGame game): base(player1Handler, player2Handler)
    {
        Player1Handler = player1Handler;
        Player2Handler = player2Handler;

        Game = game;
    }


    public async Task DropConnectPiece(int column)
    {
        int response = Game.Board.DropPiece(column);

        if (response == 0)
        {
            await SendBoard();

        }

        if (response == 1)
        {
            await SendWinMessage();

        }


    }

    public override async Task SendBoard()
    {
        //Lista de piezas original
        List<ConnectPiece> pieces = Game.Board.convertBoardToList();

        //Lista de piezas sin  los movimientos básicos
        var roomMessage = new SocketMessage<ConnectBoardDto>
        {
            Type = SocketCommunicationType.CONNECT_BOARD,

            Data = new ConnectBoardDto
            {
                Pieces = pieces,
                Player1Turn = Game.Board.Player1Turn,
                Player1Time = (int)Game.Board.Player1Time.TotalSeconds,
                Player2Time = (int)Game.Board.Player2Time.TotalSeconds,

            }
        };

        string stringBoardMessage = JsonSerializer.Serialize(roomMessage);
        await SendMessage(stringBoardMessage);
    }

    public async Task SendConnectRoom()
    {
        await SendRoom(GameType.Connect4);
    }


    public override async Task SendWinMessage()
    {
        int winnerId = Game.Board.Player1Turn ? Player1Handler.Id : Player2Handler.Id;


        //Mensaje con el id del ganador
        var winnerMessage = new SocketMessage<int>
        {
            Type = SocketCommunicationType.END_GAME,

            Data = winnerId,
        };

        string stringWinnerMessage = JsonSerializer.Serialize(winnerMessage);

        await SendMessage(stringWinnerMessage);
    }


    public override async Task MessageHandler(string message)
    {
        SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);

        switch (recived.Type)
        {
            case SocketCommunicationType.CONNECT4_MOVEMENTS:
                ConnectDropPieceRequest request = JsonSerializer.Deserialize<SocketMessage<ConnectDropPieceRequest>>(message).Data;

                await DropConnectPiece(request.Column);

                break;


        }
    }

    public override async Task SaveGame(IServiceProvider serviceProvider, GameResult gameResult)
    {
        using var scope = serviceProvider.CreateAsyncScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

        Play play = new Play
        {
            StartDate = Game.StartDate,
            EndDate = DateTime.Now,
            Game = GameType.Connect4,
        };

        unitOfWork.PlayRepository.Add(play);

        await unitOfWork.SaveAsync();

        PlayDetail playDetailUser1 = new PlayDetail
        {
            PlayId = play.Id,
            UserId = Game.Board.Player1Turn ? Player1Handler.Id : Player2Handler.Id,
            GameResult = gameResult
        };

        PlayDetail playDetailUser2 = new PlayDetail
        {
            PlayId = play.Id,
            UserId = Game.Board.Player1Turn ? Player2Handler.Id : Player1Handler.Id,
            GameResult = gameResult == GameResult.DRAW ? gameResult : GameResult.LOSE
        };

        unitOfWork.PlayDetailRepository.Add(playDetailUser1);
        unitOfWork.PlayDetailRepository.Add(playDetailUser2);
        await unitOfWork.SaveAsync();


        await SendWinMessage();
    }
}
