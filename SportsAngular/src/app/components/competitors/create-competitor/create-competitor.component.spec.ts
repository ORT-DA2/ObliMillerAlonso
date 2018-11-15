import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCompetitorComponent } from './create-competitor.component';

describe('CreateCompetitorComponent', () => {
  let component: CreateCompetitorComponent;
  let fixture: ComponentFixture<CreateCompetitorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateCompetitorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateCompetitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
