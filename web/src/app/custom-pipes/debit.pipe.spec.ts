import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DebitPipe } from './debit.pipe';

describe('DebitPipe', () => {
  it('debits a string amount', () => {
    // Arrange
    let pipe = new DebitPipe();
    let amount = '80.12';

    // Act
    const newAmount = pipe.transform(amount);

    // Assert
    expect(newAmount).toEqual('(80.12)');
  });
});
