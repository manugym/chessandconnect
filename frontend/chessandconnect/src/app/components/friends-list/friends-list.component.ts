import { Component, OnInit } from '@angular/core';
import { FriendsService } from '../../services/friends.service';
import { Friend } from '../../models/dto/friend';
import { User } from '../../models/dto/user';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { FormsModule } from '@angular/forms';
import { Game } from '../../models/game';

@Component({
  selector: 'app-friends-list',
  imports: [RouterLink, FormsModule],
  templateUrl: './friends-list.component.html',
  styleUrl: './friends-list.component.css'
})
export class FriendsListComponent implements OnInit {

  searchQuery: string;
  private searchTimeout: any;

  constructor(
    public friendService: FriendsService) { }


  async ngOnInit(): Promise<void> {
    console.log("obtener amigos")
    if(!this.searchQuery){
      this.searchQuery = ""
    }
    await this.friendService.getFriends(this.searchQuery)
  }


  async deleteFriend(friendId: number) {
    //Mensaje de confirmación
    const result = await Swal.fire({
      title: '¿Estás seguro?',
      text: 'No podrás revertir esta acción.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      customClass: {
        popup: 'bg-[#301e16] text-[#E8D5B5]',
        title: 'font-bold text-lg',
        confirmButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
        cancelButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg'
      }
    });

    if (result.isConfirmed) {
      console.log("Amigo eliminado: " + friendId);
      await this.friendService.deleteFriend(friendId);
    } else {
      console.log("Eliminación cancelada")
    }
  }
  
      async onSearch(){
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(async () => {
          await this.friendService.getFriends(this.searchQuery);
      }, 500);
    }


  async newGameInvitation(friendId: number){
    await this.friendService.newGameInvitation(friendId, Game.Chess)
  }





}
