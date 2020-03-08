#include "pch.h"

#include "Logger.h"
#include <ShlObj.h>

void CreateDirectories(wchar_t* path)
{
    wchar_t* item = path;

    while (item != nullptr)
    {
        wchar_t* nextitem = wcschr(item, '\\');
        if (nextitem == nullptr)
        {
            CreateDirectory(path, NULL);
            item = nextitem;
        }
        else
        {
            *nextitem = 0;
            CreateDirectory(path, NULL);
            *nextitem = '\\';
            item = nextitem + 1;
        }
    }
}

Logger::Logger(bool enabled)
    : mEnabled(enabled)
{
    mLineBuffer = nullptr;
    mLineBufferCount = 0;
    mConvertBuffer = nullptr;
    mConvertBufferCount = 0;
    if (!mEnabled)
    {
        ZeroMemory(mLogDir, sizeof(mLogDir));
    }
    else
    {
        PWSTR root;
        SHGetKnownFolderPath(FOLDERID_RoamingAppData, 0, NULL, &root);
        swprintf_s(mLogDir, L"%s\\Zukalitech\\Logs\\Hyperwave\\Background", root);
        CoTaskMemFree(root);
        CreateDirectories(mLogDir);
    }
}

Logger::~Logger()
{
    if (mLineBuffer != nullptr)
        free(mLineBuffer);
    if (mConvertBuffer != nullptr)
        free(mConvertBuffer);
}

void Logger::Trace(const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"TRACE", format, ap);
    va_end(ap);
}

void Logger::Debug(const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"DEBUG", format, ap);
    va_end(ap);
}

void Logger::Info(const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"INFO", format, ap);
    va_end(ap);
}

void Logger::Warn(const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"WARN", format, ap);
    va_end(ap);
}

void Logger::Error(const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"ERR", format, ap);
    va_end(ap);
}

void Logger::Fatal(const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"FATAL", format, ap);
    va_end(ap);
}

void Logger::Error(DWORD err_code, const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"FATAL", format, ap, err_code);
    va_end(ap);
}

void Logger::Fatal(DWORD err_code, const wchar_t* format, ...)
{
    va_list ap;
    va_start(ap, format);
    Log(L"FATAL", format, ap, err_code);
    va_end(ap);
}

void Logger::Log(const wchar_t* loglevel, const wchar_t* format, va_list args, DWORD error)
{
    if (!mEnabled)
        return;

    SYSTEMTIME tm;
    wchar_t header[256];

    GetSystemTime(&tm);

    va_list measure;
    va_copy(measure, args);
    int count = _vscwprintf(format, measure) + 1;
    va_end(measure);

    if (!AllocateBuffer(count))
        return;

    vswprintf_s(mLineBuffer, mLineBufferCount, format, args);

    swprintf_s(header, L"%02d:%02d:%02d.%04d [%s]: ", tm.wHour, tm.wMinute, tm.wSecond, tm.wMilliseconds, loglevel);

    HANDLE file;

    if (!OpenAndLockFile(&tm, &file))
        return;

    WriteText(file, header);
    WriteText(file, mLineBuffer);
    if (error == ERROR_SUCCESS)
        WriteText(file, L"\r\n");
    else
    {
        swprintf_s(header, L": [%d]", error);
        WriteText(file, header);
        wchar_t* err_text = nullptr;
        FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, 0, GetLastError(), 0, (LPWSTR)&err_text, 1024, nullptr);
        WriteText(file, err_text);
        LocalFree(err_text);
	}
    UnlockAndCloseFile(file);
}

bool Logger::AllocateBuffer(size_t count)
{
    if (count > mLineBufferCount)
    {
        wchar_t* buffer;
        if (mLineBuffer == nullptr)
            buffer = (wchar_t*)malloc(count * sizeof(wchar_t));
        else
            buffer = (wchar_t*)realloc(mLineBuffer, count * sizeof(wchar_t));

        if (buffer == nullptr)
            return false;
        else
        {
            mLineBuffer = buffer;
            mLineBufferCount = count;
        }
    }
    return true;
}

bool Logger::AllocateConvertBuffer(size_t count)
{
    if (count > mConvertBufferCount)
    {
        char* buffer;
        if (mConvertBuffer == nullptr)
            buffer = (char*)malloc(count * sizeof(char));
        else
            buffer = (char*)realloc(mConvertBuffer, count * sizeof(char));

        if (buffer == nullptr)
            return false;
        else
        {
            mConvertBuffer = buffer;
            mConvertBufferCount = count;
        }
    }
    return true;
}

bool Logger::OpenAndLockFile(PSYSTEMTIME tm, HANDLE* result)
{
    wchar_t fname[MAX_PATH];
    swprintf_s(fname, L"%s\\%04d-%02d-%02d.log.txt", mLogDir, tm->wYear, tm->wMonth, tm->wDay);
    *result = CreateFile(fname, GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_ALWAYS, 0, NULL);
    if (*result == INVALID_HANDLE_VALUE)
        return false;
    OVERLAPPED o = { 0 };
    o.Offset = ~0;
    o.OffsetHigh = ~0;
    if (!LockFileEx(*result, LOCKFILE_EXCLUSIVE_LOCK, 0, 1, 0, &o))
    {
        CloseHandle(*result);
        return false;
    }

    SetFilePointer(*result, 0, 0, FILE_END);
    return true;
}

void Logger::UnlockAndCloseFile(HANDLE file)
{
    OVERLAPPED o = { 0 };
    o.Offset = ~0;
    o.OffsetHigh = ~0;
    UnlockFileEx(file, 0, 1, 0, &o);
    CloseHandle(file);
}

void Logger::WriteText(HANDLE file, const wchar_t* text)
{
    int count = WideCharToMultiByte(CP_UTF8, 0, text, -1, nullptr, 0, NULL, NULL);
    if (!AllocateConvertBuffer(count))
        return;
    DWORD size = WideCharToMultiByte(CP_UTF8, 0, text, -1, mConvertBuffer, mConvertBufferCount, NULL, NULL) - 1;
    DWORD w;
    WriteFile(file, mConvertBuffer, size, &w, NULL);
}
