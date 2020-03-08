#pragma once
#include <Windows.h>
#include <gcroot.h>

#include "..\Hyperwave.Cpp.Common\SharedData.h"
#include "ServiceConnection.h"


using namespace Hyperwave::Shell;

namespace Hyperwave
{
namespace Shell
{
public
    ref class FatalShellException : System::Exception
    {
    };
public
    enum class ServiceState
    {
        STOPPED,
        CONNECTING,
        RECONNECTING,
        ONLINE,
        ERROR_STOPPED
    };
}
}

class ServiceClient
{
public:
    ServiceClient(System::Action ^ listener, IShellLoggerFactory ^factory);
    ~ServiceClient();

    ServiceState GetState();

    void PostWorkItem(IWorkItem ^ item);

private:
    ServiceState SetState(ServiceState newstate, bool notify_window = true);
    bool SetStateIf(ServiceState state, ServiceState newstate);

    static DWORD CALLBACK ThreadEntry(PVOID param);
    DWORD ThreadMain();

    bool RegisterWindowClass();
    bool CreateWindowInstance();
    void UnRegisterWindowClass();

    void ConnectToApp(bool restart);
    void RegisterProcessMonitor();
    void UnRegisterProcessMonitor();
    static void CALLBACK OnProcessStopped(LPVOID userdata, BOOLEAN timeout);

    void StartApp(bool restart);
    void CloseApp();

    void AddWorkItem(LPARAM lparam);
    void ProcessWorkItems();
    void FailWorkItems();


    static LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam);
    bool OnWindowMessage(UINT msg, WPARAM wparam, LPARAM lparam, LRESULT* result);
    bool HandleInternalMessage(UINT msg, WPARAM wparam, LPARAM lparam, LRESULT* result);
    bool HandleAppMessage(WPARAM msg, LPARAM arg, LRESULT* result);
    bool HandleTimer(WPARAM timer_id, LRESULT* result);

	System::String ^ GetWin32ErrorText();

private:
    typedef System::Collections::Generic::Queue<IWorkItem ^> TWorkItemQueue;

private:
    volatile LONG mState;
    volatile static LONG mClassRegCount;
    static UINT mAppMessage;
    msclr::gcroot<TWorkItemQueue ^> mBackQueue;
    msclr::gcroot<System::Action ^> mListener;

    HWND m_hWnd;
    HANDLE m_hWndThread;
    DWORD mWndThreadId;

    HANDLE m_hStartupSignal;

    HANDLE m_hProcess;
    HANDLE m_hProcessWait;
    HWND m_hServerWnd;

    SharedData* mShared;
    wchar_t* mHyperwaveDirectory;
    msclr::gcroot<IShellLogger ^> mLog;
};