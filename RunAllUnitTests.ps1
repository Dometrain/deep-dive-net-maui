 # Define the current folder as the search folder
 $SearchFolder = "$(pwd)"

 # Recursively find all `.csproj` files
 Write-Host "Searching for test project files (*.csproj) in '$SearchFolder'..." -ForegroundColor Cyan
 $csprojFiles = Get-ChildItem -Path $SearchFolder -Recurse -Filter *.csproj -ErrorAction SilentlyContinue

 if ($csprojFiles.Count -eq 0) {
     Write-Host "No test project files (*.csproj) found in '$SearchFolder'." -ForegroundColor Yellow
     exit 0
 }

 # Filter for test projects by checking for the inclusion of "xunit", "mstest", or "nunit" in the project file
 $testProjects = @()
 foreach ($csprojFile in $csprojFiles) {
     if (Select-String -Path $csprojFile.FullName -Pattern 'xunit|mstest|nunit' -Quiet) {
         $testProjects += $csprojFile
     }
 }

 if ($testProjects.Count -eq 0) {
     Write-Host "No test projects found in '$SearchFolder'." -ForegroundColor Yellow
     exit 0
 }

 # Define a flag to track errors
 $global:hasError = $false

 # Run tests for `.csproj` files in parallel
 $testProjects | ForEach-Object -Parallel {
     if ($global:hasError) { throw "Stopping due to previous error" }

     $testProject = $_
     try {
         Write-Host "Running tests for project: $($testProject)"
         $testOutput = dotnet test $testProject -c Release 2>&1

         if ($LASTEXITCODE -eq 0) {
             Write-Host "Tests succeeded for: $($testProject)" -ForegroundColor Green
         } else {
             Write-Host "Tests failed for: $($testProject)" -ForegroundColor Red
             Write-Host "Test output:" -ForegroundColor Yellow
             Write-Output $testOutput -ForegroundColor Yellow
             $global:hasError = $true
             throw "Tests failed for $testProject"
         }
     }
     catch {
         $global:hasError = $true
         Write-Host "Error running tests for $testProject" -ForegroundColor Red
     }
 }

 # Check if any tests failed and fail the workflow if necessary
 if ($global:hasError) {
     Write-Host "One or more tests failed." -ForegroundColor Red
     exit 1
 }

 Write-Host "Test process completed." -ForegroundColor Cyan
