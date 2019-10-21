import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TrimPipe } from './trim.pipe';

describe('TrimPipe', () => {
  it('trims spaces', () => {
    // Arrange
    const pipe = new TrimPipe();
    const subcategory = '   TestCategory  ';

    // Act
    const newSubcategory = pipe.transform(subcategory);

    // Assert
    expect(newSubcategory).toEqual('TestCategory');
  });
});
