#include "pch.h"
#include "SharedData.h"

SharedData::SharedData()
{
    if (!MapMemory(true))
        return;

    mProcessId = 0;
    mWindow = nullptr;
}

SharedData::SharedData(HWND window)
{
    if (!MapMemory(false))
        return;

    HWND oldwindow;
    HWND nullwindow = nullptr;

    mProcessId = GetCurrentProcessId();
    mWindow = window;

    while ((oldwindow = (HWND)InterlockedCompareExchangePointer((volatile PVOID*)&mBlock->ServerWindow, window, nullwindow)) != nullwindow)
    {
        if (IsWindow(oldwindow)) // server already running
        {
            UnmapViewOfFile(mBlock);
            CloseHandle(m_hSharedMem);
            m_hSharedMem = nullptr;
            mBlock = nullptr;
            return;
        }
        nullwindow = oldwindow;
    }
    InterlockedExchange(&mBlock->ServerProcessId, mProcessId);
}

SharedData::~SharedData()
{
    if (m_hSharedMem == nullptr)
        return;
    if (mWindow != nullptr)
    {
        InterlockedCompareExchangePointer((PVOID*)&mBlock->ServerWindow, nullptr, mWindow);
        InterlockedCompareExchange(&mBlock->ServerProcessId, 0, mProcessId);
    }

    UnmapViewOfFile(mBlock);
    CloseHandle(m_hSharedMem);
    m_hSharedMem = nullptr;
    mBlock = nullptr;
}

bool SharedData::MapMemory(bool readonly)
{
    DWORD protect = readonly
        ? PAGE_EXECUTE_READ
        : PAGE_EXECUTE_READWRITE;
    
	DWORD sam = readonly
        ? FILE_MAP_READ
        : FILE_MAP_READ | FILE_MAP_WRITE;

    m_hSharedMem = CreateFileMapping(INVALID_HANDLE_VALUE, nullptr, protect, 0, sizeof(SharedBlock), L"Hyperwave.Background.7e8d4d8b-0e02-4f2b-a246-571b6a8cff52");

    if (m_hSharedMem == nullptr || m_hSharedMem == INVALID_HANDLE_VALUE)
    {
        mBlock = nullptr;
        m_hSharedMem = nullptr;
        return false;
    }

    mBlock = (SharedBlock*)MapViewOfFile(m_hSharedMem, sam, 0, 0, 0);
    if (mBlock == nullptr)
    {
        CloseHandle(m_hSharedMem);
        m_hSharedMem = nullptr;
        return false;
    }
    return true;
}
