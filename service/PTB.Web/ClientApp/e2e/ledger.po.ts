import { browser, by, element } from 'protractor';

export class LedgerPage {
  navigateTo() {
    return browser.get('/ledger');
  }

  hasALedger() {
    var ledgerRow = element(by.css('[scope="row"]'));
    expect(ledgerRow.isPresent()).toBeTruthy();
  }
}
