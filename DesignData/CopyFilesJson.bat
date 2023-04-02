@ECHO off

SET inputDirectory=%~dp0json
SET outputDirectory=%~dp0..\Unity\Assets\Resources\MasterData

DEL /Q /F %outputDirectory%
ROBOCOPY "%inputDirectory%" "%outputDirectory%" "*.json" /E /NDL /NJH /NJS /nc /ns /np

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE