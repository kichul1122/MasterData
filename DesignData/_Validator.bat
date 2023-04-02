@ECHO off

PUSHD ..\ConsoleApp
dotnet build .\MasterData.Validator\MasterData.Validator.csproj -c Release /property:WarningLevel=0
dotnet publish .\MasterData.Validator\MasterData.Validator.csproj -c Release -r win-x64 --self-contained=false /p:PublishSingleFile=true /property:WarningLevel=0
XCOPY /Y .\MasterData.Validator\bin\Release\net6.0\win-x64\publish\*.exe ..\DesignData\exe
POPD

SET inputDirectory=%~dp0json
PUSHD  exe
CALL ".\MasterData.Validator.exe" excutedirectory -i %inputDirectory%
POPD 

GOTO PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE