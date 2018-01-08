$toolsDir = 'tools'

Remove-Item $toolsDir\*.* -Force -Recurse -ErrorAction SilentlyContinue

iex 'dotnet restore --packages $toolsDir tools.project.json'

Get-ChildItem -Path .\$toolsDir -Filter *.exe -Recurse |
	ForEach-Object {
		Write-Host 'Running ' $_.FullName
		Start-Process $_.FullName -ArgumentList --no-dump
	}