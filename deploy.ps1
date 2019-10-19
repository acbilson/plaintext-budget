param(
[Parameter(Mandatory=$true,Position=0)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("to_ecr", "to_eb")]
  [String]$DeployAction,

  [Parameter(Mandatory=$true,Position=1)]
  [ValidateNotNullOrEmpty()]
  [ValidateSet("web", "server", "both")]
  [String]$Component
)

switch ($DeployAction) {

  # deploys most recent Docker images to the aws Docker repo
  "to_ecr" {

    # logs into AWS ECR, which is the name of the AWS docker repository
    aws ecr get-login --no-include-email | sed 's|https://||' | iex
    $repoBaseUrl = "727829961330.dkr.ecr.us-east-2.amazonaws.com"

    switch ($Component) {

      "web" {

        # copies my build to a new tag
        docker tag abilson/ptb-web:nginx-alpine $repoBaseUrl/ptb-web:nginx-alpine

        # pushes my image to the aws repo
        docker push $repoBaseUrl/ptb-web:nginx-alpine
      }

      "server" {

        # copies my build to a new tag
        docker tag abilson/ptb-json-server:12.11.1-alpine $repoBaseUrl/ptb-server:12.11.1-alpine

        # pushes my image to the aws repo
        docker push $repoBaseUrl/ptb-server:12.11.1-alpine
      }
    }
  } # to_ecr

  # deploys my code via Docker using the elastic beanstalk service
  "to_eb" {

    # logs into AWS ECR, which is the name of the AWS docker repository
    aws ecr get-login --no-include-email | sed 's|https://||' | iex
    $repoBaseUrl = "727829961330.dkr.ecr.us-east-2.amazonaws.com"

    switch ($Component) {

      "web" {

        # copies my build to a new tag
        docker tag abilson/ptb-web:nginx-alpine $repoBaseUrl/ptb-web:nginx-alpine

        # pushes my image to the aws repo
        docker push $repoBaseUrl/ptb-web:nginx-alpine
      }

      "server" {

        # copies my build to a new tag
        docker tag abilson/ptb-json-server:12.11.1-alpine $repoBaseUrl/ptb-server:12.11.1-alpine

        # pushes my image to the aws repo
        docker push $repoBaseUrl/ptb-server:12.11.1-alpine
      }
    }
  } # to_ecr

}
