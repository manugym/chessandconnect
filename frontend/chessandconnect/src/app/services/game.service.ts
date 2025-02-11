import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { BasePiece } from '../models/Games/Base/BasePiece';

@Injectable({
  providedIn: 'root'
})
export class GameService {


  isHost = false
  opponent: User
  pieces: BasePiece[]

  constructor() { }
}
