import { LedgerPage } from './ledger.po';
import { browser } from 'protractor';

describe('Ledger Page', () => {
  let page: LedgerPage;

  beforeEach(() => {
    page = new LedgerPage();
  });

  it('navigates', () => {
    page.navigateTo();
  });

  fit('has a ledger', () => {
    page.navigateTo().then(() => {
      page.hasALedger();
    });

  });
});
