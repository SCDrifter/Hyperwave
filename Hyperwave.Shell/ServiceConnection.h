#pragma once
#include "IWorkItem.h"

using namespace System::Threading::Tasks;

class ServiceClient;


namespace Hyperwave
{
namespace Shell
{
public
    ref class ServiceConnection
    {
    public:
        ServiceConnection();
        ~ServiceConnection()
        {
            this->!ServiceConnection();
        }
        !ServiceConnection();

        property bool IsConnected
        {
            bool get();
        }

        Task<bool> ^ GetEnabledAsync();
        Task<unsigned int> ^ GetInitialDelayAsync();
        Task<unsigned int> ^ GetIntervalDelayAsync();
        Task<bool> ^ GetSupressFullscreenAsync();

        Task ^ SetEnabledAsync(bool value);
        Task ^ SetInitialDelayAsync(unsigned int value);
        Task ^ SetIntervalDelayAsync(unsigned int value);
        Task ^ SetSupressFullscreenAsync(bool value);

		event System::EventHandler ^ StateChanged;
	private:
        void OnStateChanged();

    private:
        ServiceClient* mClient;
    };
}
}
