SET NUNIT_PATH="C:\Program Files (x86)\NUnit.org\nunit-console"
SET TESTS_PATH="D:\Proj\TestRailIntegratoionV2\TestSuiteNetFramework\bin\Debug"
REM SET TESTS_LIST_PATH="D:\Proj\TestRailIntegratoionV2\TestSuiteCoreNunit\TestsList.txt"
SET TESTS_LIST_PATH="D:\Proj\TestRailIntegratoionV2\TestSuiteNetFramework\bin\Debug\TestsList.txt"

rem nunit3-console.exe %TESTS_PATH% --where:method==DomainUiLive.AccountPanel.Dashboard.Smoke.LinkAccountBalanceTopUp
rem nunit3-console.exe %TESTS_PATH% --where:cat==SmokeTest,FunctionalTest
rem nunit3-console.exe %TESTS_PATH% --where:cat==SmokeTestDashboard

c:
cd %NUNIT_PATH%
nunit3-console.exe %TESTS_PATH%\TestSuiteNetFramework.dll --testlist=%TESTS_LIST_PATH% --work=%TESTS_PATH% --result=nunit2

REM pause
