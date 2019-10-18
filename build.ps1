param(
  [Parameter(Mandatory=$true,Position=0)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("dev", "prod", "test")]
  [String]$BuildType,

  [Parameter(Mandatory=$true,Position=1)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("build", "start", "stop")]
  [String]$BuildAction,

  [Parameter(Mandatory=$false,Position=2)]
  [String]$Target=""

)

switch ($BuildType) {

  "dev" {

    switch ($BuildAction) {

      "build" {

        docker-compose -f docker-compose.yml -f docker-compose.dev.yml build $Target
      }
      "start" {
      
        docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d $Target
      }
      "stop" {

        docker-compose down
      }
    }
  }
  "prod" {
    switch ($BuildAction) {

      "build" {

        docker-compose -f docker-compose.yml build $Target
      }
      "start" {

        docker-compose -f docker-compose.yml up -d $Target
      }
      "stop" {

        docker-compose down
      }
    }
  }
  "test" {

    switch ($BuildAction) {

      "build" {

      }
      "start" {

      }
      "stop" {

      }
    }
  }
}
