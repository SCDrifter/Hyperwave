#pragma once
namespace Hyperwave
{
namespace Shell
{
	// Since we can't reference NLog directly from this module we will use a instance of this class to smuggle that functionality in
    public interface class IShellLogger
    {
    public:
        virtual void Info(System::String ^ text) = 0;
        virtual void Info(System::String ^ format, ... cli::array<System::Object ^> ^ args) = 0;
        virtual void Trace(System::String ^ text) = 0;
        virtual void Trace(System::String ^ format, ... cli::array<System::Object ^> ^ args) = 0;
        virtual void Debug(System::String ^ text) = 0;
        virtual void Debug(System::String ^ format, ... cli::array<System::Object ^> ^ args) = 0;
        virtual void Warning(System::String ^ text) = 0;
        virtual void Warning(System::String ^ format, ... cli::array<System::Object ^> ^ args) = 0;
        virtual void Error(System::String ^ text) = 0;
        virtual void Error(System::String ^ format, ... cli::array<System::Object ^> ^ args) = 0;
        virtual void Fatal(System::String ^ text) = 0;
        virtual void Fatal(System::String ^ format, ... cli::array<System::Object ^> ^ args) = 0;
    };
};
};