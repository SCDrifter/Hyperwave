#pragma once
class Logger
{
public:
    Logger(bool enabled);
    ~Logger();

    void Trace(const wchar_t* format, ...);
    void Debug(const wchar_t* format, ...);
    void Info(const wchar_t* format, ...);
    void Warn(const wchar_t* format, ...);
    void Error(const wchar_t* format, ...);
    void Fatal(const wchar_t* format, ...);

	
    void Error(DWORD err_code,const wchar_t* format, ...);
    void Fatal(DWORD err_code,const wchar_t* format, ...);

private:
    void Log(const wchar_t* loglevel, const wchar_t* format, va_list args, DWORD error = ERROR_SUCCESS);
    bool AllocateBuffer(size_t count);
    bool AllocateConvertBuffer(size_t count);
    bool OpenAndLockFile(PSYSTEMTIME tm, HANDLE* result);
    void UnlockAndCloseFile(HANDLE file);
    void WriteText(HANDLE file, const wchar_t* text);

private:
    bool mEnabled;
    wchar_t mLogDir[MAX_PATH];
    wchar_t* mLineBuffer;
    size_t mLineBufferCount;

	
    char* mConvertBuffer;
    size_t mConvertBufferCount;
};
