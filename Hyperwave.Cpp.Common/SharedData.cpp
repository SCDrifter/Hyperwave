#include "pch.h"

#include "SharedData.h"

constexpr auto MUTEX_NAME = L"Hyperwave.Background.";

SharedData::SharedData()
    : mLock(MUTEX_NAME)
{
    if (!MapMemory(false))
        return;

    mProcessId = 0;
    mWindow = nullptr;
}

SharedData::SharedData(HWND window)
    : mLock(MUTEX_NAME)
{
    if (!MapMemory(false))
        return;

    mProcessId = GetCurrentProcessId();
    mWindow = window;

    {
        Lock lock(mLock);

        HANDLE existing_process = OpenProcessHandleNoLock();

        if (existing_process != NULL) // server already running
        {
            CloseHandle(existing_process);
            UnmapViewOfFile(mBlock);
            CloseHandle(m_hSharedMem);
            m_hSharedMem = nullptr;
            mBlock = nullptr;
            return;
        }

        mBlock->ServerProcessId = mProcessId;
        mBlock->ServerWindow = mWindow;
        GetCreationTime(GetCurrentProcess(), &mBlock->ServeCreateTime);
    }
}

SharedData::~SharedData()
{
    if (m_hSharedMem == nullptr)
        return;
    if (mWindow != nullptr)
    {
        Lock lock(mLock);
        mBlock->ServerWindow = nullptr;
        mBlock->ServerProcessId = 0;
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

void SharedData::GetCreationTime(HANDLE hprocess, FILETIME* creationtime)
{
    FILETIME trash1, trash2, trash3;
    GetProcessTimes(hprocess, creationtime, &trash1, &trash2, &trash3);
}

HANDLE SharedData::OpenProcessHandleNoLock() const
{
    HANDLE ret = OpenProcess(SYNCHRONIZE | PROCESS_QUERY_INFORMATION | PROCESS_TERMINATE, FALSE, mBlock->ServerProcessId);

    if (ret == nullptr)
        return NULL;

    FILETIME create = { 0 };

    GetCreationTime(ret, &create);

    if (CompareFileTime(&create, &mBlock->ServeCreateTime) != 0)
    {
        CloseHandle(ret);
        ret = NULL;
    }

    if (ret != nullptr && WaitForSingleObject(ret, 0) == WAIT_OBJECT_0)
    {
        CloseHandle(ret);
        ret = NULL;
    }

    return ret;
}
