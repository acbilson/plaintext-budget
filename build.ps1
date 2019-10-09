param(
  [Parameter(Mandatory)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("Build", "Run", "Bash")]
  [String]$Action
)

switch ($Action) {

  "Build" {

    # creates a complete Docker image for the PTB app
    docker build --file='docker/Dockerfile' --tag='ptb-test' .
  }
  "Run" {

    # creates container and launches server on localhost:4000
    docker run --rm -p 4000:80 ptb-test:latest dotnet PTB.Web.dll
  }
  "Bash" {

    # enters bash on docker container to review
    docker exec -it ptb-test /bin/bash
  }

}
