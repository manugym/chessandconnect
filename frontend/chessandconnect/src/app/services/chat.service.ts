import { Injectable, Type } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { Subscription } from 'rxjs';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { Chat } from '../models/dto/chat';
import { MatchMakingService } from './match-making.service';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { User_Chat } from '../models/dto/user-chat';
import { GameService } from './game.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  messageReceived$: Subscription;

  messages: User_Chat[] = []

  constructor(
    public webSocketService: WebsocketService,
    private gameService: GameService

  ) {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );
  }

  public async SendMessage(m: string) {
    const chat: Chat = {
      Message: m
    }
    const message: SocketMessageGeneric<Chat> = {
      Type: SocketCommunicationType.CHAT,
      Data: chat
    }

    this.webSocketService.sendRxjs(JSON.stringify(message))

  }

  private async readMessage(message: string): Promise<void> {

    try {
      // Paso del mensaje a objeto
      const parsedMessage = JSON.parse(message);

      const socketMessage = new SocketMessageGeneric<any>();
      socketMessage.Type = parsedMessage.Type as SocketCommunicationType;
      socketMessage.Data = parsedMessage.Data;


      this.handleSocketMessage(socketMessage);
    } catch (error) {
      console.error('Error al parsear el mensaje recibido:', error);
    }
  }

  private async handleSocketMessage(message: SocketMessageGeneric<any>): Promise<void> {


    switch (message.Type) {
      case SocketCommunicationType.CHAT:

        const user_chat: User_Chat = {
          userId: this.gameService.opponent.id,
          Message: message.Data.Message
        }

        this.messages.push(user_chat)
        break

    }

  }
}
