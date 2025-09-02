param(
    [Parameter(Position=0)]
    [ValidateSet("docker", "standalone", "dev")]
    [string]$Mode = "dev",
    
    [Parameter(Position=1)]
    [string]$AppNameOrProfile = "SES",
    
    [Parameter(Position=2)]
    [string]$ArchitectureOrHttpPort = "amd64",
    
    [Parameter(Position=3)]
    [string]$CertPathOrHttpsPort,
    
    [Parameter(Position=4)]
    [string]$CertPassword
)

function Show-Usage {
    Write-Host "Usage: .\Run.ps1 <mode> [options]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Modes:" -ForegroundColor Cyan
    Write-Host "  docker     - Run Docker container"
    Write-Host "  standalone - Run standalone .NET application"
    Write-Host "  dev        - Run in development mode"
    Write-Host ""
    Write-Host "Docker mode:" -ForegroundColor Cyan
    Write-Host "  .\Run.ps1 docker <app_name> <architecture> [cert_path] [cert_password]"
    Write-Host "  Example: .\Run.ps1 docker SES amd64" -ForegroundColor Green
    Write-Host "  Example: .\Run.ps1 docker SES amd64 .\cert.pfx mypassword" -ForegroundColor Green
    Write-Host ""
    Write-Host "Standalone mode:" -ForegroundColor Cyan
    Write-Host "  .\Run.ps1 standalone <app_name> [custom_port_http] [custom_port_https]"
    Write-Host "  Example: .\Run.ps1 standalone SES" -ForegroundColor Green
    Write-Host "  Example: .\Run.ps1 standalone SES 8080 8085" -ForegroundColor Green
    Write-Host ""
    Write-Host "Development mode:" -ForegroundColor Cyan
    Write-Host "  .\Run.ps1 dev [profile]"
    Write-Host "  Example: .\Run.ps1 dev https" -ForegroundColor Green
    Write-Host "  Example: .\Run.ps1 dev Release" -ForegroundColor Green
    Write-Host ""
    Write-Host "Available profiles: http, https, Release, ReleaseDocker, default"
}

