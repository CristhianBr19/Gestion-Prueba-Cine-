import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PeliculaSala } from './Models/peliculaSala';
import { Peliculas } from './Models/interfaceMovie';
import { Sala } from './Models/interfaceSala';

@Injectable({
  providedIn: 'root',
})
export class ApiServicePeliSala {

  readonly ApiUrlPeliculaSala=  'http://localhost:5218/api/pelicula_salacine';
   readonly ApiUrlPeliculas= 'http://localhost:5218/api/peliculas';
     readonly ApiUrlSalas= 'http://localhost:5218/api/sala_cine';

  constructor(private http:HttpClient){}

  getPeliSala(){
    return this.http.get<PeliculaSala[]>(this.ApiUrlPeliculaSala);
  }
   getPeliculas(){
      return this.http.get<Peliculas[]>(this.ApiUrlPeliculas);
    }
     getSalas(){
           return this.http.get<Sala[]>(this.ApiUrlSalas);
         }

  searchPeliSala(search:string){
    let params = new HttpParams();
    if(search){
      params = params.set('search', search);
    }
    return this.http.get<PeliculaSala[]>(`${this.ApiUrlPeliculaSala}/search`,{params});

  }
  desactivePeliSala(id:number){
    return this.http.put<any>(`${this.ApiUrlPeliculaSala}/desactive/${id}`,{});
  }

  editPeliSal(pelisala:PeliculaSala){
    let urlEditar=(`${this.ApiUrlPeliculaSala}/${pelisala.id}`);
    return this.http.put<PeliculaSala>(urlEditar, pelisala);

  }

  addPeli(peliSala:PeliculaSala){
    return this.http.post<PeliculaSala>(this.ApiUrlPeliculaSala, peliSala);
  }

}
