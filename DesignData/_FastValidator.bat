@ECHO off

SET inputDirectory=%~dp0json
PUSHD  exe
CALL ".\MasterData.Validator.exe" excutedirectory -i %inputDirectory%
POPD 

GOTO PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE