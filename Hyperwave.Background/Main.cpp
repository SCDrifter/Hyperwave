// Hyperwave.Background.cpp : Defines the entry point for the application.
//
#include "pch.h"

#include "Main.h"
#include "framework.h"

#pragma comment(lib, "wtsapi32.lib")

constexpr auto WINDOW_CLASS = L"eefcaaa8-8ed8-4f29-bb12-fc6fb40728d9";
constexpr DWORD TIMER_CLIENT_CONNECT = 0;
constexpr DWORD TIMER_INITIAL_TRIGGER = 1;
constexpr DWORD TIMER_NORMAL_TRIGGER = 2;
constexpr DWORD CLIENT_CONNECT_TIMEOUT = 5000;

Settings* gSettings;
States* gStates;
SharedData* gSharedData;

HINSTANCE gLocalModule;
HWND g_hWnd;

UINT gAppMessage;
bool mShutdownInitiated = false;

Logger* gLog = nullptr;

int MainFlow(HINSTANCE hInstance, LPWSTR lpCmdLine, int nCmdShow);
int ShutdownFlow();
bool RegisterWindowClasses(HINSTANCE hInstance);
BOOL InitInstance(HINSTANCE, int);
int MessagePump();
LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam);
LRESULT HandleAppMessage(WPARAM msg, LPARAM arg);
LRESULT HandleTimers(WPARAM timer_id, LPARAM arg);
LRESULT HandlePowerBroadcast(WPARAM pbtevent, LPARAM eventdata);
LRESULT HandleSessionChange(WPARAM wtsevent, LPARAM session_id);
bool SuspendOperation();
void ResumeOperation();
void LaunchApp();
bool IsLoggingEnabled();

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR lpCmdLine,
    _In_ int nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);

    if (lstrcmpi(lpCmdLine, L"--shutdown") == 0)
        return ShutdownFlow();
    else
        return MainFlow(hInstance, lpCmdLine, nCmdShow);
}

int ShutdownFlow()
{
    SharedData data;
    if (!data.IsConnected())
        return 0;
    HANDLE process = data.OpenProcessHandle();

    if (process == NULL)
        return 0;

    gAppMessage = RegisterWindowMessage(HSERV_REGISTERED_MESSAGE_NAME);

    PostMessage(data.ServerWindow(), gAppMessage, HSERV_SHUTDOWN, 0);

    WaitForSingleObject(process, INFINITE);
    CloseHandle(process);
    return 0;
}

int MainFlow(HINSTANCE hInstance, LPWSTR lpCmdLine, int nCmdShow)
{
    HWND first_client = NULL;

    Settings settings;
    States states;

    gSettings = &settings;
    gStates = &states;

    gSettings->Load();
    gSettings->Save();

    Logger logger(IsLoggingEnabled());
    gLog = &logger;

    gLog->Info(L"Command started with args: %s", lpCmdLine);

    if (1 == swscanf_s(lpCmdLine, L"%p", &first_client))
        gStates->AddClient(first_client);

    gAppMessage = RegisterWindowMessage(HSERV_REGISTERED_MESSAGE_NAME);

    if (!RegisterWindowClasses(hInstance))
    {
        gLog->Error(GetLastError(), L"Unable to create window class!");
    }

    if (!InitInstance(hInstance, nCmdShow))
    {
        gLog->Fatal(GetLastError(), L"Unable to create window!");
        return FALSE;
    }

    return MessagePump();
}

bool IsLoggingEnabled()
{
    wchar_t file[MAX_PATH * 2];

    swprintf_s(file, L"%s\\NLog.config", gSettings->HyperwaveDirectory());

    DWORD attr = GetFileAttributes(file);

    if (attr == INVALID_FILE_ATTRIBUTES)
        return false;
    else if (attr & FILE_ATTRIBUTE_DIRECTORY)
        return false;
    else
		return true;
}

