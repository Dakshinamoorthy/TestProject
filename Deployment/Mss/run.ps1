$TargetDir = "Build"
$TargetPath = (Get-Location).path
New-Item -Path $TargetPath -Name $TargetDir -type directory -force
cd Build

.\run

# read-host
