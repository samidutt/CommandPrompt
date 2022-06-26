*********************************************
## Get current runnig version of docker
docker version 

## Get currnet running information of docker
docker info

## List docker images
docker images
docker image ls

## Pull and run hello-world image
docker run hello-world

## Pull and run ubuntu image
docker pull ubuntu
docker run -it ubuntu /bin/bash

## Deletes an image forcefully
docker image rm ubuntu -f

## Pull and run sample dotnet core web app in cloud shell
docker run -it --rm -p 4000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
