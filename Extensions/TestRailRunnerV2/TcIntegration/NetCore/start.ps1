Invoke-Command -ScriptBlock {dotnet test --test (Get-Content %FolderForTestRail%\TestsList.txt)}
