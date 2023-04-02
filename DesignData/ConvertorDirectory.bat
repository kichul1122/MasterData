@ECHO off

SET inputDirectory=%~dp0
SET outputDirectory=%~dp0json

PUSHD  exe
DEL /Q /F %outputDirectory%
CALL ".\MasterData.Convertor.exe" excutedirectory -i %inputDirectory% -o %outputDirectory% -n KC
POPD 

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE