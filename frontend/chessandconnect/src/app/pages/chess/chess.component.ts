import { Component, OnInit } from '@angular/core';
import { GameService } from '../../services/game.service';
import { CommonModule } from '@angular/common';
import { ChessPieceColor } from '../../models/Games/Chess/Enums/Color';
import { PieceType } from '../../models/Games/Chess/Enums/PieceType';
import { ChessPiece } from '../../models/Games/Chess/ChessPiece';
import { Point } from '../../models/Games/Base/Point';
import { ChessMoveRequest } from '../../models/Games/Chess/ChessMoveRequest'
import { ApiService } from '../../services/api.service';
import { ChatComponent } from "../../components/chat/chat.component";



@Component({
  selector: 'app-chess',
  imports: [CommonModule, ChatComponent],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})

export class ChessComponent implements OnInit {

  ChessPieceColor = ChessPieceColor;

  selectedPiece: ChessPiece | null = null;

  constructor(public gameService: GameService, private api: ApiService) { }


  ngOnInit(): void {
    console.log("Opponent:", this.gameService.opponent)
    console.log("PIECES:", this.gameService.pieces)
  }





  letters: string[] = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H']
  lettersReverse: string[] = ['H', 'G', 'F', 'E', 'D', 'C', 'B', 'A']
  numbersReverse: string[] = ['1', '2', '3', '4', '5', '6', '7', '8']
  numbers: string[] = ['8', '7', '6', '5', '4', '3', '2', '1']

  rows: number[] = [7, 6, 5, 4, 3, 2, 1, 0]
  cols: number[] = [0, 1, 2, 3, 4, 5, 6, 7]
  cells: string[] = [];



  getPieceSymbol(pieceType: PieceType): string {
    switch (pieceType) {
      case PieceType.BISHOP: return '♝';
      case PieceType.KING: return '♚';
      case PieceType.KNIGHT: return '♞';
      case PieceType.PAWN: return '♙';
      case PieceType.QUEEN: return '♛';
      case PieceType.ROOK: return '♜';
      default: return '';
    }
  }

  selectPiece(piece: ChessPiece) {
    this.selectedPiece = piece;
  }

  async movePiece(destinationX: number, destinationY: number) {
    if (!this.selectedPiece) {
      return;
    }

    const moveRequest: ChessMoveRequest = { PieceId: this.selectedPiece.Id, MovementX: destinationX,  MovementY: destinationY};

    const result = await this.api.post(`Game/moveChessPiece`, moveRequest);

    if (result.success) {
      this.selectedPiece = null; 
    }

    console.log("Pieza movida: ", result);
  }






}
