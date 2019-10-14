import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LedgerPageComponent } from './budget-page.component';

describe('LedgerPageComponent', () => {
  let component: LedgerPageComponent;
  let fixture: ComponentFixture<LedgerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LedgerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LedgerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