bool RegisterWindowClasses(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex = { 0 };

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.hInstance = hInstance;
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszClassName = WINDOW_CLASS;

    return RegisterClassExW(&wcex) != 0;
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    gLocalModule = hInstance;

    g_hWnd = CreateWindowEx(0, WINDOW_CLASS, L"", WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);

    if (!g_hWnd)
    {
        return FALSE;
    }

    return TRUE;
}

int MessagePump()
{
    SharedData shared(g_hWnd);
    MSG msg;

    gSharedData = &shared;

    if (gSharedData->IsConnected())
        PostMessage(g_hWnd, WM_APP, 0, 0);
    else
    {
        gLog->Fatal(L"Unable to connect to shared data");
        DestroyWindow(g_hWnd);
    }
    gLog->Info(L"Entering windows loop");
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
    gLog->Info(L"Exiting program");

    return (int)msg.wParam;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam)
{
    switch (msg)
    {
        case WM_CREATE:
            gLog->Trace(L"WM_CREATE");
            //if (!WTSRegisterSessionNotification(hwnd, NOTIFY_FOR_THIS_SESSION))
            //{
            //    gLog->Error(GetLastError(), L"WTSRegisterSessionNotification()");
            //    return FALSE;
            //}

            if (!gSettings->Enabled())
                SetTimer(hwnd, TIMER_CLIENT_CONNECT, CLIENT_CONNECT_TIMEOUT, NULL);
            else
                SetTimer(hwnd, TIMER_INITIAL_TRIGGER, gSettings->InitialDelay() * 1000, NULL);
            break;
        case WM_DESTROY:
            gLog->Trace(L"WM_DESTROY");
            gSettings->Save();
            //WTSUnRegisterSessionNotification(hwnd);
            PostQuitMessage(0);
            break;
        case WM_POWERBROADCAST:
            gLog->Info(L"WM_POWERBROADCAST");
            return HandlePowerBroadcast(wparam, lparam);
        case WM_WTSSESSION_CHANGE:
            gLog->Info(L"WM_WTSSESSION_CHANGE");
            return HandleSessionChange(wparam, lparam);
        case WM_TIMER:
            gLog->Trace(L"WM_TIMER");
            return HandleTimers(wparam, lparam);
        case WM_APP:
            gLog->Trace(L"WM_APP");
            gStates->BroadcastMessage(g_hWnd, gAppMessage, HSERV_SERVER_BROADCAST, reinterpret_cast<LPARAM>(g_hWnd));
            //TODO: Register power and terminal state messages
            return 0;
        default:
            if (msg == gAppMessage)
                return HandleAppMessage(wparam, lparam);

            return DefWindowProc(hwnd, msg, wparam, lparam);
    }
    return 0;
}
LRESULT HandleAppMessage(WPARAM msg, LPARAM arg)
{
    bool barg = arg != 0;
    HWND wndarg = reinterpret_cast<HWND>(arg);
    unsigned int iarg = (unsigned int)arg;
    switch (msg)
    {
        case HSERV_SERVER_BROADCAST:
            return 0;

        case HSERV_SHUTDOWN:
            if (!gStates->HasValidClients(g_hWnd))
                DestroyWindow(g_hWnd);
            else
            {
                mShutdownInitiated = true;
                gStates->BroadcastMessage(g_hWnd, gAppMessage, HSERV_SHUTDOWN, 0);
            }
            return 0;

        case HSERV_CLIENT_CONNECT:
            gLog->Info(L"Client %p connected", wndarg);
            gStates->AddClient(wndarg);
            PostMessage(wndarg, gAppMessage, HSERV_SERVER_BROADCAST, reinterpret_cast<LPARAM>(g_hWnd));
            if (mShutdownInitiated)
                PostMessage(wndarg, gAppMessage, HSERV_SHUTDOWN, 0);
            return 0;

        case HSERV_CLIENT_DISCONNECT:
            gLog->Info(L"Client %p disconnected", wndarg);
            if (!gStates->RemoveClient(wndarg) && mShutdownInitiated)
                DestroyWindow(g_hWnd);
            else
                SetTimer(g_hWnd, TIMER_CLIENT_CONNECT, CLIENT_CONNECT_TIMEOUT, NULL);
            return 0;

        case HSERV_GET_ENABLED:
            return gSettings->Enabled() ? 1 : 0;

        case HSERV_GET_INITIAL_DELAY:
            return gSettings->InitialDelay();

        case HSERV_GET_INTERVAL_DELAY:
            return gSettings->IntervalDelay();

        case HSERV_GET_SUPRESS_FULLSCREEN:
            return gSettings->SupressFullscreen();

        case HSERV_SET_ENABLED:
            if (barg == gSettings->Enabled())
                return 0;

            gSettings->Enabled(arg != 0);
            if (gSettings->Enabled())
            {
                KillTimer(g_hWnd, TIMER_NORMAL_TRIGGER);
                KillTimer(g_hWnd, TIMER_CLIENT_CONNECT);
                SetTimer(g_hWnd, TIMER_INITIAL_TRIGGER, gSettings->InitialDelay() * 1000, NULL);
            }
            else
            {
                SetTimer(g_hWnd, TIMER_CLIENT_CONNECT, CLIENT_CONNECT_TIMEOUT, NULL);
                KillTimer(g_hWnd, TIMER_INITIAL_TRIGGER);
                KillTimer(g_hWnd, TIMER_NORMAL_TRIGGER);
            }
            return 0;
        case HSERV_SET_INITIAL_DELAY:
            if (iarg == gSettings->InitialDelay())
                return 0;

            gSettings->InitialDelay(iarg);
            if (gSettings->Enabled())
            {
                KillTimer(g_hWnd, TIMER_INITIAL_TRIGGER);
                KillTimer(g_hWnd, TIMER_NORMAL_TRIGGER);
                SetTimer(g_hWnd, TIMER_INITIAL_TRIGGER, gSettings->InitialDelay() * 1000, NULL);
            }
            return 0;

        case HSERV_SET_INTERVAL_DELAY:
            if (iarg == gSettings->IntervalDelay())
                return 0;

            gSettings->IntervalDelay(iarg);
            if (gSettings->Enabled())
            {
                KillTimer(g_hWnd, TIMER_INITIAL_TRIGGER);
                KillTimer(g_hWnd, TIMER_NORMAL_TRIGGER);
                SetTimer(g_hWnd, TIMER_INITIAL_TRIGGER, gSettings->InitialDelay() * 1000, NULL);
            }
            return 0;

        case HSERV_SET_SUPRESS_FULLSCREEN:
            gSettings->SupressFullscreen(barg);
            return 0;

        case HSERV_SAVE_SETTINGS:
            gSettings->Save();
            return 0;

        default:
            return 0;
    }
}

