import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewFavouritesComponent } from './view-favourites.component';

describe('ViewFavouritesComponent', () => {
  let component: ViewFavouritesComponent;
  let fixture: ComponentFixture<ViewFavouritesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewFavouritesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewFavouritesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
