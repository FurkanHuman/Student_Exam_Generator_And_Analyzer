#!/bin/bash
# Get parameters
APP_NAME=$1
ARCHITECTURE=$2
CERT_PATH=$3
CERT_PASSWORD=$4

echo "APP_NAME: $APP_NAME"
echo "ARCHITECTURE: $ARCHITECTURE"

# Check if parameters are missing
if [ -z "$APP_NAME" ] || [ -z "$ARCHITECTURE" ] || [ -z "$CERT_PATH" ] || [ -z "$CERT_PASSWORD" ]; then
    echo "Usage: ./Build.sh <app_name> <architecture> <cert_path> <cert_password>"
    exit 1
fi

# Build Docker image
echo "Docker Build started" 
docker buildx build --platform linux/$ARCHITECTURE -t $APP_NAME:$ARCHITECTURE --load .
echo "Docker Build finished"

# Check if a running container exists and stop it
echo "Checking for running container..."
RUNNING_CONTAINER=$(docker ps -q --filter "name=$APP_NAME")
if [ ! -z "$RUNNING_CONTAINER" ]; then
    echo "Running container found for $APP_NAME. Stopping it..."
    docker stop "$RUNNING_CONTAINER"
    docker rm "$RUNNING_CONTAINER"
else
    echo "No running container found for $APP_NAME."
fi

# Run Docker container
echo "Docker container started for $APP_NAME with architecture $ARCHITECTURE."
docker run -d --name $APP_NAME -p 8080:8080 -p 8085:8085 \
    -v "$CERT_PATH:/https/cert.pfx:ro" \
    -e ASPNETCORE_URLS="http://+:8080;https://+:8085" \
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx \
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="$CERT_PASSWORD" \
    $APP_NAME:"$ARCHITECTURE"