LRESULT HandleTimers(WPARAM timer_id, LPARAM arg)
{
    switch (timer_id)
    {
        case TIMER_CLIENT_CONNECT:
            gLog->Trace(L"TIMER_CLIENT_CONNECT");
            if (gSettings->Enabled())
                KillTimer(g_hWnd, timer_id);
            else if (!gStates->HasValidClients(g_hWnd))
                DestroyWindow(g_hWnd);

            return 0;

        case TIMER_INITIAL_TRIGGER:
            gLog->Trace(L"TIMER_INITIAL_TRIGGER");
            KillTimer(g_hWnd, timer_id);
            LaunchApp();
            SetTimer(g_hWnd, TIMER_NORMAL_TRIGGER, gSettings->IntervalDelay() * 1000, NULL);
            return 0;
        case TIMER_NORMAL_TRIGGER:
            gLog->Trace(L"TIMER_NORMAL_TRIGGER");
            LaunchApp();
            return 0;
        default:
            KillTimer(g_hWnd, timer_id);
            return 0;
    }
}

void LaunchApp()
{
    if (gStates->HasValidClients(g_hWnd))
        return;
    if (gSettings->SupressFullscreen() && gStates->IsFullscreenApplicationRunning())
        return;

    STARTUPINFO sinfo = { sizeof(STARTUPINFO), 0 };
    PROCESS_INFORMATION pi = { 0 };

    wchar_t cmdline[MAX_PATH * 2];

    swprintf_s(cmdline, L"%s\\Hyperwave.exe CheckMail", gSettings->HyperwaveDirectory());

    if (!CreateProcess(nullptr, cmdline, NULL, NULL, FALSE, 0, NULL, gSettings->HyperwaveDirectory(), &sinfo, &pi))
        return;

    CloseHandle(pi.hProcess);
    CloseHandle(pi.hThread);
}
LRESULT HandlePowerBroadcast(WPARAM pbtevent, LPARAM eventdata)
{
    switch (pbtevent)
    {
        case PBT_APMSUSPEND:
            gLog->Info(L"Computer sleep mode notification recieved");
            SuspendOperation();
            break;
        case PBT_APMRESUMECRITICAL:
            gLog->Info(L"Computer resume from critical sleep mode notification recieved");
            if (!SuspendOperation())
                return 0;
            ResumeOperation();
            break;
        case PBT_APMRESUMESUSPEND:
            gLog->Info(L"Computer resume from sleep mode notification recieved");
            ResumeOperation();
            break;
    }
    return 0;
}

