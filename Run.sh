#!/bin/bash

# Function to show usage
show_usage() {
    echo "Usage: ./Run.sh <mode> <app_name> [options]"
    echo ""
    echo "Modes:"
    echo "  docker     - Run Docker container"
    echo "  standalone - Run standalone .NET application"
    echo "  dev        - Run in development mode"
    echo ""
    echo "Docker mode:"
    echo "  ./Run.sh docker <app_name> <architecture> [cert_path] [cert_password]"
    echo "  Example: ./Run.sh docker SES amd64"
    echo "  Example: ./Run.sh docker SES amd64 ./cert.pfx mypassword"
    echo ""
    echo "Standalone mode:"
    echo "  ./Run.sh standalone <app_name> [custom_port_http] [custom_port_https]"
    echo "  Example: ./Run.sh standalone SES"
    echo "  Example: ./Run.sh standalone SES 8080 8085"
    echo ""
    echo "Development mode:"
    echo "  ./Run.sh dev [profile]"
    echo "  Example: ./Run.sh dev https"
    echo "  Example: ./Run.sh dev Release"
    echo ""
    echo "Available profiles: http, https, Release, ReleaseDocker"
}

# Get parameters with defaults
MODE=${1:-"dev"}
APP_NAME=${2:-"SES"}

# Validate basic parameters
if [ -z "$MODE" ]; then
    show_usage
    exit 1
fi

case "$MODE" in
    "docker")
        ARCHITECTURE=${3:-"amd64"}
        CERT_PATH=$4
        CERT_PASSWORD=$5
        
        # Check if image exists
        if ! docker image inspect "${APP_NAME}:${ARCHITECTURE}" >/dev/null 2>&1; then
            echo "Error: Docker image ${APP_NAME}:${ARCHITECTURE} not found"
            echo "Build the image first with: ./Build.sh docker $APP_NAME $ARCHITECTURE"
            exit 1
        fi
        
        # Stop and remove existing container if any
        EXISTING_CONTAINER=$(docker ps -aq --filter "name=^/${APP_NAME}$")
        if [ -n "$EXISTING_CONTAINER" ]; then
            echo "Stopping existing container $APP_NAME..."
            docker stop "$APP_NAME" >/dev/null 2>&1
            docker rm "$APP_NAME" >/dev/null 2>&1
        fi
        
        echo "=== Running container $APP_NAME ($ARCHITECTURE) ==="
        
        # Prepare Docker run command
        DOCKER_ARGS="run -d --name $APP_NAME -p 8080:8080 -p 8085:8085"
        
        # Add certificate if provided
        if [ -n "$CERT_PATH" ] && [ -n "$CERT_PASSWORD" ]; then
            if [ ! -f "$CERT_PATH" ]; then
                echo "Error: Certificate file not found: $CERT_PATH"
                exit 1
            fi
            
            CERT_ABSOLUTE_PATH=$(realpath "$CERT_PATH")
            DOCKER_ARGS="$DOCKER_ARGS -v $CERT_ABSOLUTE_PATH:/https/cert.pfx:ro"
            DOCKER_ARGS="$DOCKER_ARGS -e ASPNETCORE_Kestrel__Certificates__Default__Password=$CERT_PASSWORD"
            echo "Using certificate: $CERT_PATH"
        else
            echo "Running without HTTPS certificate (HTTP only)"
        fi
        
        DOCKER_ARGS="$DOCKER_ARGS -e ASPNETCORE_URLS=http://+:8080;https://+:8085 ${APP_NAME}:${ARCHITECTURE}"
        
        # Run container
        eval "docker $DOCKER_ARGS"
        
        if [ $? -eq 0 ]; then
            echo "=== Container started successfully ==="
            echo "Application is running at:"
            echo "  HTTP:  http://localhost:8080"
            if [ -n "$CERT_PATH" ] && [ -n "$CERT_PASSWORD" ]; then
                echo "  HTTPS: https://localhost:8085"
            else
                echo "  HTTPS: Not available (no certificate)"
            fi
            echo ""
            echo "Useful commands:"
            echo "  docker logs $APP_NAME -f    # View logs"
            echo "  docker stop $APP_NAME       # Stop container"
            echo "  docker exec -it $APP_NAME sh # Enter container"
        else
            echo "Error: Failed to start container"
            exit 1
        fi
        ;;
        
    "standalone")
        CUSTOM_PORT_HTTP=$3
        CUSTOM_PORT_HTTPS=$4
        
        # Check if published app exists
        PUBLISH_PATH="publish/$APP_NAME"
        if [ ! -d "$PUBLISH_PATH" ]; then
            echo "Error: Published application not found: $PUBLISH_PATH"
            echo "Build the application first with: ./Build.sh standalone $APP_NAME"
            exit 1
        fi
        
        if [ ! -f "$PUBLISH_PATH/BlazorWebUI.dll" ]; then
            echo "Error: BlazorWebUI.dll not found in $PUBLISH_PATH"
            exit 1
        fi
        
        echo "=== Running standalone application ==="
        
        # Set custom ports if provided
        if [ -n "$CUSTOM_PORT_HTTP" ]; then
            if [ -n "$CUSTOM_PORT_HTTPS" ]; then
                export ASPNETCORE_URLS="http://localhost:${CUSTOM_PORT_HTTP};https://localhost:${CUSTOM_PORT_HTTPS}"
                echo "Using custom ports: HTTP=$CUSTOM_PORT_HTTP, HTTPS=$CUSTOM_PORT_HTTPS"
            else
                export ASPNETCORE_URLS="http://localhost:${CUSTOM_PORT_HTTP}"
                echo "Using custom HTTP port: $CUSTOM_PORT_HTTP"
            fi
        else
            export ASPNETCORE_URLS="http://localhost:8080;https://localhost:8085"
            echo "Using default ports: HTTP=8080, HTTPS=8085"
        fi
        
        # Set production environment
        export ASPNETCORE_ENVIRONMENT=Production
        
        echo "Starting $APP_NAME..."
        echo "Press Ctrl+C to stop"
        echo ""
        
        cd "$PUBLISH_PATH"
        dotnet BlazorWebUI.dll
        ;;
        
    "dev")
        PROFILE=${APP_NAME:-"https"}  # APP_NAME becomes profile when mode is dev, default to https
        
        # Check if we're in the right directory
        if [ ! -f "Src/SES/BlazorWebUI/BlazorWebUI/BlazorWebUI.csproj" ]; then
            echo "Error: Please run this script from the solution root directory"
            exit 1
        fi
        
        echo "=== Running in Development mode ==="
        echo "Profile: $PROFILE"
        echo "Press Ctrl+C to stop"
        echo ""
        
        cd "Src/SES/BlazorWebUI/BlazorWebUI"
        
        # Run with specified profile
        if [ "$PROFILE" = "default" ]; then
            dotnet run
        else
            dotnet run --launch-profile "$PROFILE"
        fi
        ;;
        
    *)
        echo "Error: Invalid mode '$MODE'"
        show_usage
        exit 1
        ;;
esac
