[CmdletBinding(PositionalBinding = $false)]
param(
    [switch] $Publish
)

Write-Host "Run Parameters:" -ForegroundColor Cyan
Write-Host "  Publish: $Publish"
Write-Host "  dotnet --version:" (dotnet --version)

$publishDir = "$PSScriptRoot\docs"
$publishProfileOut = "$PSScriptRoot\src\XdtPlayground\bin\publish-gh-pages"

Write-Host "Restoring all projects..." -ForegroundColor "Magenta"
dotnet restore
Write-Host "Done restoring." -ForegroundColor "Green"

Write-Host "Building all projects..." -ForegroundColor "Magenta"
dotnet build -c Release --no-restore
Write-Host "Done building." -ForegroundColor "Green"

# ----------TESTS------------
if ($RunTest) {
    Write-Host "Running tests: " -ForegroundColor "Magenta"
    dotnet test -c Release 
    if ($LastExitCode -ne 0) {
        Write-Host "Error with tests, aborting build." -Foreground "Red"
        Exit 1
    }
    Write-Host "Tests passed!" -ForegroundColor "Green"
}

# ----------PUBLISH------------
if ($Publish) {
    mkdir -Force $publishDir | Out-Null
    Write-Host "Clearing existing $publishDir..." -NoNewline
    Get-ChildItem $publishDir | Remove-Item
    Write-Host "done." -ForegroundColor "Green"

    Write-Host "Publishing ..." -ForegroundColor "Magenta"
    dotnet build -c Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile

    Write-Host "Copy from $publishProfileOut to $publishDir"
    Copy-Item -Path "$publishProfileOut\*" -Destination "$publishDir" -Recurse

    dotnet pack ".\src\XdtPlayground\bin\publish-gh-pages\" --no-build -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -c Release /p:PackageOutputPath=$packageOutputFolder
}
Write-Host "Build Complete." -ForegroundColor "Green"
