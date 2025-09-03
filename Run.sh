#!/bin/bash

# Function to show usage
show_usage() {
    echo "Usage: ./Run.sh <mode> <app_name> [options]"
    echo ""
    echo "Modes:"
    echo "  docker     - Build and run with Docker"
    echo "  standalone - Build and run standalone .NET application"
    echo ""
    echo "Docker mode:"
    echo "  ./Run.sh docker <app_name> <architecture> [cert_path] [cert_password] [http_port] [https_port]"
    echo "  Example: ./Run.sh docker myapp amd64"
    echo "  Example: ./Run.sh docker myapp amd64 ./cert.pfx mypassword 8080 8085"
    echo ""
    echo "Standalone mode:"
    echo "  ./Run.sh standalone <app_name> [configuration] [http_port] [https_port]"
    echo "  Example: ./Run.sh standalone myapp Release 8080 8085"
    echo ""
    echo "Architectures for Docker: amd64, arm64"
    echo "Configurations for Standalone: Debug, Release (default: Release)"
}

# Get parameters
MODE=$1
APP_NAME=$2

# Validate basic parameters
if [ -z "$MODE" ] || [ -z "$APP_NAME" ]; then
    echo "Error: Mode and app name are required"
    show_usage
    exit 1
fi

case "$MODE" in
    "docker")
        ARCHITECTURE=$3
        CERT_PATH=$4
        CERT_PASSWORD=$5
        PORT_HTTP=${6:-8080}
        PORT_HTTPS=${7:-8085}
        
        # Validate Docker parameters
        if [ -z "$ARCHITECTURE" ]; then
            echo "Error: Architecture parameter required for Docker mode"
            show_usage
            exit 1
        fi
        
        # Check if certificate is provided and validate
        USE_CERTIFICATE=false
        if [ -n "$CERT_PATH" ] && [ -n "$CERT_PASSWORD" ]; then
            if [ ! -f "$CERT_PATH" ]; then
                echo "Error: Certificate file not found: $CERT_PATH"
                exit 1
            fi
            USE_CERTIFICATE=true
            echo "Using certificate: $CERT_PATH"
        elif [ -n "$CERT_PATH" ] || [ -n "$CERT_PASSWORD" ]; then
            echo "Error: Both certificate path and password must be provided or both omitted"
            show_usage
            exit 1
        else
            echo "Running without HTTPS certificate (HTTP only)"
        fi
        
        # Stop and remove existing container if any
        EXISTING_CONTAINER=$(docker ps -aq --filter "name=^${APP_NAME}$")
        if [ -n "$EXISTING_CONTAINER" ]; then
            echo "Stopping existing container $APP_NAME..."
            docker stop "$APP_NAME" >/dev/null 2>&1
            docker rm "$APP_NAME" >/dev/null 2>&1
        fi
        
        # Run Docker container
        echo "=== Running container $APP_NAME ($ARCHITECTURE) ==="
        
        DOCKER_ARGS="run -d --name $APP_NAME -p $PORT_HTTP:8080 -p $PORT_HTTPS:8085 -e ASPNETCORE_URLS=\"http://+:8080;https://+:8085\""
        
        # Add certificate mounting if provided
        if [ "$USE_CERTIFICATE" = true ]; then
            if command -v realpath >/dev/null 2>&1; then
                CERT_ABSOLUTE_PATH=$(realpath "$CERT_PATH")
            else
                CERT_ABSOLUTE_PATH=$(readlink -f "$CERT_PATH")
            fi
            DOCKER_ARGS="$DOCKER_ARGS -v $CERT_ABSOLUTE_PATH:/https/cert.pfx:ro"
            DOCKER_ARGS="$DOCKER_ARGS -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx"
            DOCKER_ARGS="$DOCKER_ARGS -e ASPNETCORE_Kestrel__Certificates__Default__Password=$CERT_PASSWORD"
        fi
        
        DOCKER_ARGS="$DOCKER_ARGS $APP_NAME:$ARCHITECTURE"
        
        eval "docker $DOCKER_ARGS"
        
        if [ $? -eq 0 ]; then
            echo "=== Container started successfully ==="
            echo "Application is running at:"
            echo "  HTTP:  http://localhost:$PORT_HTTP"
            if [ "$USE_CERTIFICATE" = true ]; then
                echo "  HTTPS: https://localhost:$PORT_HTTPS"
            else
                echo "  HTTPS: Not available (no certificate provided)"
            fi
            echo ""
            echo "To view logs: docker logs $APP_NAME"
            echo "To stop: docker stop $APP_NAME"
        else
            echo "Error: Failed to start container"
            docker rm -f "$APP_NAME" >/dev/null 2>&1
            exit 1
        fi
        ;;
        
    "standalone")
        CONFIGURATION=${3:-Release}
        PORT_HTTP=${4:-8080}
        PORT_HTTPS=${5:-8085}
        
        # Validate configuration
        if [ "$CONFIGURATION" != "Debug" ] && [ "$CONFIGURATION" != "Release" ]; then
            echo "Error: Invalid configuration '$CONFIGURATION'. Use Debug or Release"
            show_usage
            exit 1
        fi
        
        echo "=== Building Standalone Application ==="
        echo "App: $APP_NAME, Configuration: $CONFIGURATION"
        
        PROJECT_PATH="Src/SES/BlazorWebUI/BlazorWebUI"
        
        if [ ! -d "$PROJECT_PATH" ]; then
            echo "Error: Project directory not found: $PROJECT_PATH"
            exit 1
        fi
        
        cd "$PROJECT_PATH"
        
        echo "Restoring dependencies..."
        dotnet restore
        if [ $? -ne 0 ]; then
            echo "Error: Failed to restore dependencies"
            exit 1
        fi
        
        echo "Building application..."
        dotnet build -c "$CONFIGURATION"
        if [ $? -ne 0 ]; then
            echo "Error: Build failed"
            exit 1
        fi
        
        PUBLISH_PATH="../../../../publish/$APP_NAME"
        mkdir -p "$PUBLISH_PATH"
        
        echo "Publishing application..."
        dotnet publish -c "$CONFIGURATION" -o "$PUBLISH_PATH"
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
        echo "  dotnet BlazorWebUI.dll --urls \"http://localhost:$PORT_HTTP;https://localhost:$PORT_HTTPS\""
        echo ""
        echo "Or run with specific profile from project directory:"
        echo "  cd Src/SES/BlazorWebUI/BlazorWebUI"
        echo "  dotnet run --launch-profile Release --urls \"http://localhost:$PORT_HTTP;https://localhost:$PORT_HTTPS\""
        ;;
        
    *)
        echo "Error: Invalid mode '$MODE'"
        show_usage
        exit 1
        ;;
esac

echo "Script completed successfully!"
