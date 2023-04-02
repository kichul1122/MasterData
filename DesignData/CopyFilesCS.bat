@ECHO off

SET inputDirectory=%~dp0cs
SET outputDirectory=%~dp0..\Unity\Assets\_Scripts\MasterData

DEL /Q /F %outputDirectory%
ROBOCOPY "%inputDirectory%" "%outputDirectory%" "*.cs" /E /NDL /NJH /NJS /nc /ns /np

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE