import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PrependZerosPipe } from './prepend-zeros.pipe';

describe('PrependZerosPipe', () => {
  it('prepends the correct number of zeros', () => {
    // Arrange
    const pipe = new PrependZerosPipe();

    // Act
    const newIndex = pipe.transform(1234, 4);

    // Assert
    expect(newIndex.length).toEqual(4);
    expect(newIndex).toEqual('0001234');
  });
});
