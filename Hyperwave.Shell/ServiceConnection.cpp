#include "ServiceConnection.h"
#include "..\Hyperwave.Cpp.Common\Messages.h"
#include "ServiceClient.h"

generic<typename T>
    ref class WorkItem abstract : IWorkItem
{
public:
    WorkItem()
    {
        mCompletion = gcnew TaskCompletionSource<T>();
    }

    Task<T> ^ Wait() {
        return mCompletion->Task;
    }

        // Inherited via IWorkItem
        virtual void Complete(void* result)
    {
        mCompletion->SetResult(ConvertValue(result));
    }
    virtual void Failed()
    {
        mCompletion->SetCanceled();
    }
    virtual int Msg()
    {
        return 0;
    }
    virtual void* Param()
    {
        return nullptr;
    }

protected:
    virtual T ConvertValue(void* value) abstract = 0;

private:
    TaskCompletionSource<T> ^ mCompletion;
};

ref class BoolWorkItem : WorkItem<bool>
{
protected:
    // Inherited via WorkItem
    virtual bool ConvertValue(void* value) override
    {
        return value != nullptr;
    }
};

ref class IntWorkItem : WorkItem<unsigned int>
{
protected:
    // Inherited via WorkItem
    virtual unsigned int ConvertValue(void* value) override
    {
        return (unsigned int)value;
    }
};

Hyperwave::Shell::ServiceConnection::ServiceConnection(IShellLoggerFactory ^ factory)
{
    mLog = factory->Create(GetType()->FullName);
    mClient = nullptr;
    mLogFactory = factory;
}

Hyperwave::Shell::ServiceConnection::!ServiceConnection()
{
    if (mClient != nullptr)
		delete mClient;
}

bool Hyperwave::Shell::ServiceConnection::IsConnected::get()
{
    return mClient->GetState() == ServiceState::ONLINE;
}

ref class GetEnabledWorkItem : BoolWorkItem
{
public:
    virtual int Msg() override
    {
        return HSERV_GET_ENABLED;
    }
};

void Hyperwave::Shell::ServiceConnection::Connect()
{
    if (mClient != nullptr)
        return;

    mClient = new ServiceClient(this, mLogFactory);
    mClient->Connect();
}

Task<bool> ^ Hyperwave::Shell::ServiceConnection::GetEnabledAsync()
{
    mLog->Trace("{0}()", __FUNCTION__);
    GetEnabledWorkItem ^ item = gcnew GetEnabledWorkItem();
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class GetInitialDelayWorkItem : IntWorkItem
{
public:
    virtual int Msg() override
    {
        return HSERV_GET_INITIAL_DELAY;
    }
};

Task<unsigned int> ^ Hyperwave::Shell::ServiceConnection::GetInitialDelayAsync()
{
    Connect();

    mLog->Trace("{0}()", __FUNCTION__);
    GetInitialDelayWorkItem ^ item = gcnew GetInitialDelayWorkItem();
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class GetIntervalDelayWorkItem : IntWorkItem
{
public:
    virtual int Msg() override
    {
        return HSERV_GET_INTERVAL_DELAY;
    }
};

Task<unsigned int> ^ Hyperwave::Shell::ServiceConnection::GetIntervalDelayAsync()
{
    Connect();

    mLog->Trace("{0}()", __FUNCTION__);
    GetIntervalDelayWorkItem ^ item = gcnew GetIntervalDelayWorkItem();
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class GetSupressFullscreenWorkItem : BoolWorkItem
{
public:
    virtual int Msg() override
    {
        return HSERV_GET_SUPRESS_FULLSCREEN;
    }
};

Task<bool> ^ Hyperwave::Shell::ServiceConnection::GetSupressFullscreenAsync()
{
    Connect();

    mLog->Trace("{0}()", __FUNCTION__);
    GetSupressFullscreenWorkItem ^ item = gcnew GetSupressFullscreenWorkItem();
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class SetEnabledWorkItem : BoolWorkItem
{
public:
    SetEnabledWorkItem(bool value)
    {
        mValue = value;
    }

    virtual int Msg() override
    {
        return HSERV_SET_ENABLED;
    }

    virtual void* Param() override
    {
        return mValue ? (void*)1 : 0;
    }

private:
    bool mValue;
};

Task ^ Hyperwave::Shell::ServiceConnection::SetEnabledAsync(bool value)
{
    Connect();

    mLog->Trace("{0}({1})", __FUNCTION__, value);
    SetEnabledWorkItem ^ item = gcnew SetEnabledWorkItem(value);
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class SetInitialDelayWorkItem : BoolWorkItem
{
public:
    SetInitialDelayWorkItem(unsigned int value)
    {
        mValue = value;
    }

    virtual int Msg() override
    {
        return HSERV_SET_INITIAL_DELAY;
    }

    virtual void* Param() override
    {
        return (void*)mValue;
    }

private:
    unsigned int mValue;
};

Task ^ Hyperwave::Shell::ServiceConnection::SetInitialDelayAsync(unsigned int value)
{
    Connect();

    mLog->Trace("{0}({1})", __FUNCTION__, value);
    SetInitialDelayWorkItem ^ item = gcnew SetInitialDelayWorkItem(value);
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class SetIntervalDelayWorkItem : BoolWorkItem
{
public:
    SetIntervalDelayWorkItem(unsigned int value)
    {
        mValue = value;
    }

    virtual int Msg() override
    {
        return HSERV_SET_INTERVAL_DELAY;
    }

    virtual void* Param() override
    {
        return (void*)mValue;
    }

private:
    unsigned int mValue;
};

Task ^ Hyperwave::Shell::ServiceConnection::SetIntervalDelayAsync(unsigned int value)
{
    Connect();

    mLog->Trace("{0}({1})", __FUNCTION__, value);
    SetIntervalDelayWorkItem ^ item = gcnew SetIntervalDelayWorkItem(value);
    mClient->PostWorkItem(item);
    return item->Wait();
}

ref class SetSupressFullscreenWorkItem : BoolWorkItem
{
public:
    SetSupressFullscreenWorkItem(bool value)
    {
        mValue = value;
    }

    virtual int Msg() override
    {
        return HSERV_SET_SUPRESS_FULLSCREEN;
    }

    virtual void* Param() override
    {
        return mValue ? (void*)1 : 0;
    }

private:
    bool mValue;
};

Task ^ Hyperwave::Shell::ServiceConnection::SetSupressFullscreenAsync(bool value)
{
    Connect();

    mLog->Trace("{0}({1})", __FUNCTION__, value);
    SetSupressFullscreenWorkItem ^ item = gcnew SetSupressFullscreenWorkItem(value);
    mClient->PostWorkItem(item);
    return item->Wait();
}

void Hyperwave::Shell::ServiceConnection::OnStateChanged()
{
    StateChanged(this, System::EventArgs::Empty);
}

void Hyperwave::Shell::ServiceConnection::OnShutdownInitiated()
{
    ShutdownInitiated(this, System::EventArgs::Empty);    
}

bool Hyperwave::Shell::ServiceConnection::IsFullscreenAppRunning::get()
{
    QUERY_USER_NOTIFICATION_STATE state = QUNS_ACCEPTS_NOTIFICATIONS;

    SHQueryUserNotificationState(&state);

    return state != QUNS_ACCEPTS_NOTIFICATIONS && state != QUNS_APP;
}