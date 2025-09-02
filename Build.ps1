param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet("docker", "standalone")]
    [string]$Mode,
    
    [Parameter(Mandatory = $true, Position = 1)]
    [string]$AppName,
    
    [Parameter(Position = 2)]
    [string]$ArchitectureOrConfig,
    
    [Parameter(Position = 3)]
    [string]$CertPathOrEmpty,
    
    [Parameter(Position = 4)]
    [string]$CertPassword,
    
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

function Show-Usage {
    Write-Host "Usage: .\Build.ps1 <mode> <app_name> [options]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Modes:" -ForegroundColor Cyan
    Write-Host "  docker     - Build and run with Docker"
    Write-Host "  standalone - Build and run standalone .NET application"
    Write-Host ""
    Write-Host "Docker mode:" -ForegroundColor Cyan
    Write-Host "  .\Build.ps1 docker <app_name> <architecture> [cert_path] [cert_password]"
    Write-Host "  Example: .\Build.ps1 docker myapp amd64" -ForegroundColor Green
    Write-Host "  Example: .\Build.ps1 docker myapp amd64 .\cert.pfx mypassword" -ForegroundColor Green
    Write-Host ""
    Write-Host "Standalone mode:" -ForegroundColor Cyan
    Write-Host "  .\Build.ps1 standalone <app_name> [configuration]"
    Write-Host "  Example: .\Build.ps1 standalone myapp Release" -ForegroundColor Green
    Write-Host ""
    Write-Host "Architectures for Docker: amd64, arm64"
    Write-Host "Configurations for Standalone: Debug, Release (default: Release)"
}

