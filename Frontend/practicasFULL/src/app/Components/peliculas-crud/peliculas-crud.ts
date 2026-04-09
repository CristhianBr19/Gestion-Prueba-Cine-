import { Component, ElementRef, signal, ViewChild } from '@angular/core';
import { Peliculas } from '../../Models/interfaceMovie';
import { ApiService } from '../../Service/api-service';
import { DatePipe, NgIf } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

declare const bootstrap:any;
@Component({
  selector: 'app-peliculas-crud',
  imports: [ReactiveFormsModule,NgIf, FormsModule, DatePipe],
  templateUrl: './peliculas-crud.html',
  styleUrl: './peliculas-crud.css',
})
export class PeliculasCrud {

  peliculas = signal<Peliculas[]>([]);
  fechaFiltro:string="";

  formPeliculas!: FormGroup;

  editinId:number|null = null;
  minDate='1942-01-01';
  maxDate= new Date().toISOString().split("T")[0];
  modalRef:any;

  constructor(private miservicio:ApiService, private fb:FormBuilder){
     this.formPeliculas = this.fb.group({
      nombre:['',[Validators.required]],
      duracion:['',[Validators.required]],
      active:[true],
      fecha_publicacion:['']
     })
  }

  @ViewChild('peliModalRef') modalElement!:ElementRef;
  ngAfterViewInit(){
    this.modalRef = new bootstrap.Modal(this.modalElement.nativeElement);


  }

  ngOnInit(){
    this.loadPeliculas();
  }

  loadPeliculas(){
    this.miservicio.getPeliculas().subscribe({
      next:(data)=>{
          this.peliculas.set(data);
      }
    })
  }


  search(busq:HTMLInputElement){

    const params = busq.value.toLowerCase();

    this.miservicio.searchPeli(params).subscribe({
      next:(data:Peliculas[])=>{
         this.peliculas.set(data);

      }
    })
  }

  desactivatePEli(peli:Peliculas){
    const confirmado = confirm(`Estas seguro de eliminar la pelicula: ${peli.nombre} ?` );
    if(confirmado){
      this.miservicio.desactivatePeli(peli.id).subscribe({
        next:()=>{
          alert('Eliminado exitosamente');
          this.loadPeliculas();
        }, error:(e)=>{
             console.log(e);
        }
      });
    }else{
      alert('Nos se encontro la pelicula ');
    }
  }

    openModal(){
      this.editinId = null;
      this.formPeliculas.reset();
      this.modalRef.show();
    }

    editModal(peli:Peliculas){
      this.editinId = peli.id ?? null;
      const peliForma ={...peli};
      if(peli.fecha_publicacion){
       peliForma.fecha_publicacion = peli.fecha_publicacion.split('T')[0];
      }

      this.formPeliculas.patchValue(peliForma);
      this.modalRef.show();
    }

    save(){
      if(this.formPeliculas.invalid){
        this.formPeliculas.markAsTouched();
        return;
      }
      const datos = this.formPeliculas.value;

      if(this.editinId){
        let editDatos:Peliculas={...datos, id:this.editinId};
        this.miservicio.updatePeli(editDatos).subscribe({
          next:()=>{
            alert('Pelicula actualizada con exito');
            this.modalRef.hide();
            this.loadPeliculas();
          }
        });

      }else{
        let nuevoDatos:Peliculas={...datos};
        this.miservicio.addPeli(nuevoDatos).subscribe({
          next:(nuevoD)=>{
            this.peliculas.update(list=>[...list,nuevoD]);
            alert('Pelicula creada con exito');
            this.modalRef.hide();
            this.loadPeliculas();
          }
        })
      }

    }

    aplicarFiltroFecha(){
      if(!this.fechaFiltro){
        alert("La fecha debe ser seleccionada");
        return;
      }

      this.miservicio.searchPeliFecha(this.fechaFiltro).subscribe({
        next:(data) =>{
           this.peliculas.set(data);
        },error:(e) =>{
          this.peliculas.set([]);
          alert("No se encontraron peliculas para esa fecha ");
        }
      })

    }


}



