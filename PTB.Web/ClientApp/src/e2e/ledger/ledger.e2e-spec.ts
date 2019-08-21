import { TestBed, inject } from '@angular/core/testing';
import {} from 'jasmine';
import { browser } from "protractor";

// spec.js
describe('Protractor Demo App', function() {
    it('should have a title', function() {
      browser.get('http://localhost:4200/ledger');
  
      browser.getTitle()
      .then(result => expect(result).toEqual('Super Calculator'));
    });
  });