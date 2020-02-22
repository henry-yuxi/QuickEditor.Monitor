@echo off

set PROJ_GIT_PATH=D:\GitProjects\QuickEditor.Monitor
set UPM_PREFIX=QuickEditor.Monitor/Assets/QuickEditor/QuickEditor.Monitor
set UPM_TAG=0.1.5
d:
cd /d %PROJ_GIT_PATH%
git subtree split --prefix=%UPM_PREFIX% --branch upm
git tag %UPM_TAG% upm       
git push origin upm --tags
@cmd.exe