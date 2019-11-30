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

bool States::HasValidClients()
{
    TWindowSet dellist;
    for (HWND i : mClients)
    {
        if (!IsWindow(i))
            dellist.insert(i);
    }

    for (HWND i : dellist)
    {
        RemoveClient(i);
	}
	
    return mClients.size() > 0;
}
