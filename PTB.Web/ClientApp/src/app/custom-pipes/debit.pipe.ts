import { Pipe, PipeTransform } from '@angular/core';
/*
 * Converts a number to the accounting paradigm of ($)
 * Usage:
 *   value | accounting
 * Example:
 *   {{ 123.12 | debit }}
 *   formats to: (123.12)
*/
@Pipe({name: 'debit'})
export class DebitPipe implements PipeTransform {
  transform(value: string): string {
    return "(" + value + ")";
  }
}
