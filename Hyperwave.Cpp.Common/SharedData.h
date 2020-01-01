#pragma once

#include "Mutex.h"

class SharedData
{
public:
    SharedData();
    SharedData(HWND window);
    ~SharedData();

    DWORD ServerProcessId() const
    {
        if (!IsConnected())
            return 0;

        Lock lock(mLock);
        return mBlock->ServerProcessId;
    }
    HWND ServerWindow() const
    {
        if (!IsConnected())
            return 0;

        Lock lock(mLock);
        return mBlock->ServerWindow;
    }

    bool IsConnected() const
    {
        return m_hSharedMem != NULL;
    }

    HANDLE OpenProcessHandle() const
    {
        if (!IsConnected())
            return NULL;

        Lock lock(mLock);
        return OpenProcessHandleNoLock();
	}

private:
#pragma pack(push, 4)
    struct SharedBlock
    {
        DWORD ServerProcessId;
        HWND ServerWindow;
        FILETIME ServeCreateTime;
    };
#pragma pack(pop)
private:
    bool MapMemory(bool readonly);
    static void GetCreationTime(HANDLE hprocess, FILETIME* creationtime);
    HANDLE OpenProcessHandleNoLock() const;

private:
    SharedBlock* mBlock;
    HANDLE m_hSharedMem;
    mutable Mutex mLock;
    DWORD mProcessId;
    HWND mWindow;
};
