$TargetDir = "Build"
$TargetPath = (Get-Location).path
$Componenets=$args[0]

Remove-Item -Recurse -Force $TargetDir

New-Item -Path $TargetPath -Name $TargetDir -type directory -force
cd $TargetDir

git clone https://git.namecheap.net/scm/ncpl/nc.platform.presetup.git
cd nc.platform.presetup
#Invoke-RestMethod -OutFile "nc-install.ps1" -Uri "https://git.namecheap.net/projects/NCPL/repos/nc.platform.presetup/raw/App/nc-install.ps1?at=refs%2Fheads%2Fmaster"
#if ($Componenets){
.\publishToFolder.ps1 ..\ Autotests $Componenets
#}
#else
#{
#.\publishToFolder.ps1 ..\ Autotests
#}

cd ..
#.\nc-install -env Autotests -cmp "dns:*-rc*"

dotnet app.dll migrate -a Nc.Platform.ServiceDiscovery.DatabaseProvider -c "Data Source=(local);Initial Catalog=Nc_Platform_ServicesRegistry;User ID=sa;Password=1234567890-=q"
dotnet app.dll migrate -a Nc.Platform.ConfigurationProviders.DatabaseProvider -c "Data Source=(local);Initial Catalog=Nc_Platform_Configuration;User ID=sa;Password=1234567890-=q"
dotnet app.dll migrate -a Nc.Scheduler.Component -c "Data Source=(local);Initial Catalog=Nc_Platform_Scheduler;User ID=sa;Password=1234567890-=q"
dotnet app.dll migrate -a Nc.IdentityProvider.Component -c "Data Source=(local);Initial Catalog=Nc_Platform_Identity;User ID=sa;Password=1234567890-=q"

.\run

#read-host
