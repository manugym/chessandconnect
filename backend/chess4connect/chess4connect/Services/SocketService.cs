﻿using chess4connect.Enums;
using chess4connect.Models;
using chess4connect.Models.SocketComunication;
using System.Text.Json;

namespace chess4connect.Services;

public class SocketService
{

    public async Task ManageMessage(string message)
    {
        //Paso a Json
        var messageJson = JsonSerializer.Deserialize<SocketMessage<User>>(message);

        //Obtiene el tipo 

        SocketComunicationType type = messageJson.Type;

        //Lo gestiona su respectivo servicio
        switch (type)
        {
            case SocketComunicationType.GAME:
                break;

            case SocketComunicationType.CHAT:
                break;

            case SocketComunicationType.FRIEND:
                break;
        }

    }
}
