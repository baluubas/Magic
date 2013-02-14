@echo off
cd %~dp0

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe ..\Source\Magic.sln /t:Clean,Build /p:OutDir="%~dp0\bin" /p:PlatformTarget=x86 /p:Configuration=Release"
..\ThirdParty\WiX\candle Magic.wxs
..\ThirdParty\WiX\light Magic.wixobj

..\ThirdParty\WiX\candle -ext ..\ThirdParty\WiX\sdk\WixUtilExtension.dll -ext ..\ThirdParty\WiX\sdk\WixBalExtension.dll Bootstrapper.wxs
..\ThirdParty\WiX\light -ext ..\ThirdParty\WiX\sdk\WixUtilExtension.dll -ext ..\ThirdParty\WiX\sdk\WixBalExtension.dll Bootstrapper.wixobj

rmdir "bin" /s /q
del Magic.wixobj
del Magic.wixpdb
del Magic.msi
del Bootstrapper.wixobj
del Bootstrapper.wixpdb

move Bootstrapper.exe MagicInstall.exe

pause