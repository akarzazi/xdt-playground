[CmdletBinding(PositionalBinding = $false)]
param(
    [switch] $Publish,
    [switch] $Npm
)

Write-Host "Run Parameters:" -ForegroundColor Cyan
Write-Host "  Publish: $Publish"
Write-Host "  dotnet --version:" (dotnet --version)

$publishDir = "$PSScriptRoot\docs"
$publishProfileOut = "$PSScriptRoot\src\XdtPlayground\bin\publish-gh-pages\wwwroot"
$npmBuildDir = "$PSScriptRoot\src\XdtPlayground\Monaco"

# ----------NPM BUILD------------
if ($Npm) {
    Write-Host "npm build" -ForegroundColor "Magenta"
    Push-Location -Path "$npmBuildDir" -PassThru
    npm install
    npm run build-prod
    if ($LastExitCode -ne 0) {
        Write-Host "Error with Build Monaco, aborting build." -Foreground "Red"
        pop-Location -PassThru
        Exit 1
    }
    pop-Location -PassThru
}

# ----------.NET BUILD------------
Write-Host "Restoring all projects..." -ForegroundColor "Magenta"
dotnet restore
Write-Host "Done restoring." -ForegroundColor "Green"

Write-Host "Building all projects..." -ForegroundColor "Magenta"
dotnet build -c Release --no-restore
Write-Host "Done building." -ForegroundColor "Green"

# ----------PUBLISH------------
if ($Publish) {

    mkdir -Force $publishDir | Out-Null
    Write-Host "Clearing existing $publishDir..." -NoNewline
    Remove-Item  $publishDir -Recurse -Force
    Write-Host "done." -ForegroundColor "Green"

    Write-Host "Publishing ..." -ForegroundColor "Magenta"
    dotnet build -c Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile

    Write-Host "Copy from $publishProfileOut to $publishDir" -ForegroundColor "Magenta"
    Copy-Item -Path $publishProfileOut -Destination $publishDir -Recurse

    #Write-Host "Delete dir '$publishDir\_framework\_bin' $" -ForegroundColor "Magenta"
   # Remove-Item "$publishDir\_framework\_bin" -Recurse -Force

    Write-Host "Delete *.br , *.gz"
    Remove-Item $publishDir  -Recurse -Force -Include *.br , *.gz
}

Write-Host "Build Complete." -ForegroundColor "Green"
