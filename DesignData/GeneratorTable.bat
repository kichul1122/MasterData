@ECHO off

SET inputDirectory=%~dp0cs
SET outputDirectory=%~dp0table

PUSHD  exe
DEL /Q /F %outputDirectory%
CALL ".\MasterMemory.Generator.exe" -i %inputDirectory% -o %outputDirectory% -n KC -f
POPD 

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE