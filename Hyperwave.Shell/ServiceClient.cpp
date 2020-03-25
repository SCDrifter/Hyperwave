
#include "ServiceClient.h"
#include "..\Hyperwave.Cpp.Common\Messages.h"
#include "..\Hyperwave.Cpp.Common\HyperwaveUtil.h"
#include <stdio.h>

extern HMODULE gLocalModule;

using namespace System::Runtime::InteropServices;

constexpr auto TIMER_CONNECT = 0;
constexpr auto WM_STATE_CHANGED = WM_APP + 0;
constexpr auto WM_APP_STOPPED = WM_APP + 1;
constexpr auto WM_WORK_ITEM = WM_APP + 2;

constexpr auto WINDOW_CLASS = L"c4c3d60d-7ee7-4c86-a1e0-0fcd2584f4df";

volatile LONG ServiceClient::mClassRegCount = 0;

UINT ServiceClient::mAppMessage = WM_NULL;


ServiceClient::ServiceClient(IServiceConnection ^ listener, IShellLoggerFactory ^ factory)
{
    mShared = nullptr;
    mState = (LONG)ServiceState::STOPPED;
    mListener = listener;
    mLog = factory->Create("Hyperwave.Shell.dll!ServiceClient");

    mHyperwaveDirectory = HyperwaveUtil::GetApplicationDirectory();

    m_hWnd = nullptr;
    m_hProcess = nullptr;
    m_hProcessWait = nullptr;
    m_hServerWnd = nullptr;

    mBackQueue = gcnew TWorkItemQueue();

    m_hStartupSignal = nullptr;
    m_hWndThread = nullptr;
}

ServiceClient::~ServiceClient()
{
    if (m_hWndThread != nullptr)
    {
        mLog->Info("Shutting down thread");
        if (IsWindow(m_hWnd))
            PostMessage(m_hWnd, WM_CLOSE, 0, 0);

        WaitForSingleObject(m_hWndThread, INFINITE);
        CloseHandle(m_hWndThread);
        m_hWndThread = nullptr;
        mLog->Info("Thread shutdown");
    }

    if (mHyperwaveDirectory != nullptr)
        delete[] mHyperwaveDirectory;
}

void ServiceClient::Connect()
{
    m_hStartupSignal = CreateEvent(nullptr, TRUE, FALSE, nullptr);

    if (m_hStartupSignal == nullptr)
    {
        mLog->Fatal("CreateEvent()={0}", GetWin32ErrorText());
        throw gcnew FatalShellException();
    }

    m_hWndThread = CreateThread(nullptr, 0, &ServiceClient::ThreadEntry, this, 0, &mWndThreadId);

    if (m_hWndThread == nullptr)
    {
        mLog->Fatal("CreateThread()={0}", GetWin32ErrorText());
        CloseHandle(m_hStartupSignal);
        throw gcnew FatalShellException();
    }
    HANDLE wait[] = { m_hWndThread, m_hStartupSignal };

    switch (WaitForMultipleObjects(2, wait, FALSE, INFINITE))
    {
        case WAIT_OBJECT_0:
            mLog->Error("Thread stopped unexpectedly");
            CloseHandle(m_hStartupSignal);
            CloseHandle(m_hWndThread);
            m_hWndThread = nullptr;
            SetState(ServiceState::ERROR_STOPPED);
            break;
        case WAIT_OBJECT_0 + 1:
            mLog->Info("Thread now ready, Resuming main flow.");
            CloseHandle(m_hStartupSignal);
            break;
        default:
            throw gcnew FatalShellException();
    }
}

ServiceState ServiceClient::GetState()
{
    return (ServiceState)InterlockedCompareExchange(&mState, mState, mState);
}

ServiceState ServiceClient::SetState(ServiceState newstate, bool notify_window)
{
    ServiceState oldstate = (ServiceState)InterlockedExchange(&mState, (LONG)newstate);
    if (oldstate == newstate)
        return oldstate;

    mLog->Info("State: {0}->{1}", oldstate, newstate);

    if (((IServiceConnection ^) mListener) != nullptr)
        mListener->OnStateChanged();

    if (notify_window && m_hWnd != nullptr)
        PostMessage(m_hWnd, WM_STATE_CHANGED, (WPARAM)oldstate, (LPARAM)newstate);

    return oldstate;
}

bool ServiceClient::SetStateIf(ServiceState state, ServiceState newstate)
{
    ServiceState oldstate = (ServiceState)InterlockedCompareExchange(&mState, (LONG)newstate, (LONG)state);
    if (oldstate == newstate || oldstate != state)
        return false;

    mLog->Info("State: {0}->{1}", oldstate, newstate);

    if (((IServiceConnection ^) mListener) != nullptr)
        mListener->OnStateChanged();

    if (m_hWnd != nullptr)
        PostMessage(m_hWnd, WM_STATE_CHANGED, (WPARAM)oldstate, (LPARAM)newstate);

    return true;
}

void ServiceClient::PostWorkItem(IWorkItem ^ item)
{

    if (GetState() == ServiceState::ERROR_STOPPED)
    {
        item->Failed();
        return;
    }

    GCHandle handle = GCHandle::Alloc(item, GCHandleType::Normal);
    PostMessage(m_hWnd, WM_WORK_ITEM, 0, (LPARAM)GCHandle::ToIntPtr(handle));
}

void ServiceClient::AddWorkItem(LPARAM lparam)
{
    GCHandle handle = GCHandle::FromIntPtr((System::IntPtr)lparam);
    IWorkItem ^ item = safe_cast<IWorkItem ^>(handle.Target);
    handle.Free();

    mBackQueue->Enqueue(item);

    switch (GetState())
    {
        case ServiceState::ONLINE:
            ProcessWorkItems();
            break;
        case ServiceState::ERROR_STOPPED:
            FailWorkItems();
        default:
            break;
    }
}

DWORD ServiceClient::ThreadEntry(PVOID param)
{
    ServiceClient* client = reinterpret_cast<ServiceClient*>(param);
    return client->ThreadMain();
}

DWORD ServiceClient::ThreadMain()
{
    SharedData sdata;

    if (!sdata.IsConnected())
    {
        mLog->Error("Unable to connect to shared data");
        return -3;
    }
    mShared = &sdata;

    if (!RegisterWindowClass())
    {
        mLog->Error("Unable to register window class");
		return -1;
    }
    if (!CreateWindowInstance())
    {
        mLog->Error("Failed to create window");
        return -2;
    }
    SetEvent(m_hStartupSignal);
    MSG msg;

    while (GetMessage(&msg, nullptr, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
    mLog->Error("Thread closed");
    UnRegisterWindowClass();

    m_hWnd = nullptr;
    mShared = nullptr;

    FailWorkItems();

    return 0;
}

bool ServiceClient::RegisterWindowClass()
{
    if (InterlockedIncrement(&mClassRegCount) > 1)
        return true;

    if (mAppMessage == WM_NULL)
        mAppMessage = RegisterWindowMessage(HSERV_REGISTERED_MESSAGE_NAME);

    WNDCLASSEX wcex = { 0 };

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.hInstance = gLocalModule;
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszClassName = WINDOW_CLASS;

    bool ret = (0 != RegisterClassEx(&wcex));
    if (!ret)
        mLog->Error("RegisterClassEx() = {0}",GetWin32ErrorText());
    return ret;
}

bool ServiceClient::CreateWindowInstance()
{
    m_hWnd = CreateWindowEx(0, WINDOW_CLASS, L"", 0, 0, 0, 100, 100, HWND_MESSAGE, NULL, gLocalModule, this);
    bool ret = (m_hWnd != nullptr);
    
	if (!ret)
        mLog->Error("CreateWindowEx() = {0}", GetWin32ErrorText());

    return ret;
}

void ServiceClient::UnRegisterWindowClass()
{
    if (InterlockedDecrement(&mClassRegCount) > 0)
        return;
    UnregisterClass(WINDOW_CLASS, gLocalModule);
}

void ServiceClient::ConnectToApp(bool restart)
{
    m_hProcess = mShared->OpenProcessHandle();
    if (m_hProcess == nullptr)
    {
        mLog->Info("Background process not running, starting");
        StartApp(restart);
        return;
    }
    RegisterProcessMonitor();
    mLog->Info("Background process running, sending connect message");
    PostMessage(mShared->ServerWindow(), mAppMessage, HSERV_CLIENT_CONNECT, (LPARAM)m_hWnd);
    SetState(restart ? ServiceState::RECONNECTING : ServiceState::CONNECTING);
}

void ServiceClient::RegisterProcessMonitor()
{
    UnRegisterProcessMonitor();
    if (!RegisterWaitForSingleObject(&m_hProcessWait, m_hProcess, &ServiceClient::OnProcessStopped, this, INFINITE, WT_EXECUTEONLYONCE))
    {
        mLog->Warning("RegisterWaitForSingleObject()={0}", GetWin32ErrorText());
    }
}

void ServiceClient::UnRegisterProcessMonitor()
{
    if (m_hProcessWait == nullptr)
        return;

    UnregisterWaitEx(m_hProcessWait, INVALID_HANDLE_VALUE);
    m_hProcessWait = nullptr;
}

void ServiceClient::OnProcessStopped(LPVOID userdata, BOOLEAN timeout)
{
    ServiceClient* client = reinterpret_cast<ServiceClient*>(userdata);
    PostMessage(client->m_hWnd, WM_APP_STOPPED, 0, 0);
}

void ServiceClient::StartApp(bool restart)
{
    STARTUPINFO sinfo = { 0 };
    sinfo.cb = sizeof(sinfo);

    PROCESS_INFORMATION pi = { 0 };

    wchar_t cmdline[MAX_PATH * 2];

    swprintf_s(cmdline, L"%s\\Hyperwave.Background.exe %p", mHyperwaveDirectory, m_hWnd);

	mLog->Info("Starting background process:\r\n\t{0}", gcnew System::String(cmdline));

    if (!CreateProcess(nullptr, cmdline, nullptr, nullptr, FALSE, 0, nullptr, mHyperwaveDirectory, &sinfo, &pi))
    {
        mLog->Error("CreateProcess()={0}", GetWin32ErrorText());
        SetState(ServiceState::ERROR_STOPPED);
        return;
    }

    m_hProcess = pi.hProcess;
    CloseHandle(pi.hThread);

    RegisterProcessMonitor();
    SetState(restart ? ServiceState::RECONNECTING : ServiceState::CONNECTING);
}

void ServiceClient::CloseApp()
{
    UnRegisterProcessMonitor();
    if (m_hProcess != nullptr)
    {
        CloseHandle(m_hProcess);
        m_hProcess = nullptr;
    }
}

void ServiceClient::ProcessWorkItems()
{
    while (mBackQueue->Count > 0 && IsWindow(m_hServerWnd))
    {
        IWorkItem ^ item = mBackQueue->Peek();

        DWORD_PTR msg_result;

        if (0 == SendMessageTimeout(m_hServerWnd, mAppMessage, (WPARAM)item->Msg(), (LPARAM)item->Param(), SMTO_ABORTIFHUNG | SMTO_ERRORONEXIT, 1000, &msg_result))
            break;

        mBackQueue->Dequeue();

        item->Complete((void*)msg_result);
    }
}

void ServiceClient::FailWorkItems()
{
    while (mBackQueue->Count > 0 && IsWindow(m_hServerWnd))
    {
        IWorkItem ^ item = mBackQueue->Dequeue();
        item->Failed();
    }
}

LRESULT ServiceClient::WndProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam)
{
    LRESULT result = 0;
    ServiceClient* client = nullptr;
    if (msg != WM_NCCREATE)
    {
        client = reinterpret_cast<ServiceClient*>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
    }
    else
    {
        CREATESTRUCT* cstruct = reinterpret_cast<CREATESTRUCT*>(lparam);
        client = reinterpret_cast<ServiceClient*>(cstruct->lpCreateParams);

        if (client != nullptr)
        {
            SetWindowLongPtr(hwnd, GWLP_USERDATA, (LONG_PTR)client);
            client->m_hWnd = hwnd;
        }
    }

    if (client != nullptr && client->OnWindowMessage(msg, wparam, lparam, &result))
        return result;
    else
        return DefWindowProc(hwnd, msg, wparam, lparam);
}

bool ServiceClient::OnWindowMessage(UINT msg, WPARAM wparam, LPARAM lparam, LRESULT* result)
{
    switch (msg)
    {
        case WM_CREATE:
            ConnectToApp(false);
            *result = 0;
            return true;

        case WM_DESTROY:
            mLog->Info("Window closing");
            CloseApp();
            if (SetState(ServiceState::STOPPED, false) == ServiceState::ONLINE)
                PostMessage(m_hServerWnd, mAppMessage, HSERV_CLIENT_DISCONNECT, (LPARAM)m_hWnd);

            *result = 0;
            PostQuitMessage(0);
            return true;

        case WM_TIMER:
            return HandleTimer(wparam, result);
        default:
            if (msg == mAppMessage)
                return HandleAppMessage(wparam, lparam, result);
            else
                return HandleInternalMessage(msg, wparam, lparam, result);
    }
}

bool ServiceClient::HandleInternalMessage(UINT msg, WPARAM wparam, LPARAM lparam, LRESULT* result)
{
    *result = 0;
    switch (msg)
    {
        case WM_APP_STOPPED:
            mLog->Info("msg: WM_APP_STOPPED");
            CloseApp();
            if (SetStateIf(ServiceState::RECONNECTING, ServiceState::ERROR_STOPPED))
                return true;
            SetState(ServiceState::STOPPED);
            StartApp(true);
            return true;

        case WM_STATE_CHANGED:
            mLog->Info("msg: WM_STATE_CHANGED");
            switch ((ServiceState)lparam)
            {
                case ServiceState::ERROR_STOPPED:
                    //DestroyWindow(m_hWnd);
                    break;

                case ServiceState::ONLINE:
                    KillTimer(m_hWnd, TIMER_CONNECT);
                    ProcessWorkItems();
                    break;

                case ServiceState::CONNECTING:
                case ServiceState::RECONNECTING:
                    SetTimer(m_hWnd, TIMER_CONNECT, 5000, nullptr);
                    break;
            }
            return true;

        case WM_WORK_ITEM:
            mLog->Info("msg: WM_WORK_ITEM");
            AddWorkItem(lparam);
            return true;
    }
    return false;
}

bool ServiceClient::HandleAppMessage(WPARAM msg, LPARAM arg, LRESULT* result)
{
    *result = 0;
    switch (msg)
    {
        case HSERV_SERVER_BROADCAST:
            mLog->Info("msg: HSERV_SERVER_BROADCAST");
            m_hServerWnd = (HWND)arg;
            SetState(ServiceState::ONLINE);
            return true;
        case HSERV_SHUTDOWN:
            mLog->Info("msg: HSERV_SHUTDOWN");
            mListener->OnShutdownInitiated();
            return true;
    }
    return false;
}

bool ServiceClient::HandleTimer(WPARAM timer_id, LRESULT* result)
{
    *result = 0;
    switch (timer_id)
    {
        case TIMER_CONNECT:
            mLog->Info("Timer: TIMER_CONNECT");
            TerminateProcess(m_hProcess, -1);
            return true;
        default:
            break;
    }
    return false;
}

System::String ^ ServiceClient::GetWin32ErrorText()
{
    wchar_t* text = nullptr;
    FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, 0, GetLastError(), 0, (LPWSTR)&text, 1024, nullptr);
    if (text == nullptr)
        return gcnew System::String("Unknown win32 error");
    System::String ^ ret = gcnew System::String(text);
    LocalFree(text);

    return ret->Substring(0, ret->Length - 2);
}
