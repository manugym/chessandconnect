import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { User } from '../models/dto/user';
import { Result } from '../models/result';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  currentUser: User



  constructor(private api: ApiService) {}

  async getUser(): Promise<void> { 
    const result = await this.api.get<User>('User')
    if (!result.success) {
      this.api.handleError('Usuario no encontrado');
    }



  getSearchUsers(query: string){
    return this.api.get<User>(`User/searchUser?query=${query}`)
  }

  public handleError(message: string): void {
    Swal.fire({
      icon: 'error',
      text: message,
      showConfirmButton: true,
    });
  }
}
