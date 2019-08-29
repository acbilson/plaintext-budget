import { Pipe, PipeTransform } from '@angular/core';
/*
 * Trims a string on both start and end
 * Usage:
 *   value | trim
 * Example:
 *   {{ "   hello" | trim }}
 *   formats to: "hello"
*/
@Pipe({ name: 'trim' })
export class TrimPipe implements PipeTransform {
  transform(value: string): string {
    return value.trim();
  }
}
