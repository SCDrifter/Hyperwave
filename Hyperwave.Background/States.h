#pragma once
#include <set>
#include <Windows.h>
class States
{
public:
    bool IsFullscreenApplicationRunning();
    void AddClient(HWND hwnd);
    bool RemoveClient(HWND hwnd);
    bool HasValidClients();

private:
    typedef std::set<HWND> TWindowSet;

private:
    TWindowSet mClients;
};