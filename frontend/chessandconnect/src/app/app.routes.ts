import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { redirectionGuard } from './guards/redirection.guard.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';
import { MenusComponent } from './pages/menus/menus.component';
import { MatchMakingChessComponent } from './pages/match-making-chess/match-making-chess.component';
import { MatchMakingConnect4Component } from './pages/match-making-connect4/match-making-connect4.component';

export const routes: Routes = [
    {path: "", component: HomeComponent},
    {path: "login", component: LoginComponent},
    {path: "register", component: RegisterComponent},
    {path: "menus", component: MenusComponent, canActivate: [redirectionGuard]},
    {path: "profile", component: UserProfileComponent,canActivate: [redirectionGuard]},
    {path: "chess", component: MatchMakingChessComponent, canActivate: [redirectionGuard]},
    {path: "connect", component: MatchMakingConnect4Component, canActivate: [redirectionGuard]},
];
