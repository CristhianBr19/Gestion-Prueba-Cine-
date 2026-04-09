import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarPeliculas } from './asignar-peliculas';

describe('AsignarPeliculas', () => {
  let component: AsignarPeliculas;
  let fixture: ComponentFixture<AsignarPeliculas>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AsignarPeliculas]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AsignarPeliculas);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
