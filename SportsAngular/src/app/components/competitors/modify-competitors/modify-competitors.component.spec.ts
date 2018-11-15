import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifyCompetitorsComponent } from './modify-competitors.component';

describe('ModifyCompetitorsComponent', () => {
  let component: ModifyCompetitorsComponent;
  let fixture: ComponentFixture<ModifyCompetitorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModifyCompetitorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModifyCompetitorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
