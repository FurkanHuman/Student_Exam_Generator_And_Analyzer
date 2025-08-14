#!/bin/bash

# Get parameters
APP_NAME=$1
ARCHITECTURE=$2
CERT_PATH=$3
CERT_PASSWORD=$4

# Validate parameters
if [ -z "$APP_NAME" ] || [ -z "$ARCHITECTURE" ] || [ -z "$CERT_PATH" ] || [ -z "$CERT_PASSWORD" ]; then
    echo "Usage: ./Build.sh <app_name> <architecture> <cert_path> <cert_password>"
    exit 1
fi

echo "=== Building Docker image ==="
echo "App: $APP_NAME, Architecture: $ARCHITECTURE"

# Build Docker image
docker buildx build \
    --platform "linux/$ARCHITECTURE" \
    -t "$APP_NAME:$ARCHITECTURE" \
    --load \
    .

echo "=== Docker Build finished ==="

# Stop and remove existing container if any
EXISTING_CONTAINER=$(docker ps -aq --filter "name=^/${APP_NAME}$")
if [ -n "$EXISTING_CONTAINER" ]; then
    echo "Stopping existing container $APP_NAME..."
    docker stop "$APP_NAME"
    docker rm "$APP_NAME"
fi

# Run Docker container
echo "=== Running container $APP_NAME ($ARCHITECTURE) ==="
docker run -d --name "$APP_NAME" \
    -p 8080:8080 -p 8085:8085 \
    -v "$CERT_PATH:/https/cert.pfx:ro" \
    -e ASPNETCORE_URLS="http://+:8080;https://+:8085" \
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx \
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="$CERT_PASSWORD" \
    "$APP_NAME:$ARCHITECTURE"

echo "=== Container started successfully ==="
