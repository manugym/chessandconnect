import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';
import { MenuService } from '../../services/menu.service';
import { ApiService } from '../../services/api.service';
import { UserListComponent } from '../../components/user-list/user-list.component';
import { WebsocketService } from '../../services/websocket.service';
import { MatDialog } from '@angular/material/dialog';
import { NavigationStart, Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { GameInvitationComponent } from '../../components/game-invitation/game-invitation.component';
import { RequestListComponent } from '../../components/request-list/request-list.component';
import { AuthService } from '../../services/auth.service';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-menus',
  imports: [NavbarComponent, FriendsListComponent, RouterLink],
  templateUrl: './menus.component.html',
  styleUrl: './menus.component.css'
})
export class MenusComponent implements OnInit{
  constructor(
    public menuService: MenuService,
    private api: ApiService,
    private webSocketService: WebsocketService,
    private router: Router,
    private authService: AuthService,
    private gameService: GameService

  ) {
    
  }

  async ngOnInit(): Promise<void> {
    this.gameService.opponent = null
    this.authService.getCurrentUser();
    await this.webSocketService.connectRxjs()

  }
}
