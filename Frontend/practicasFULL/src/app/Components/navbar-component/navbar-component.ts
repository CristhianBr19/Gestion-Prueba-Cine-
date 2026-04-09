import { Component, NgModule } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule, NgIf } from '@angular/common';
@Component({
  selector: 'app-navbar-component',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar-component.html',
  styleUrl: './navbar-component.css',
})
export class NavbarComponent {

  constructor(private router:Router){}

  isLoggedIn():boolean{
    return localStorage.getItem('usuarioLogueado')==='true';
  }

  onLogout(){
    localStorage.removeItem('usuarioLogueado');
  this.router.navigate(['/login']);
  }

}
