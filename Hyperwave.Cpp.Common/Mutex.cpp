#include "pch.h"

#include "Mutex.h"

Mutex::Mutex()
{
    m_hMutex = CreateMutex(NULL, FALSE, NULL);
    if (m_hMutex == NULL)
        throw MutexIntializeException();
}

Mutex::Mutex(const wchar_t* name)
{
    m_hMutex = CreateMutex(NULL, FALSE, name);
    if (m_hMutex == NULL)
        throw MutexIntializeException();
}

Mutex::~Mutex()
{
    CloseHandle(m_hMutex);
}

bool Mutex::Lock()
{
    switch (WaitForSingleObject(m_hMutex, INFINITE))
    {
        case WAIT_OBJECT_0:
            return true;
        case WAIT_ABANDONED:
            return false;
        default:
            throw MutexLockException();
    }
}

void Mutex::Unlock()
{
    if(!ReleaseMutex(m_hMutex))
        throw MutexLockException();
}

Lock::Lock(Mutex& mutex)
{
    mLock = &mutex;
    mIsAbandoned = !mLock->Lock();
}

Lock::Lock(Mutex* mutex)
{
    mLock = mutex;
    mIsAbandoned = !mLock->Lock();
}

Lock::~Lock()
{
    mLock->Unlock();
}
