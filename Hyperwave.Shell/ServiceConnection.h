#pragma once
#include "IShellLoggerFactory.h"
#include "IWorkItem.h"

using namespace System::Threading::Tasks;

class ServiceClient;

namespace Hyperwave
{
namespace Shell
{   
	
	public ref class ServiceConnection : IServiceConnection
    {
    public:
        ServiceConnection(IShellLoggerFactory ^ factory);
        ~ServiceConnection()
        {
            this->!ServiceConnection();
        }
        !ServiceConnection();

        property bool IsConnected
        {
            bool get();
        }

		void Connect();

        Task<bool> ^ GetEnabledAsync();
        Task<unsigned int> ^ GetInitialDelayAsync();
        Task<unsigned int> ^ GetIntervalDelayAsync();
        Task<bool> ^ GetSupressFullscreenAsync();

        Task ^ SetEnabledAsync(bool value);
        Task ^ SetInitialDelayAsync(unsigned int value);
        Task ^ SetIntervalDelayAsync(unsigned int value);
        Task ^ SetSupressFullscreenAsync(bool value);

        event System::EventHandler ^ StateChanged;
        event System::EventHandler ^ ShutdownInitiated;

        static property bool IsFullscreenAppRunning
        {
            bool get();
        }

    private:
        virtual void OnStateChanged() sealed = IServiceConnection::OnStateChanged;
        virtual void OnShutdownInitiated() sealed = IServiceConnection::OnShutdownInitiated;

    private:
        ServiceClient* mClient;
        IShellLogger ^ mLog;
        IShellLoggerFactory ^ mLogFactory;
        //bool mStateChanged;
    };
}
}
