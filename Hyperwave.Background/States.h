#pragma once
#include <set>
#include <Windows.h>
class States
{
public:
    bool IsFullscreenApplicationRunning();
    void AddClient(HWND hwnd);
    bool RemoveClient(HWND hwnd);
    bool HasValidClients(HWND postback);
    void BroadcastMessage(HWND postback, UINT msg, WPARAM wparam, LPARAM lparam);

private:
    typedef std::set<HWND> TWindowSet;

private:
    TWindowSet mClients;
};