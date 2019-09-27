# FolderAPI
curl -Method GET -Uri http://localhost:5000/api/Folder/GetFileFolders -UseBasicParsing | ConvertFrom-Json | Format-List
curl -Method GET -Uri http://localhost:5000/api/Folder/GetFileSchema -UseBasicParsing | ConvertFrom-Json | Format-List

# LedgerAPI
curl -Method GET -Uri "http://localhost:5000/api/Ledger/Read?fileName=checking&index=0&count=5" -UseBasicParsing | ConvertFrom-Json | Format-List

# LoggingAPI
curl -Method POST -Uri http://localhost:5000/api/Logging/Log -Body "{ Level: 1, Message:'This is a server test', Timestamp:'', Context:'Powershell'}" -UseBasicParsing -Headers @{ "Content-Type"="application/json" }

# BudgetAPI
$response = curl -Method GET -Uri "http://localhost:5000/api/Budget/Read?start=2018-04-01&end=2018-04-30" -UseBasicParsing | ConvertFrom-Json
$response | select -First 5