LRESULT HandleSessionChange(WPARAM wtsevent, LPARAM session_id)
{
    enum Action
    {
        ACTION_NOTHING,
        ACTION_SUSPEND,
        ACTION_RESUME
    };

    Action action = ACTION_NOTHING;
    switch (wtsevent)
    {
        case WTS_CONSOLE_CONNECT: //The session identified by lParam was connected to the console terminal or RemoteFX session.
            gLog->Trace(L"WTS_CONSOLE_CONNECT");
            action = ACTION_RESUME;
            break;
        case WTS_REMOTE_CONNECT: //The session identified by lParam was connected to the remote terminal.
            gLog->Trace(L"WTS_REMOTE_CONNECT");
            action = ACTION_RESUME;
            break;
        case WTS_SESSION_UNLOCK: //The session identified by lParam has been unlocked.
            gLog->Trace(L"WTS_SESSION_UNLOCK");
            action = ACTION_RESUME;
            break;

        case WTS_CONSOLE_DISCONNECT: //The session identified by lParam was disconnected from the console terminal or RemoteFX session.
            gLog->Trace(L"WTS_CONSOLE_DISCONNECT");
            action = ACTION_SUSPEND;
            break;
        case WTS_REMOTE_DISCONNECT: //The session identified by lParam was disconnected from the remote terminal.
            gLog->Trace(L"WTS_REMOTE_DISCONNECT");
            action = ACTION_SUSPEND;
            break;
        case WTS_SESSION_LOCK: //The session identified by lParam has been locked.
            gLog->Trace(L"WTS_SESSION_LOCK");
            action = ACTION_SUSPEND;
            break;

        default:
            gLog->Trace(L"Unhandled session event(%p)", wtsevent);
            break;
    }

    switch (action)
    {
        case ACTION_NOTHING:
            break;
        case ACTION_SUSPEND:
            SuspendOperation();
            break;
        case ACTION_RESUME:
            ResumeOperation();
            break;
        default:
            break;
    }
    return 0;
}

bool SuspendOperation()
{
    gLog->Info(L"Suspending operations");
    if (!gSettings->Enabled())
    {
        if (!gStates->HasValidClients(g_hWnd))
        {
            PostMessage(g_hWnd, WM_CLOSE, 0, 0);
            return false;
        }
    }

    KillTimer(g_hWnd, TIMER_INITIAL_TRIGGER);
    KillTimer(g_hWnd, TIMER_NORMAL_TRIGGER);

    return true;
}

void ResumeOperation()
{
    gLog->Info(L"Resuming operations");
    if (!gSettings->Enabled())
    {
        gLog->Warn(L"Not enabled, this shouldn't be.");
        PostMessage(g_hWnd, WM_CLOSE, 0, 0);
        return;
    }
    SetTimer(g_hWnd, TIMER_INITIAL_TRIGGER, gSettings->InitialDelay() * 1000, NULL);
}