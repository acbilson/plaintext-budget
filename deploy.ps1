# logs into AWS ECR, which is the name of the AWS docker repository
aws ecr get-login --no-include-email | sed ‘s|https://||’ | iex

# builds the relevant docker containers
docker-compose build -f docker-compose.dev.yml
