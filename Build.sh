#!/bin/bash

# Function to show usage
show_usage() {
    echo "Usage: ./Build.sh <mode> <app_name> [options]"
    echo ""
    echo "Modes:"
    echo "  docker     - Build and run with Docker"
    echo "  standalone - Build and run standalone .NET application"
    echo ""
    echo "Docker mode:"
    echo "  ./Build.sh docker <app_name> <architecture> [cert_path] [cert_password]"
    echo "  Example: ./Build.sh docker myapp amd64"
    echo "  Example: ./Build.sh docker myapp amd64 ./cert.pfx mypassword"
    echo ""
    echo "Standalone mode:"
    echo "  ./Build.sh standalone <app_name> [configuration]"
    echo "  Example: ./Build.sh standalone myapp Release"
    echo ""
    echo "Architectures for Docker: amd64, arm64"
    echo "Configurations for Standalone: Debug, Release (default: Release)"
}

# Get parameters
MODE=$1
APP_NAME=$2

# Validate basic parameters
if [ -z "$MODE" ] || [ -z "$APP_NAME" ]; then
    show_usage
    exit 1
fi

case "$MODE" in
    "docker")
        ARCHITECTURE=$3
        CERT_PATH=$4
        CERT_PASSWORD=$5
        
        # Validate Docker parameters
        if [ -z "$ARCHITECTURE" ] || [ -z "$CERT_PATH" ] || [ -z "$CERT_PASSWORD" ]; then
            echo "Error: Missing parameters for Docker mode"
            show_usage
            exit 1
        fi
        
        # Validate certificate file exists
        if [ ! -f "$CERT_PATH" ]; then
            echo "Error: Certificate file not found: $CERT_PATH"
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
        
        if [ $? -ne 0 ]; then
            echo "Error: Docker build failed"
            exit 1
        fi
        
        echo "=== Docker Build finished ==="
        
        # Stop and remove existing container if any
        EXISTING_CONTAINER=$(docker ps -aq --filter "name=^/${APP_NAME}$")
        if [ -n "$EXISTING_CONTAINER" ]; then
            echo "Stopping existing container $APP_NAME..."
            docker stop "$APP_NAME"
            docker rm "$APP_NAME"
        fi
        
        # Get absolute path for certificate
        CERT_ABSOLUTE_PATH=$(realpath "$CERT_PATH")
        
        # Run Docker container
        echo "=== Running container $APP_NAME ($ARCHITECTURE) ==="
        docker run -d --name "$APP_NAME" \
            -p 8080:8080 -p 8085:8085 \
            -v "$CERT_ABSOLUTE_PATH:/https/cert.pfx:ro" \
            -e ASPNETCORE_URLS="http://+:8080;https://+:8085" \
            -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx \
            -e ASPNETCORE_Kestrel__Certificates__Default__Password="$CERT_PASSWORD" \
            "$APP_NAME:$ARCHITECTURE"
        
        if [ $? -eq 0 ]; then
            echo "=== Container started successfully ==="
            echo "Application is running at:"
            echo "  HTTP:  http://localhost:8080"
            echo "  HTTPS: https://localhost:8085"
            echo ""
            echo "To view logs: docker logs $APP_NAME"
            echo "To stop: docker stop $APP_NAME"
        else
            echo "Error: Failed to start container"
            exit 1
        fi
        ;;
        
    "standalone")
        CONFIGURATION=${3:-Release}
        
        echo "=== Building Standalone Application ==="
        echo "App: $APP_NAME, Configuration: $CONFIGURATION"
        
        # Navigate to project directory
        PROJECT_PATH="Src/SES/BlazorWebUI/BlazorWebUI"
        
        if [ ! -d "$PROJECT_PATH" ]; then
            echo "Error: Project directory not found: $PROJECT_PATH"
            exit 1
        fi
        
        cd "$PROJECT_PATH"
        
        # Restore dependencies
        echo "Restoring dependencies..."
        dotnet restore
        
        if [ $? -ne 0 ]; then
            echo "Error: Failed to restore dependencies"
            exit 1
        fi
        
        # Build application
        echo "Building application..."
        dotnet build -c "$CONFIGURATION"
        
        if [ $? -ne 0 ]; then
            echo "Error: Build failed"
            exit 1
        fi
        
        # Publish application
        echo "Publishing application..."
        dotnet publish -c "$CONFIGURATION" -o "../../../../publish/$APP_NAME"
        
        if [ $? -ne 0 ]; then
            echo "Error: Publish failed"
            exit 1
        fi
        
        cd - > /dev/null
        
        echo "=== Build completed successfully ==="
        echo "Published to: publish/$APP_NAME"
        echo ""
        echo "To run the application:"
        echo "  cd publish/$APP_NAME"
        echo "  dotnet BlazorWebUI.dll"
        ;;
        
    *)
        echo "Error: Invalid mode '$MODE'"
        show_usage
        exit 1
        ;;
esac