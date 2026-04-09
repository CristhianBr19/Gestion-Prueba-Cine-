import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-component',
  imports: [FormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {

  user:string='';
  pass:string='';

  constructor(private route:Router){}

  login(){

    if(this.user === 'admin' && this.pass==='1234'){

      localStorage.setItem('usuarioLogueado', 'true');

      this.route.navigate(['/dashboard'])

    }else{
      alert('Usuario o contraseña incorrecta');
      this.pass="";
    }

  }

}
