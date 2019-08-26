export interface IFileFolders {

  "ledgerFolder": IPTBFolder,
}

export interface IPTBFolder {

  "defaultFileName": string,
  "name": string,
  "files": IPTBFile[]

}

export interface IPTBFile {
    "ledgerDate": string,
    "startDate": string,
    "endDate": string,
    "directoryName": string,
    "fileName": string,
    "fullPath": string,
    "lineCount": number,
    "shortName": string
  }