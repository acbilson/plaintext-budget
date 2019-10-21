import { Pipe, PipeTransform } from '@angular/core';
/*
 * Appends zeros to a number to standardize width to a certain value.
 * Takes a width argument that defaults to 7.
 * Usage:
 *   value | prependZeros:width
 * Example:
 *   {{ 123 | prependZeros:10 }}
 *   formats to: 0000000123
 */
@Pipe({ name: 'prependZeros' })
export class PrependZerosPipe implements PipeTransform {
  transform(value: number, width?: number): string {
    let prependCount = isNaN(width) ? 7 : width;
    prependCount -= value.toString().length;
    const prepend = '0'.repeat(prependCount);
    return prepend + value;
  }
}
