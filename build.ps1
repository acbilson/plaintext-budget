param(
  [Parameter(Mandatory)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("Start", "Stop")]
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
}
