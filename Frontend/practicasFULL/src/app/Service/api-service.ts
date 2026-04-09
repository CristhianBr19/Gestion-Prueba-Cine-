import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Peliculas } from '../Models/interfaceMovie';
import { Sala } from '../Models/interfaceSala';
import {  PeliculaSala } from '../Models/peliculaSala';

@Injectable({
  providedIn: 'root',
})
export class ApiService {

  readonly ApiUrlPeliculas= 'http://localhost:5218/api/peliculas';
  readonly ApiUrlSalas= 'http://localhost:5218/api/sala_cine';
  readonly ApiUrlPeliculaSala=  'http://localhost:5218/api/pelicula_salacine';

  constructor (public http: HttpClient){}

  getPeliculas(){
    return this.http.get<Peliculas[]>(this.ApiUrlPeliculas);
  }

  getPeliculasId(id:number){
    return this.http.get<Peliculas>(`${this.ApiUrlPeliculas}/${id}`);

  }

     getSalas(){
       return this.http.get<Sala[]>(this.ApiUrlSalas);
     }



  getPeliculaSala(){
    return this.http.get<PeliculaSala[]>(this.ApiUrlPeliculaSala);
  }

  getPeliculaSalaId(id:number){
    return this.http.get<PeliculaSala>(`${this.ApiUrlPeliculaSala}/${id}`);
  }

  searchPeli(titulo:string){
    let params = new HttpParams();
    if(titulo){
      params= params.set('search', titulo);
    }
    return this.http.get<Peliculas[]>(`${this.ApiUrlPeliculas}/search`, {params});
  }


  desactivatePeli(id:number){
    return this.http.put<void>(`${this.ApiUrlPeliculas}/desactivate/${id}`, {});
  }

  updatePeli(peli:Peliculas){
    let urlEditar = `${this.ApiUrlPeliculas}/${peli.id}`;
    return this.http.put<Peliculas>(urlEditar, peli);

  }

  addPeli(peli:Peliculas){
    return this.http.post<Peliculas>(this.ApiUrlPeliculas, peli);
  }

    searchPeliFecha(fecha:string){
    //let params = new HttpParams();
    //if(fecha){
      //params= params.set('fecha', fecha);
   // }
    return this.http.get<Peliculas[]>(`${this.ApiUrlPeliculas}/searchFecha/${fecha}`);
  }




}
