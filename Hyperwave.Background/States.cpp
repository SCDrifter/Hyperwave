#include "pch.h"

#include "States.h"
#include <shellapi.h>

bool States::IsFullscreenApplicationRunning()
{
    QUERY_USER_NOTIFICATION_STATE state = QUNS_ACCEPTS_NOTIFICATIONS;

    SHQueryUserNotificationState(&state);

    return state != QUNS_ACCEPTS_NOTIFICATIONS && state != QUNS_APP;
}

void States::AddClient(HWND hwnd)
{
    mClients.insert(hwnd);
}

bool States::RemoveClient(HWND hwnd)
{
    mClients.erase(hwnd);
    return mClients.size() > 0;
}

bool States::HasValidClients(HWND postback)
{
    size_t size = mClients.size();
    for (HWND i : mClients)
    {
        if (!IsWindow(i))
        {
            PostMessage(postback, gAppMessage, HSERV_CLIENT_DISCONNECT, (LPARAM)i);
            size--;
        }
    }

    return size > 0;
}

void States::BroadcastMessage(HWND postback, UINT msg, WPARAM wparam, LPARAM lparam)
{
    for (HWND i : mClients)
    {
        if (!IsWindow(i))
            PostMessage(postback, gAppMessage, HSERV_CLIENT_DISCONNECT, (LPARAM)i);
        else
            PostMessage(i, msg, wparam, lparam);
    }
}
