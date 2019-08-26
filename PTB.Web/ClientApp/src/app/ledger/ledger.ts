export interface ILedger {
  "index": number,
  "date": string,
  "type": string,
  "amount": string,
  "subcategory": string,
  "title": string,
  "subject": string,
  "locked": string,
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
  "offset": number
}