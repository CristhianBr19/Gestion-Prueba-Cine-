import { Component, ElementRef, signal, viewChild, ViewChild } from '@angular/core';
import { Sala } from '../../Models/interfaceSala';

import { ApiServiceSala } from '../../Service/api-service-sala';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgClass } from '@angular/common';

declare const bootstrap:any;
@Component({
  selector: 'app-salas-crud',
  imports: [ReactiveFormsModule, FormsModule, NgClass],
  templateUrl: './salas-crud.html',
  styleUrl: './salas-crud.css',
})
export class SalasCrud {

  sala = signal<Sala[]>([]);

  formSala!:FormGroup;
  get nombre(){
    return this.formSala.get('nombre')
  }
  get estados(){
    return this.formSala.get('estado')
  }

  editingId:number|null = null;
  minDate='1942-06-21';
  maxDate= new Date().toISOString().split("T")[0];
  modalRef:any;
  estado:any []=[]

  constructor(private miservicio:ApiServiceSala, private fb:FormBuilder){
    this.formSala = this.fb.group({
      nombre:['',[Validators.required]],
      estado:['',[Validators.required]],
      active:[true]

    })
  }

  @ViewChild('SalaModalRef') modalElement!:ElementRef;
  ngAfterViewInit(){
    this.modalRef = new bootstrap.Modal(this.modalElement.nativeElement);
  }



  ngOnInit(){
    this.loadSalas();
    this.cargarSalas();
    this.estado = this.miservicio.estado;
  }

  loadSalas(){
    this.miservicio.getSalas().subscribe({
      next:(data)=>{
        this.sala.set(data);

      },error:(e)=>{
        console.log(e);
      }
    })
  }

  search(busq:HTMLInputElement){
    const params = busq.value.toLocaleLowerCase();

    this.miservicio.searchSala(params).subscribe({
      next:(data:Sala[])=>{
        this.sala.set(data);
      }
    })

  }
  openModal(){
    this.editingId = null;
    this.formSala.reset();
    this.modalRef.show();
  }
  editModal(sala:Sala){
    this.editingId = sala.id ?? null;
    this.formSala.patchValue(sala);
    this.modalRef.show();

  }
  desactivateSala(sala:Sala){
    const confirmado = confirm(`Estas seguro de eliminar la sala: ${sala.nombre}?`);
    if(confirmado) {
      this.miservicio.desactivateSala(sala.id).subscribe({
        next:()=>{
          alert('Sala eliminada exitosamente');
          this.loadSalas();
        },error:(e)=>{
               console.log(e);
        }
      });
    }else{
      alert('No se encontro la sala');
    }
  }

  save(){
    if(this.formSala.invalid){
      this.formSala.markAllAsTouched();
      return;
    }
    const datosSala = this.formSala.value;
    if(this.editingId){
      let editDatos:Sala={...datosSala, id:this.editingId};
      this.miservicio.updateSalas(editDatos).subscribe({
        next:() =>{
          alert('Sala actuaizada con exito');
          this.modalRef.hide();
          this.loadSalas();
          this.cargarSalas();
        }
      });
    }else{
      let nuevosDatos:Sala={...datosSala};
      this.miservicio.addSalas(nuevosDatos).subscribe({
        next:(datos)=>{
          this.sala.update(list=>[...list, datos]);
          alert("Sala creada con exito");
          this.modalRef.hide();
          this.loadSalas();
          this.cargarSalas();
        }
      })
    }
  }

  cargarSalas(){
    this.miservicio.getSalas().subscribe({
      next:(data)=>{
        this.sala.set(data);

     data.forEach(sala => {
        this.miservicio.ObtenerEstadoReal(sala.nombre).subscribe(res => {

          this.sala.update(listaActual =>
            listaActual.map(s =>
              s.id === sala.id ? { ...s, estadoReal: res } : s
            )
          );

        });
      });
    }
  });
}
}
