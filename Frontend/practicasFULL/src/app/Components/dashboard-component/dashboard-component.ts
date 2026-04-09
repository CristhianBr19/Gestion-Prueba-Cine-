import { Component } from '@angular/core';
import { ApiService } from '../../Service/api-service';
import { ApiServiceSala } from '../../Service/api-service-sala';

@Component({
  selector: 'app-dashboard-component',
  imports: [],
  templateUrl: './dashboard-component.html',
  styleUrl: './dashboard-component.css',
})
export class DashboardComponent {

  totalpeliculas: number = 0;
  totalsalas: number = 0;
  totalsalasdisponibles:number = 0;

  constructor(private miservicio:ApiService, private  servicioSala: ApiServiceSala){}

  ngOnInit() :void{
    this.getPeliculas();
  }

  getPeliculas(){
    this.miservicio.getPeliculas().subscribe({
      next:(data)=>{
       this.totalpeliculas = data.length;
      }

    });
    this.miservicio.getSalas().subscribe({
      next:(data)=>{
        this.totalsalas = data.length;
        //reiniciamos contador
        this.totalsalasdisponibles =  0;   // = data.filter(p=>p.active === true).length;
        data.forEach(sala=>{
          this.servicioSala.ObtenerEstadoReal(sala.nombre).subscribe(res=>{
            if(res == 'Sala disponible'){
              this.totalsalasdisponibles++;
            }
          });
        });

      }
    });
  }
}
