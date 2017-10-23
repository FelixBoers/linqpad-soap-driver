@ECHO OFF
SET MsBuild14Dir="%PROGRAMFILES(X86)%\MSBuild\14.0\Bin"
SET MsBuild12Dir="%PROGRAMFILES(X86)%\MSBuild\12.0\Bin"

IF NOT EXIST %MsBuild14Dir%\* GOTO MSBUILD_14_NOT_FOUND
SET MsBuildDir=%MsBuild14Dir%

:BUILD

CALL %MSBuildDir%\MsBuild.exe SoapContextDriver.sln /nologo /verbosity:minimal /p:Configuration="Release" /p:Platform="x86" /p:LinqPadVersion=4 /t:Rebuild
CALL %MSBuildDir%\MsBuild.exe SoapContextDriver.sln /nologo /verbosity:minimal /p:Configuration="Release" /p:Platform="AnyCPU" /p:LinqPadVersion=4 /t:Rebuild
CALL %MSBuildDir%\MsBuild.exe SoapContextDriver.sln /nologo /verbosity:minimal /p:Configuration="Release" /p:Platform="x86" /p:LinqPadVersion=5 /t:Rebuild
CALL %MSBuildDir%\MsBuild.exe SoapContextDriver.sln /nologo /verbosity:minimal /p:Configuration="Release" /p:Platform="AnyCPU" /p:LinqPadVersion=5 /t:Rebuild

IF ERRORLEVEL 1 GOTO ERROR

GOTO END

:MSBUILD_14_NOT_FOUND
IF NOT EXIST %MSbuild12Dir%\* GOTO MSBUILD_NOT_FOUND
SET MsBuildDir=%MsBuild12Dir%
GOTO Build

:MSBUILD_NOT_FOUND
ECHO MSBuild.exe not found in %MsBuild14Dir%;%MsBuild12Dir%
PAUSE
EXIT /b -1

:ERROR
ECHO Build exit with code %ERRORLEVEL%
PAUSE

:END
PAUSE