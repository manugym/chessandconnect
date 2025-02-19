import { Injectable } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { AuthService } from './auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ConnectPiece } from '../models/Games/Connect/connect-piece';
import { Subscription } from 'rxjs';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { ConnectBoard } from '../models/Games/Connect/connect-board';
import { GameService } from './game.service';
import { PieceColor } from '../models/Games/Chess/Enums/Color';

@Injectable({
  providedIn: 'root'
})
export class ConnectService {

  messageReceived$: Subscription;


  pieces: ConnectPiece[]

  constructor(
    public webSocketService: WebsocketService,
    private authService: AuthService,
    private dialog: MatDialog,
    public gameService: GameService
  ) {
    
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );
    
  }


  private async readMessage(message: string): Promise<void> {
    console.log('Masage:', message);

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

    console.log("BOARD:", message)

    switch (message.Type) {
      case SocketCommunicationType.CONNECT_BOARD:

        const board = message.Data as ConnectBoard;
        console.log("CONNECT BOARD", board)


        this.pieces = board.Pieces
        this.gameService.turn = board.Turn
        this.gameService.currentPlayerTimer = this.gameService.playerColor == PieceColor.YELLOW ? board.Player1Time : board.Player2Time
        this.gameService.opponentTimer = this.gameService.playerColor == PieceColor.YELLOW ? board.Player2Time : board.Player1Time

        this.gameService.startCountdown();

        break

    }


  }
}