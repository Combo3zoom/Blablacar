import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentUserTripsComponent } from './current-user-trips.component';

describe('CurrentUserTripsComponent', () => {
  let component: CurrentUserTripsComponent;
  let fixture: ComponentFixture<CurrentUserTripsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CurrentUserTripsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrentUserTripsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
