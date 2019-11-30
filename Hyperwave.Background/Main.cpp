// Hyperwave.Background.cpp : Defines the entry point for the application.
//
#include "pch.h"

#include "Main.h"
#include "framework.h"

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

ATOM RegisterWindowClasses(HINSTANCE hInstance);
BOOL InitInstance(HINSTANCE, int);
int MessagePump();
LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam);
LRESULT HandleAppMessage(WPARAM msg, LPARAM arg);
LRESULT HandleTimers(WPARAM timer_id, LPARAM arg);
void LaunchApp();

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR lpCmdLine,
    _In_ int nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);
    Settings settings;
    States states;
    gSettings = &settings;
    gStates = &states;

    gSettings->Load();
    gSettings->Save();

    gAppMessage = RegisterWindowMessage(HSERV_REGISTERED_MESSAGE_NAME);

    RegisterWindowClasses(hInstance);

    if (!InitInstance(hInstance, nCmdShow))
    {
        return FALSE;
    }

    return MessagePump();
}

ATOM RegisterWindowClasses(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex = { 0 };

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.hInstance = hInstance;
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszClassName = WINDOW_CLASS;

    return RegisterClassExW(&wcex);
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

    if (!gSharedData->IsConnected())
        DestroyWindow(g_hWnd);
    else
        PostMessage(g_hWnd, WM_APP, 0, 0);

    while (GetMessage(&msg, nullptr, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return (int)msg.wParam;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam)
{
    switch (msg)
    {
        case WM_CREATE:
            if (!gSettings->Enabled())
                SetTimer(hwnd, TIMER_CLIENT_CONNECT, CLIENT_CONNECT_TIMEOUT, NULL);
            else
                SetTimer(hwnd, TIMER_INITIAL_TRIGGER, gSettings->InitialDelay() * 1000, NULL);
            break;
        case WM_DESTROY:
            gSettings->Save();
            PostQuitMessage(0);
            break;
        case WM_TIMER:
            return HandleTimers(wparam, lparam);
        case WM_APP:
            SendNotifyMessage(HWND_BROADCAST, gAppMessage, 0, reinterpret_cast<LPARAM>(g_hWnd));
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

        case HSERV_CLIENT_CONNECT:
            KillTimer(g_hWnd, TIMER_CLIENT_CONNECT);
            gStates->AddClient(wndarg);
            return 0;

        case HSERV_CLIENT_DISCONNECT:
            if (!gStates->RemoveClient(wndarg) && !gSettings->Enabled())
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
                if (!gStates->HasValidClients())
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

            if (!gSettings->Enabled() && !gStates->HasValidClients())
                DestroyWindow(g_hWnd);
            else
                KillTimer(g_hWnd, timer_id);

            return 0;

        case TIMER_INITIAL_TRIGGER:
            KillTimer(g_hWnd, timer_id);
            LaunchApp();
            SetTimer(g_hWnd, TIMER_NORMAL_TRIGGER, gSettings->IntervalDelay() * 1000, NULL);
            return 0;
        case TIMER_NORMAL_TRIGGER:
            LaunchApp();
            return 0;
        default:
            KillTimer(g_hWnd, timer_id);
            return 0;
    }
}

void LaunchApp()
{
    if (gStates->HasValidClients())
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