try {
    switch ($Mode) {
        "docker" {
            $Architecture = $ArchitectureOrConfig
            $CertPath = $CertPathOrEmpty
            
            # Validate Docker parameters
            if (-not $Architecture) {
                Write-Host "Error: Architecture parameter required for Docker mode" -ForegroundColor Red
                Show-Usage
                exit 1
            }
            
            # Check if certificate is provided
            $useCertificate = $false
            if ($CertPath -and $CertPassword) {
                if (-not (Test-Path $CertPath)) {
                    Write-Host "Error: Certificate file not found: $CertPath" -ForegroundColor Red
                    exit 1
                }
                $useCertificate = $true
                Write-Host "Using certificate: $CertPath" -ForegroundColor Green
            }
            else {
                Write-Host "Running without HTTPS certificate (HTTP only)" -ForegroundColor Yellow
            }
            
            Write-Host "=== Building Docker image ===" -ForegroundColor Yellow
            Write-Host "App: $AppName, Architecture: $Architecture"
            
            # Build Docker image
            $buildArgs = @(
                "buildx", "build",
                "--platform", "linux/$Architecture",
                "-t", "${AppName}:$Architecture",
                "--load",
                "."
            )
            
            Write-Host "Running: docker $($buildArgs -join ' ')" -ForegroundColor Gray
            & docker @buildArgs
            
            if ($LASTEXITCODE -ne 0) {
                Write-Host "Error: Docker build failed" -ForegroundColor Red
                exit 1
            }
            
            Write-Host "=== Docker Build finished ===" -ForegroundColor Green
            
            # Stop and remove existing container if any
            $existingContainer = docker ps -aq --filter "name=^/${AppName}$"
            if ($existingContainer) {
                Write-Host "Stopping existing container $AppName..." -ForegroundColor Yellow
                docker stop $AppName | Out-Null
                docker rm $AppName | Out-Null
            }
            
            # Get absolute path for certificate if provided
            $volumeMount = ""
            $certEnvVars = @()
            
            if ($useCertificate) {
                $certAbsolutePath = Resolve-Path $CertPath
                $volumeMount = "-v"
                $certEnvVars = @(
                    "-e", "ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx",
                    "-e", "ASPNETCORE_Kestrel__Certificates__Default__Password=$CertPassword"
                )
            }
            
            # Run Docker container
            Write-Host "=== Running container $AppName ($Architecture) ===" -ForegroundColor Yellow
            
            $runArgs = @(
                "run", "-d", "--name", $AppName,
                "-p", "8080:8080", "-p", "8085:8085",
                "-e", "ASPNETCORE_URLS=http://+:8080;https://+:8085"
            )
            
            # Add certificate mounting if provided
            if ($useCertificate) {
                $runArgs += @("-v", "${certAbsolutePath}:/https/cert.pfx:ro")
                $runArgs += $certEnvVars
            }
            
            $runArgs += "${AppName}:$Architecture"
            
            & docker @runArgs
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "=== Container started successfully ===" -ForegroundColor Green
                Write-Host "Application is running at:" -ForegroundColor Cyan
                Write-Host "  HTTP:  http://localhost:8080" -ForegroundColor White
                if ($useCertificate) {
                    Write-Host "  HTTPS: https://localhost:8085" -ForegroundColor White
                }
                else {
                    Write-Host "  HTTPS: Not available (no certificate provided)" -ForegroundColor Yellow
                }
                Write-Host ""
                Write-Host "To view logs: docker logs $AppName" -ForegroundColor Gray
                Write-Host "To stop: docker stop $AppName" -ForegroundColor Gray
            }
            else {
                Write-Host "Error: Failed to start container" -ForegroundColor Red
                exit 1
            }
        }
        
        "standalone" {
            # Set configuration from parameter or use default
            if ($ArchitectureOrConfig -and ($ArchitectureOrConfig -eq "Debug" -or $ArchitectureOrConfig -eq "Release")) {
                $Configuration = $ArchitectureOrConfig
            }
            
            Write-Host "=== Building Standalone Application ===" -ForegroundColor Yellow
            Write-Host "App: $AppName, Configuration: $Configuration"
            
            # Navigate to project directory
            $projectPath = "Src\SES\BlazorWebUI\BlazorWebUI"
            
            if (-not (Test-Path $projectPath)) {
                Write-Host "Error: Project directory not found: $projectPath" -ForegroundColor Red
                exit 1
            }
            
            Push-Location $projectPath
            
            try {
                # Restore dependencies
                Write-Host "Restoring dependencies..." -ForegroundColor Cyan
                dotnet restore
                
                if ($LASTEXITCODE -ne 0) {
                    Write-Host "Error: Failed to restore dependencies" -ForegroundColor Red
                    exit 1
                }
                
                # Build application
                Write-Host "Building application..." -ForegroundColor Cyan
                dotnet build -c $Configuration
                
                if ($LASTEXITCODE -ne 0) {
                    Write-Host "Error: Build failed" -ForegroundColor Red
                    exit 1
                }
                
                # Create publish directory
                $publishPath = "..\..\..\..\publish\$AppName"
                if (-not (Test-Path $publishPath)) {
                    New-Item -Path $publishPath -ItemType Directory -Force | Out-Null
                }
                
                # Publish application
                Write-Host "Publishing application..." -ForegroundColor Cyan
                dotnet publish -c $Configuration -o $publishPath
                
                if ($LASTEXITCODE -ne 0) {
                    Write-Host "Error: Publish failed" -ForegroundColor Red
                    exit 1
                }
                
                Write-Host "=== Build completed successfully ===" -ForegroundColor Green
                Write-Host "Published to: publish\$AppName" -ForegroundColor Cyan
                Write-Host ""
                Write-Host "To run the application:" -ForegroundColor Yellow
                Write-Host "  cd publish\$AppName" -ForegroundColor White
                Write-Host "  dotnet BlazorWebUI.dll" -ForegroundColor White
                Write-Host ""
                Write-Host "Or run with specific profile from project directory:" -ForegroundColor Yellow
                Write-Host "  cd Src\SES\BlazorWebUI\BlazorWebUI" -ForegroundColor White
                Write-Host "  dotnet run --launch-profile Release  # Uses ports 8080/8085" -ForegroundColor White
            }
            finally {
                Pop-Location
            }
        }
        
        default {
            Write-Host "Error: Invalid mode '$Mode'" -ForegroundColor Red
            Show-Usage
            exit 1
        }
    }
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host "Script completed successfully!" -ForegroundColor Green