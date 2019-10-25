param(
[Parameter(Mandatory=$true,Position=0)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("build", "start", "stop", "restart")]
  [String]$BuildAction,

  [Parameter(Mandatory=$true,Position=1)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("dev", "prod", "test")]
  [String]$BuildType,

  
  [Parameter(Mandatory=$true,Position=2)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("web", "server", "json-server")]
  [String]$Target
)

switch ($BuildAction) {

  "build" {

    switch ($BuildType) {

      "prod" {

        docker-compose build $Target
      }

      "dev" {

        docker-compose `
        -f docker-compose.yml `
        -f docker-compose.dev.yml `
        build $Target
      }

      "test" {

      }
    }
  } # build

  "start" {

     switch ($BuildType) {

      "prod" {

        docker-compose up -d $Target
      }

      "dev" {

        docker-compose `
        -f docker-compose.yml `
        -f docker-compose.dev.yml `
        up -d $Target
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

        docker-compose restart $Target
      }

      "dev" {

        docker-compose `
        -f docker-compose.yml `
        -f docker-compose.dev.yml `
        restart $Target
      }

      "test" {

      }
    }
  } # restart
}