try {
    switch ($Mode) {
        "docker" {
            $AppName = $AppNameOrProfile
            $Architecture = $ArchitectureOrHttpPort
            $CertPath = $CertPathOrHttpsPort
            
            if (-not $AppName -or -not $Architecture) {
                Write-Host "Error: App name and architecture required for Docker mode" -ForegroundColor Red
                Show-Usage
                exit 1
            }
            
            # Check if image exists
            $imageExists = docker image inspect "${AppName}:${Architecture}" 2>$null
            if ($LASTEXITCODE -ne 0) {
                Write-Host "Error: Docker image ${AppName}:${Architecture} not found" -ForegroundColor Red
                Write-Host "Build the image first with: .\Build.ps1 docker $AppName $Architecture" -ForegroundColor Yellow
                exit 1
            }
            
            # Stop and remove existing container if any
            $existingContainer = docker ps -aq --filter "name=^/${AppName}$"
            if ($existingContainer) {
                Write-Host "Stopping existing container $AppName..." -ForegroundColor Yellow
                docker stop $AppName | Out-Null
                docker rm $AppName | Out-Null
            }
            
            Write-Host "=== Running container $AppName ($Architecture) ===" -ForegroundColor Yellow
            
            $runArgs = @(
                "run", "-d", "--name", $AppName,
                "-p", "8080:8080", "-p", "8085:8085",
                "-e", "ASPNETCORE_URLS=http://+:8080;https://+:8085"
            )
            
            # Add certificate if provided
            $useCertificate = $false
            if ($CertPath -and $CertPassword) {
                if (-not (Test-Path $CertPath)) {
                    Write-Host "Error: Certificate file not found: $CertPath" -ForegroundColor Red
                    exit 1
                }
                
                $certAbsolutePath = Resolve-Path $CertPath
                $runArgs += @("-v", "${certAbsolutePath}:/https/cert.pfx:ro")
                $runArgs += @(
                    "-e", "ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx",
                    "-e", "ASPNETCORE_Kestrel__Certificates__Default__Password=$CertPassword"
                )
                $useCertificate = $true
                Write-Host "Using certificate: $CertPath" -ForegroundColor Green
            } else {
                Write-Host "Running without HTTPS certificate (HTTP only)" -ForegroundColor Yellow
            }
            
            $runArgs += "${AppName}:$Architecture"
            
            & docker @runArgs
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "=== Container started successfully ===" -ForegroundColor Green
                Write-Host "Application is running at:" -ForegroundColor Cyan
                Write-Host "  HTTP:  http://localhost:8080" -ForegroundColor White
                if ($useCertificate) {
                    Write-Host "  HTTPS: https://localhost:8085" -ForegroundColor White
                } else {
                    Write-Host "  HTTPS: Not available (no certificate)" -ForegroundColor Yellow
                }
                Write-Host ""
                Write-Host "Useful commands:" -ForegroundColor Gray
                Write-Host "  docker logs $AppName -f        # View logs" -ForegroundColor White
                Write-Host "  docker stop $AppName           # Stop container" -ForegroundColor White
                Write-Host "  docker exec -it $AppName sh    # Enter container" -ForegroundColor White
            } else {
                Write-Host "Error: Failed to start container" -ForegroundColor Red
                exit 1
            }
        }
        
        "standalone" {
            $AppName = $AppNameOrProfile
            $CustomPortHttp = $ArchitectureOrHttpPort
            $CustomPortHttps = $CertPathOrHttpsPort
            
            if (-not $AppName) {
                Write-Host "Error: App name required for standalone mode" -ForegroundColor Red
                Show-Usage
                exit 1
            }
            
            # Check if published app exists
            $publishPath = "publish\$AppName"
            if (-not (Test-Path $publishPath)) {
                Write-Host "Error: Published application not found: $publishPath" -ForegroundColor Red
                Write-Host "Build the application first with: .\Build.ps1 standalone $AppName" -ForegroundColor Yellow
                exit 1
            }
            
            $dllPath = "$publishPath\BlazorWebUI.dll"
            if (-not (Test-Path $dllPath)) {
                Write-Host "Error: BlazorWebUI.dll not found in $publishPath" -ForegroundColor Red
                exit 1
            }
            
            Write-Host "=== Running standalone application ===" -ForegroundColor Yellow
            
            # Set custom ports if provided
            if ($CustomPortHttp) {
                if ($CustomPortHttps) {
                    $env:ASPNETCORE_URLS = "http://localhost:${CustomPortHttp};https://localhost:${CustomPortHttps}"
                    Write-Host "Using custom ports: HTTP=$CustomPortHttp, HTTPS=$CustomPortHttps" -ForegroundColor Green
                } else {
                    $env:ASPNETCORE_URLS = "http://localhost:${CustomPortHttp}"
                    Write-Host "Using custom HTTP port: $CustomPortHttp" -ForegroundColor Green
                }
            } else {
                $env:ASPNETCORE_URLS = "http://localhost:8080;https://localhost:8085"
                Write-Host "Using default ports: HTTP=8080, HTTPS=8085" -ForegroundColor Green
            }
            
            # Set production environment
            $env:ASPNETCORE_ENVIRONMENT = "Production"
            
            Write-Host "Starting $AppName..." -ForegroundColor Cyan
            Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
            Write-Host ""
            
            Push-Location $publishPath
            try {
                & dotnet "BlazorWebUI.dll"
            }
            finally {
                Pop-Location
            }
        }
        
        "dev" {
            $Profile = if ($AppNameOrProfile -and $AppNameOrProfile -ne "SES") { $AppNameOrProfile } else { "https" }
            
            # Check if we're in the right directory
            if (-not (Test-Path "Src\SES\BlazorWebUI\BlazorWebUI\BlazorWebUI.csproj")) {
                Write-Host "Error: Please run this script from the solution root directory" -ForegroundColor Red
                exit 1
            }
            
            Write-Host "=== Running in Development mode ===" -ForegroundColor Yellow
            Write-Host "Profile: $Profile" -ForegroundColor Cyan
            Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
            Write-Host ""
            
            Push-Location "Src\SES\BlazorWebUI\BlazorWebUI"
            
            try {
                if ($Profile -eq "default") {
                    & dotnet run
                } else {
                    & dotnet run --launch-profile $Profile
                }
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
