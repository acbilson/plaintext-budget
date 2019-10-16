param(
  [Parameter(Mandatory)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("Stop", "Serve")]
  [String]$Action
)

switch ($Action) {

  "Stop" {

    docker-compose down
  }

  "Serve" {

    docker-compose up -d
  }
}
