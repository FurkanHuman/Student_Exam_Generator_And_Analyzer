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

cd ..

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
        
        echo "=== Building Docker image ==="
        echo "App: $APP_NAME, Architecture: $ARCHITECTURE"
        
        # Build Docker image
        echo "Running: docker buildx build --platform linux/$ARCHITECTURE -t $APP_NAME:$ARCHITECTURE --load ."
        docker buildx build \
            --platform "linux/$ARCHITECTURE" \
            -t "$APP_NAME:$ARCHITECTURE" \
            -f ./.Dockerfile \
            --load \
            .
        
        if [ $? -ne 0 ]; then
            echo "Error: Docker build failed"
            exit 1
        fi
        
        echo "=== Docker Build finished ==="
        ;;
        
    "standalone")
        CONFIGURATION=${3:-Release}
        
        # Validate configuration
        if [ "$CONFIGURATION" != "Debug" ] && [ "$CONFIGURATION" != "Release" ]; then
            echo "Error: Invalid configuration '$CONFIGURATION'. Use Debug or Release"
            show_usage
            exit 1
        fi
        
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
        
        # Create publish directory
        PUBLISH_PATH="../../../../publish/$APP_NAME"
        mkdir -p "$PUBLISH_PATH"
        
        # Publish application
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
        echo "  dotnet BlazorWebUI.dll"
        echo ""
        echo "Or run with specific profile from project directory:"
        echo "  cd Src/SES/BlazorWebUI/BlazorWebUI"
        echo "  dotnet run --launch-profile Release  # Uses ports 8080/8085"
        ;;
        
    *)
        echo "Error: Invalid mode '$MODE'"
        show_usage
        exit 1
        ;;
esac

echo "Script completed successfully!"