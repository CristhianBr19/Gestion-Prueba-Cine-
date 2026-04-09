import { Peliculas } from "./interfaceMovie";
import { Sala } from "./interfaceSala";

export interface PeliSala {

  id?:number;
  id_pelicula:number;
  id_sala:number;
  fecha_publicacion:string;
  fecha_fin?:string;
  active?:boolean;
  pelicula?:Peliculas;
  sala?:Sala;

}
