export interface ILedger {
    "index": number,
    "date": string,
    "type": string,
    "amount": string,
    "subcategory": string,
    "title": string,
    "subject": string,
    "locked": string,
    "columns": ILedgerColumn[]
  }

  export interface ILedgerColumn {

    "columnValue": string,
    "columnName": string,
    "index": number,
    "size": number,
    "offset": number
  }