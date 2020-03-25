#pragma once
interface class IWorkItem
{
public:
    virtual void Complete(void* result) abstract = 0;
    virtual void Failed() abstract = 0;
    virtual int Msg() abstract = 0;
    virtual void* Param() abstract = 0;
};
interface class IServiceConnection
{
public:
    virtual void OnStateChanged() = 0;
    virtual void OnShutdownInitiated() = 0;
};
