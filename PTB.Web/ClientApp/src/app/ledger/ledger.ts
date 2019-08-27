export interface ILedgerEntry {
  "index": number,
  "date": ILedgerColumn,
  "type": ILedgerColumn,
  "amount": ILedgerColumn,
  "subcategory": ILedgerColumn,
  "title": ILedgerColumn,
  "subject": ILedgerColumn,
  "locked": ILedgerColumn,
}

export interface ILedgerColumn {
  "name": string,
  "value": string,
  "index": number,
  "size": number,
  "offset": number,
  "editable": boolean
}

export interface IRow {
  "index": number,
  "columns": IColumn[]
}

export interface IColumn {
  "columnValue": string,
  "columnName": string,
  "index": number,
  "size": number,
  "offset": number,
  "editable": boolean
}