param(
  [Alias('a')]
  [Parameter(Mandatory=$true,Position=0)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("build", "start", "stop", "restart")]
  [String]$Action,

  [Alias('b')]
  [Parameter(Mandatory=$false,Position=1)]
  [ValidateSet("dev", "prod", "test")]
  [String]$BuildType,
  
  [Alias('t')]
  [Parameter(Mandatory=$false,Position=2)]
  [ValidateSet("web", "server", "json-server")]
  [String]$BuildTarget
)

# confirms required values are present
$passed = $true
$buildTypeMsg = "Must enter a build type (dev, prod, test) to run the build action."
$buildTargetMsg = "Must enter a build target (web, server, json-server) to run the build action."
if (@("build","start","restart").Contains($Action) -and $BuildType -eq "") { Write-Host -ForegroundColor Red $buildTypeMsg; $passed = $false }
if (@("build","start","restart").Contains($Action) -and $BuildTarget -eq "") { Write-Host -ForegroundColor Red $buildTargetMsg; $passed = $false }
if ($passed -eq $false) { Write-Host "Action skipped. Please resolve errors to continue."; return }

switch ($Action) {

  "build" {

    switch ($BuildType) {

      "prod" {

        docker-compose build $BuildTarget
      }

      "dev" {

        docker-compose `
        -f docker-compose.yml `
        -f docker-compose.dev.yml `
        build $BuildTarget
      }

      "test" {

      }
    }
  } # build

  "start" {

     switch ($BuildType) {

      "prod" {

        docker-compose up -d $BuildTarget
      }

      "dev" {

        docker-compose `
        -f docker-compose.yml `
        -f docker-compose.dev.yml `
        up -d $BuildTarget
      }

      "test" {

      }
    }
  } # start

  "stop" {

    docker-compose down
  } # stop

  "restart" {

    switch ($BuildType) {

      "prod" {

        docker-compose restart $BuildTarget
      }

      "dev" {

        docker-compose `
        -f docker-compose.yml `
        -f docker-compose.dev.yml `
        restart $BuildTarget
      }

      "test" {

      }
    }
  } # restart
}
