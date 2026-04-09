import { Component, ElementRef, signal, ViewChild, viewChild } from '@angular/core';
import { PeliSala } from '../Models/interface_peliculas_sala';
import { ApiServicePeliSala } from '../api-service-peli-sala';
import { Peliculas } from '../Models/interfaceMovie';
import { Sala } from '../Models/interfaceSala';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { PeliculaSala } from '../Models/peliculaSala';

declare const bootstrap:any;

@Component({
  selector: 'app-peli-sala-crud',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './peli-sala-crud.html',
  styleUrl: './peli-sala-crud.css',
})
export class PeliSalaCrud {

  peliSala=signal<PeliculaSala[]>([]);
  peliculas=signal<Peliculas[]>([]);
  salas=signal<Sala[]>([]);

  formPeliSala!:FormGroup;
  editingId:number|null=null;
  minDate='1942-02-12';
  maxDate= new Date().toISOString().split('T')[0];
  modalRef:any;

  constructor(private miservicio:ApiServicePeliSala, private fb:FormBuilder){
    this.formPeliSala = this.fb.group({
      id_pelicula:['',[Validators.required]],
      id_sala:['',[Validators.required]],
      fecha_publicacion:['',[Validators.required]],
      fecha_fin:['',[Validators.required]],
      active:[true]

    })

  }

  @ViewChild('peliSalaModalRef') modalElemnt!:ElementRef;
  ngAfterViewInit(){
    this.modalRef = new bootstrap.Modal(this.modalElemnt.nativeElement);
  }

  ngOnInit(){
    this.loadPeliSala();
    this.loadPeliculas();
    this.loadSalas();
  }

  loadPeliSala(){
    this.miservicio.getPeliSala().subscribe({
      next:(data)=>{
        this.peliSala.set(data);
      },error:(e)=>{
        console.log(e);

      }
    })
  }

  loadPeliName(id:number){
    const pelis= this.peliculas().find((p) =>Number(p.id)===id);
    console.log(pelis);
    return pelis ? pelis.nombre:'Sin pelicula';

  }

  loadPeliculas(){
    this.miservicio.getPeliculas().subscribe({
      next:(data)=>{
        this.peliculas.set(data);

      },error:(e)=>{
         console.log(e);
      }
    })
  }

  loadSalas(){
    this.miservicio.getSalas().subscribe({
      next:(data)=> {
          this.salas.set(data);
      }
    })
  }

  loadSalaName(id:number){
    const sala = this.salas().find((s)=>Number(s.id) === id );
    return sala ? sala.nombre:'Sin sala';
  }


  openModal(){
    this.editingId = null;
    this.formPeliSala.reset();
    this.modalRef.show();
  }
  editModal(peliSala:PeliSala){
    this.editingId = peliSala.id ?? null;
    this.formPeliSala.patchValue(peliSala);
    this.modalRef.show();
  }
  desactivateSala(peliSala:PeliSala){
    const confirmado = confirm(`Estas seguro de eliminar la pelicula`);
    if(confirmado){
      this.miservicio.desactivePeliSala(Number(peliSala.id)).subscribe({
        next:()=> {
          alert('Pelicula eliminada con exito');
          this.loadPeliSala();
        }, error:(e)=> {
            console.log(e);
        }
      });
    }else{
      alert('No se encontro la asignacion');
    }
  }

  search(busq:HTMLInputElement){
    const busqueda = busq.value.toLocaleLowerCase();
    this.miservicio.searchPeliSala(busqueda).subscribe({
      next:(data)=>{
        this.peliSala.set(data);
      }

    })

  }
  save(){
    if(this.formPeliSala.invalid){
      this.formPeliSala.markAsTouched();
      return;
    }

    const datosForm = this.formPeliSala.value;
    if(this.editingId){
      let editDatos:PeliculaSala={...datosForm, id:this.editingId};
      this.miservicio.editPeliSal(editDatos).subscribe({
        next:()=>{
          alert('Asignacion actualizada con exito');
          this.modalRef.hide();
          this.loadPeliSala();

        },error(e){
          console.log(e);
        }

      });
    }else{
      let nuevoDatos:PeliculaSala={...datosForm};
      this.miservicio.addPeli(nuevoDatos).subscribe({
        next:(datos)=>{
          this.peliSala.update(list=>[...list,datos]);
          alert('Asignacion creada con exitro');
          this.modalRef.hide();
          this.loadPeliSala();

        }
      })
    }

  }
}
