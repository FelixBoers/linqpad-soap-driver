rem
rem  You can simplify development by updating this batch file and then calling it from the 
rem  project's post-build event.
rem
rem  It copies the output .DLL (and .PDB) to LINQPad's drivers folder, so that LINQPad
rem  picks up the drivers immediately (without needing to click 'Add Driver').
rem
rem  NB: The target directory may not be correct for your computer!
rem  You can obtain the first part of the directory by running the following query:
rem

echo Hier bin ich: %cd%

xcopy /i/y header.xml "%localappdata%\LINQPad\Drivers\DataContext\4.6\SoapContextDriver (a2ca9cc4ad783add)\"
xcopy /i/y SoapContextDriver.dll "%localappdata%\LINQPad\Drivers\DataContext\4.6\SoapContextDriver (a2ca9cc4ad783add)\"
xcopy /i/y SoapContextDriver.pdb "%localappdata%\LINQPad\Drivers\DataContext\4.6\SoapContextDriver (a2ca9cc4ad783add)\"
