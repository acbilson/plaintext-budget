import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PrependZerosPipe } from './prepend-zeros.pipe';

describe('PrependZerosPipe', () => {

  it('prepends the correct number of zeros', () => {

    // Arrange
    let pipe = new PrependZerosPipe();
    let index = "1234";
    const width = index.length + 3;

    // Act
    const newIndex = pipe.transform(index, width);

    // Assert
    expect(newIndex.length).toEqual(width);
    expect(newIndex).toEqual("0001234");
  });
});