@ECHO off

PUSHD  exe

SET inputDirectory=%~dp0
SET outputDirectory=%~dp0cs
DEL /Q /F %outputDirectory%
CALL ".\MasterData.Generator.exe" excutedirectory -i %inputDirectory% -o %outputDirectory% -n KC

SET inputDirectory=%~dp0cs
SET outputDirectory=%~dp0table
DEL /Q /F %outputDirectory%
CALL ".\MasterMemory.Generator.exe" -i %inputDirectory% -o %outputDirectory% -n KC -f

SET inputDirectory=%~dp0
SET outputDirectory=%~dp0json
DEL /Q /F %outputDirectory%
CALL ".\MasterData.Convertor.exe" excutedirectory -i %inputDirectory% -o %outputDirectory% -n KC

POPD 

SET inputDirectory=%~dp0cs
SET outputDirectory=%~dp0..\Unity\Assets\_Scripts\MasterData
DEL /Q /F %outputDirectory%
ROBOCOPY "%inputDirectory%" "%outputDirectory%" "*.cs" /E /NDL /NJH /NJS /nc /ns /np

SET inputDirectory=%~dp0table
SET outputDirectory=%~dp0..\Unity\Assets\_Scripts\MasterData\Table
DEL /Q /F /S  %outputDirectory%
ROBOCOPY "%inputDirectory%" "%outputDirectory%" "*.cs" /E /NDL /NJH /NJS /nc /ns /np

SET inputDirectory=%~dp0json
SET outputDirectory=%~dp0..\Unity\Assets\Resources\MasterData
DEL /Q /F %outputDirectory%
ROBOCOPY "%inputDirectory%" "%outputDirectory%" "*.json" /E /NDL /NJH /NJS /nc /ns /np

GOTO SKIP_PAUSE

:PAUSE
PAUSE

:SKIP_PAUSE