#pragma once
#include <Windows.h>
class Mutex
{
public:
    Mutex();
    Mutex(const wchar_t* name);
    ~Mutex();

    bool Lock();
    void Unlock();

private:
    HANDLE m_hMutex;
};

class Lock
{
public:
    Lock(Mutex& mutex);
    Lock(Mutex* mutex);
    ~Lock();

    bool IsAbandoned() const
    {
        return mIsAbandoned;
    }

private:
    Mutex* mLock;
    bool mIsAbandoned;
};

class MutexException
{
};

class MutexIntializeException : public MutexException
{
};

class MutexLockException : public MutexException
{
};