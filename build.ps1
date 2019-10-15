param(
  [Parameter(Mandatory)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("Start", "Stop", "Serve")]
  [String]$Action
)

switch ($Action) {

  "Start" {

    # runs both web and server in detached mode
    docker-compose up -d
  }

  "Stop" {

    docker-compose down

  }

  "Serve" {

    docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
  }

}
