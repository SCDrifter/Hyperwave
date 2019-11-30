#pragma once

#ifndef WIN32_LEAN_AND_MEAN
	#define WIN32_LEAN_AND_MEAN
#endif 

#include <Windows.h>

class SharedData
{
public:
    SharedData();
    SharedData(HWND window);
    ~SharedData();
    
    DWORD ServerProcessId()
    {
        if (!IsConnected())
            return 0;

        return mBlock->ServerProcessId;
    }
    HWND ServerWindow()
    {
        if (!IsConnected())
            return 0;

        return mBlock->ServerWindow;
    }

    bool IsConnected()
    {
        return m_hSharedMem != NULL;
    }

private:
#pragma pack(push, 4)
    struct SharedBlock
    {
		volatile DWORD ServerProcessId;
        volatile HWND ServerWindow;
    };
#pragma pack(pop)
private:
    bool MapMemory(bool readonly);
private:
    SharedBlock* mBlock;
    HANDLE m_hSharedMem;
    DWORD mProcessId;
    HWND mWindow;
};
