import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Sala } from '../Models/interfaceSala';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiServiceSala {

   readonly ApiUrlSalas= 'http://localhost:5218/api/sala_cine';
   estado=[
    { label: 'Operativa', value: 'Operativa' },
  { label: 'En Mantenimiento', value: 'Mantenimiento' },
  { label: 'Fuera de Servicio', value: 'Cerrada' }
   ]

   constructor(private http:HttpClient){

   }


     getSalas(){
       return this.http.get<Sala[]>(this.ApiUrlSalas);
     }

     getSalasId(id:number){
       return this.http.get<Sala>(`${this.ApiUrlSalas}/ ${id}`);
     }


       searchSala(titulo:string){
         let params= new HttpParams();
         if(titulo){
           params = params.set('search', titulo);
         }
         return this.http.get<Sala[]>(`${this.ApiUrlSalas}/search`,{params});
       }

       desactivateSala(id:number){
        return this.http.put<void>(`${this.ApiUrlSalas}/desactivate/${id}`,{});
       }

       updateSala(sala:Sala){
        let urlEditar = (`${this.ApiUrlSalas}/${sala.id}`);
        this.http.put<Sala>(urlEditar,sala);

       }

       updateSalas(sala:Sala){
          let urlEditar = (`${this.ApiUrlSalas}/${sala.id}`);
        return this.http.put<Sala>(urlEditar,sala);
       }

       addSalas(sala:Sala){
        return this.http.post<Sala>(this.ApiUrlSalas, sala);
       }

  ObtenerEstadoReal(nombreSala: string) {

  return this.http.get(`${this.ApiUrlSalas}/disponibilidad/${nombreSala}`,{
    responseType:'text' //lee el texto plano
  });

}
}

