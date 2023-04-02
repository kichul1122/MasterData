@ECHO off

SET inputDirectory=%~dp0
SET outputDirectory=%~dp0cs

PUSHD  exe
DEL /Q /F %outputDirectory%
CALL ".\MasterData.Generator.exe" excutedirectory -i %inputDirectory% -o %outputDirectory% -n KC
POPD 

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE