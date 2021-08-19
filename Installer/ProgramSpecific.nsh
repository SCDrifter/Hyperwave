!define APP_VERSION "1.0.1"
!define APP_NAME "Hyperwave"
!define APP_TITLE "Hyperwave Eve-Mail Client"
!define APP_EXE "Hyperwave.exe"
!define APP_GUID "{1614da96-8ce4-4eda-aee5-1c294841fe02}"

Function .OnInit
	InitPluginsDir
	StrCpy $INSTDIR "$LOCALAPPDATA\Programs\Zukalitech\Hyperwave"
	SetShellVarContext current
FunctionEnd

Function Registry
	DetailPrint "Registering Url Handler"
	WriteRegStr HKCU "Software\Classes\eve-mail" "" "URL:Eve-Mail Protocol"
	WriteRegStr HKCU "Software\Classes\eve-mail" "URL Protocol" ""
	WriteRegStr HKCU "Software\Classes\eve-mail\DefaultIcon"  "" "$INSTDIR\Hyperwave.exe,1"
	WriteRegStr HKCU "Software\Classes\eve-mail\shell\open\command" "" '"$INSTDIR\Hyperwave.exe" "%1"'
FunctionEnd
 
Function un.Registry
	DetailPrint "Un-Registering Url Handler"
	DeleteRegKey HKCU "Software\Classes\eve-mail"
FunctionEnd


Function ShutdownProgram
	DetailPrint "Shutting down running instances"
	SetDetailsPrint none
	File /oname=$PLUGINSDIR\Hyperwave.Background.exe "..\Hyperwave\bin\x64\Release\Hyperwave.Background.exe"
	ExecWait '"$PLUGINSDIR\Hyperwave.Background.exe" --shutdown'
	${If} ${Errors}
		SetDetailsPrint both
		DetailPrint "Unable to shutdown instance"
		Abort
		Return
	${EndIf}
	Sleep 500
	SetDetailsPrint both
FunctionEnd

Function ResumeBackgroundService
	DetailPrint "Shutting down running instances"
	SetDetailsPrint none
	Exec "$INSTDIR\Hyperwave.Background.exe"
	SetDetailsPrint both
FunctionEnd


Function un.ShutdownProgram
	DetailPrint "Shutting down running instances"
	SetDetailsPrint none
	ExecWait '"$INSTDIR\Hyperwave.Background.exe" --shutdown'
	${If} ${Errors}
		SetDetailsPrint both
		DetailPrint "Unable to shutdown instance"
		Abort
		Return
	${EndIf}
	Sleep 500
	SetDetailsPrint both
FunctionEnd