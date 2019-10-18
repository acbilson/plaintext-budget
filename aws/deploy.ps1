# retrieves AWS credentials
Invoke-Expression -Command (Get-ECRLoginCommand -Region us-east-2).Command

# example docker build
docker build -t abilson-repository .

# example tag for docker build
docker tag abilson-repository:latest 727829961330.dkr.ecr.us-east-2.amazonaws.com/abilson-repository:latest

# push build to repository
docker push 727829961330.dkr.ecr.us-east-2.amazonaws.com/abilson-repository:latest
