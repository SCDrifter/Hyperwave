#pragma once
#include "IShellLogger.h"
namespace Hyperwave
{
namespace Shell
{
    public interface class IShellLoggerFactory
    {
		public:
			virtual IShellLogger ^ Create(System::String^ name) = 0;
    };
};
};
