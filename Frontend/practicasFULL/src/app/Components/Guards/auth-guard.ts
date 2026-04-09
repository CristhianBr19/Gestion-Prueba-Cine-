import { inject, Inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router=  inject(Router)

  const logueo = localStorage.getItem('usuarioLogueado');

  if(logueo === 'true'){
    return true;


    }else{
      router.navigate(['/login']);
      return false;

  }




};
