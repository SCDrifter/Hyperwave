Unicode True

!include "LogicLib.nsh"
!include "MUI2.nsh"
!include "ProgramSpecific.nsh"

Name "${APP_TITLE}"

RequestExecutionLevel user

;SetCompressor lzma
!define REG_UNINST "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}"

OutFile "${APP_NAME}-${APP_VERSION}.exe"

InstallDir "$PROGRAMFILES\${APP_NAME}"

!insertmacro MUI_PAGE_WELCOME

!define MUI_LICENSEPAGE_BUTTON "&Install"
!insertmacro MUI_PAGE_LICENSE "License.rtf"

!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_RUN "$INSTDIR\Hyperwave.exe"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

; Return on top of stack the total size of the selected (installed) sections, formated as DWORD
; Assumes no more than 256 sections are defined
Var GetInstalledSize.total
Function GetInstalledSize
    Push $0
    Push $1
    StrCpy $GetInstalledSize.total 0
    ${ForEach} $1 0 256 + 1
        ${if} ${SectionIsSelected} $1			
            SectionGetSize $1 $0
            IntOp $GetInstalledSize.total $GetInstalledSize.total + $0
        ${Endif}
 
        ; Error flag is set when an out-of-bound section is referenced
        ${if} ${errors}
            ${break}
        ${Endif}
    ${Next}
 
    ClearErrors
    Pop $1
    Pop $0
    IntFmt $GetInstalledSize.total "0x%08X" $GetInstalledSize.total
    Push $GetInstalledSize.total
FunctionEnd

Function CreateUnInstallEntry
    WriteRegStr     HKCU  "${REG_UNINST}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
    WriteRegStr     HKCU  "${REG_UNINST}" "QuietUninstallString" "$\"$INSTDIR\Uninstall.exe$\" /S"
    
    WriteRegStr     HKCU  "${REG_UNINST}" "DisplayName" "${APP_TITLE}"
    WriteRegStr     HKCU  "${REG_UNINST}" "DisplayIcon" '"$INSTDIR\${APP_EXE}",0'
    WriteRegStr     HKCU  "${REG_UNINST}" "DisplayVersion" "${APP_VERSION}"
    WriteRegStr     HKCU  "${REG_UNINST}" "Publisher" "NRT Technology"
    WriteRegDWORD   HKCU  "${REG_UNINST}" "NoModify" 1
    WriteRegDWORD   HKCU  "${REG_UNINST}" "NoRepair" 1
    
    Call GetInstalledSize
    Pop $0
    WriteRegDWORD   HKCU  "${REG_UNINST}" "EstimatedSize" $0
    
FunctionEnd

Function CreateStartMenu    
    CreateShortCut "$SMPROGRAMS\${APP_TITLE}.lnk" "$INSTDIR\${APP_EXE}"    
FunctionEnd

Function un.DelUnInstallEntry
    DeleteRegKey HKCU  "${REG_UNINST}"
FunctionEnd

Function un.DeleteStartMenu
    Delete  "$SMPROGRAMS\${APP_TITLE}.lnk"
FunctionEnd

Section Install
	Call ShutdownProgram
	
    SetDetailsPrint textonly
	DetailPrint "Installing Files..."
	SetDetailsPrint listonly
	SetOutPath $INSTDIR
	File /x "*.pdb" /x "*.vshost.*" /x "*.ipdb" /x "*.iobj" /x "NLog.config" "..\Hyperwave\bin\x64\Release\*.*"
	SetDetailsPrint both
	
    Call Registry
    Call CreateUnInstallEntry
    Call CreateStartMenu
    WriteUninstaller "Uninstall.exe"
	Call ResumeBackgroundService
SectionEnd

Section un.Install
	Call un.ShutdownProgram
    RMDir /r $INSTDIR
    Call un.DelUnInstallEntry
    Call un.DeleteStartMenu 
    Call un.Registry
SectionEnd