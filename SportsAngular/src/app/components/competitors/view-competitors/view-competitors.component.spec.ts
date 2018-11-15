import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCompetitorsComponent } from './view-competitors.component';

describe('ViewCompetitorsComponent', () => {
  let component: ViewCompetitorsComponent;
  let fixture: ComponentFixture<ViewCompetitorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewCompetitorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewCompetitorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
