import { Routes } from '@angular/router';

import { LoginComponent } from './Components/login-component/login-component';
import { DashboardComponent } from './Components/dashboard-component/dashboard-component';
import { authGuard } from './Components/Guards/auth-guard';

import { PeliculasCrud } from './Components/peliculas-crud/peliculas-crud';
import { SalasCrud } from './Components/salas-crud/salas-crud';
import { PeliSalaCrud } from './peli-sala-crud/peli-sala-crud';

export const routes: Routes = [

  {
    path:'login',
    component:LoginComponent

  },

{
  path:'dashboard',
  component:DashboardComponent,
  canActivate:[authGuard]
},
{
  path:'salas',
  component:SalasCrud
},
{
  path:'peliculascrud',
  component:PeliculasCrud
},
{
  path:'pelisalas',
  component:PeliSalaCrud
}
];
