import { Component, OnInit } from '@angular/core';
import { GameService } from '../../services/game.service';
import { CommonModule } from '@angular/common';
import { BasePiece } from '../../models/Games/Base/BasePiece';
import { ChessBasePiece } from '../../models/Games/Chess/ChessBasePiece';


@Component({
  selector: 'app-chess',
  imports: [CommonModule],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})

export class ChessComponent implements OnInit{

  constructor(public gameService: GameService){}

  pieces: ChessBasePiece[]

  ngOnInit(): void {
    this.pieces = this.gameService.pieces
    console.log("GAMEEEEEE:", this.gameService.pieces)
  }
  
  letters: string[] = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H']
  lettersReverse: string[] = ['H', 'G', 'F', 'E', 'D', 'C', 'B', 'A']
  numbersReverse: string [] = ['1', '2', '3', '4', '5', '6', '7', '8']
  numbers: string [] = ['8', '7', '6', '5', '4', '3', '2', '1']

  rows: number[] = [7, 6, 5, 4, 3, 2, 1, 0]
  rowsReverse: number[] = [1, 2, 3, 4, 5, 6, 7, 8]
  cols: number [] = [0, 1, 2, 3, 4, 5, 6, 7]
  colsReverse: number[] = [7, 6, 5, 4, 3, 2, 1, 0]
  cells: string[] = [];

}
