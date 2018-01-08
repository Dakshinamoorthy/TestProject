#Write-Host "This is a message"

$TargetDir = ".\Deployment\ConfigTransformation\nc.platform.presetup"
$TargetPath = (Get-Location).path
$buildDir=$TargetPath
#$Componenets=$args[0]

$platformV = "1.5.0-*"
$env = $args[0]
$dir = $args[1]
#$dir = "../../../.."
#$env = "Autotest"

# get config processor arguments
$cmpArgs="`"src:$dir`" `"v_ncPlatform.version:$platformV`" `"v:$TargetDir/config.default.variables.xml`""
if ($env -and $env.Trim() -ne ""){
      $cmpArgs += " `"v:$TargetDir/Variables/$env/config.default.variables.xml`""
}

#Remove-Item -Recurse -Force $TargetDir
#New-Item -Path $TargetPath -Name $TargetDir -type directory -force
#cd $TargetDir

git clone https://git.namecheap.net/scm/ncpl/nc.platform.presetup.git
cd nc.platform.presetup

# PROCESS CONFIG
cd $buildDir
#create project.json for config processor
New-Item 'project.json' -type file -force -value '{"frameworks":{ "netstandard1.6":{"dependencies":{"NETStandard.Library":"1.6.0"}}},"tools":{"dotnet-nc-cproc":"2.0.0-*"}}'
#create nuget.config for config processor
New-Item 'NuGet.config' -type file -force -value '<?xml version="1.0" encoding="utf-8"?><configuration><packageSources><clear /><add key="rabbitmq pre" value="https://ci.appveyor.com/nuget/rabbitmq-dotnet-client-ci" /><add key="nc.proget" value="https://proget.corp.namecheap.net/nuget/namecheap"/></packageSources><disabledPackageSources /></configuration>'
Start-Process dotnet -ArgumentList 'restore project.json' -NoNewWindow -Wait
powershell -command "& dotnet nc-cproc --%" "$cmpArgs"
#cd ..
