@ECHO off

dotnet build .\MasterData.Generator\MasterData.Generator.csproj -c Release /property:WarningLevel=0
dotnet build .\MasterData.Convertor\MasterData.Convertor.csproj -c Release /property:WarningLevel=0

dotnet publish .\MasterData.Generator\MasterData.Generator.csproj -c Release -r win-x64 --self-contained=false /p:PublishSingleFile=true /property:WarningLevel=0
dotnet publish .\MasterData.Convertor\MasterData.Convertor.csproj -c Release -r win-x64 --self-contained=false /p:PublishSingleFile=true /property:WarningLevel=0

XCOPY /Y .\MasterData.Convertor\bin\Release\net6.0\win-x64\publish\*.exe ..\DesignData\exe
XCOPY /Y .\MasterData.Generator\bin\Release\net6.0\win-x64\publish\*.exe ..\DesignData\exe

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE