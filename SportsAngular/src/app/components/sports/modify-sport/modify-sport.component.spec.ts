import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifySportComponent } from './modify-sport.component';

describe('ModifySportComponent', () => {
  let component: ModifySportComponent;
  let fixture: ComponentFixture<ModifySportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModifySportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModifySportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
