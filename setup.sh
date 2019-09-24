###########################
# Ubuntu 18.04 instructions
###########################

# updates all existing packages
sudo apt update

# installs nodejs
sudo apt install nodejs

# installs dotnet 2.1 package to local
wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

# installs dotnet 2.1 from package
sudo add-apt-repository universe
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install aspnetcore-runtime-2.1=2.1.1-1
