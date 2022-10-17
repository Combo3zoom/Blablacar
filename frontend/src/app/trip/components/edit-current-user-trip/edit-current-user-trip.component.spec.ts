import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCurrentUserTripComponent } from './edit-current-user-trip.component';

describe('EditCurrentUserTripComponent', () => {
  let component: EditCurrentUserTripComponent;
  let fixture: ComponentFixture<EditCurrentUserTripComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCurrentUserTripComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditCurrentUserTripComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
