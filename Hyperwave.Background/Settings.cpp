#include "pch.h"

#include "Settings.h"
#include <winreg.h>

#include "..\\Hyperwave.Cpp.Common\HyperwaveUtil.h"

#define SETTINGS_LOCATION L"Software\\Zukalitech\\Hyperwave"

static bool RegReadBool(LPCWSTR name, bool default_value);
static unsigned int RegReadInt(LPCWSTR name, unsigned int default_value);
static wchar_t* RegReadGlobalString(LPCWSTR name);

static void RegWriteBool(HKEY key, LPCWSTR name, bool value);
static void RegWriteInt(HKEY key, LPCWSTR name, unsigned int value);

Settings::Settings()
{
    mEnabled = false;
    mInitialDelay = 0;
    mIntervalDelay = 0;
    mSupressFullscreen = false;
    mHyperwaveDirectory = nullptr;
}

Settings::~Settings()
{
    if (mHyperwaveDirectory != nullptr)
        delete[] mHyperwaveDirectory;
}

void Settings::Load()
{
    mHyperwaveDirectory = HyperwaveUtil::GetApplicationDirectory();
    mEnabled = RegReadBool(L"Enabled", false);
    mInitialDelay = RegReadInt(L"InitialDelay", 60 * 3);
    mIntervalDelay = RegReadInt(L"IntervalDelay", 60 * 30);
    mSupressFullscreen = RegReadBool(L"SupressFullscreen", true);
}

void Settings::Save() const
{
    HKEY key;

    if (SUCCEEDED(RegCreateKeyEx(HKEY_CURRENT_USER, SETTINGS_LOCATION, 0, nullptr, 0, GENERIC_WRITE, NULL, &key, nullptr)))
    {

        RegWriteBool(key, L"Enabled", mEnabled);
        RegWriteInt(key, L"InitialDelay", mInitialDelay);
        RegWriteInt(key, L"IntervalDelay", mIntervalDelay);
        RegWriteBool(key, L"SupressFullscreen", mSupressFullscreen);

        RegCloseKey(key);
    }
    if (SUCCEEDED(RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Microsoft\\Windows\\CurrentVersion\\Run", 0, GENERIC_WRITE, &key)))
    {

        if (!mEnabled)
            RegDeleteValue(key, L"Hyperwave");
        else
        {
            wchar_t path[MAX_PATH];
            swprintf_s(path, L"\"%s\\Hyperwave.Background.exe\"", mHyperwaveDirectory);
            DWORD len = lstrlen(path) + 1;
            RegSetValueEx(key, L"Hyperwave", 0, REG_SZ, (BYTE*)path, len * sizeof(wchar_t));

        }

        RegCloseKey(key);
    }
}

static bool RegReadBool(LPCWSTR name, bool default_value)
{
    DWORD result = 0;
    DWORD data_size = sizeof(DWORD);
    DWORD type = REG_DWORD;
    if (FAILED(RegGetValue(HKEY_CURRENT_USER, SETTINGS_LOCATION, name, RRF_RT_DWORD, &type, &result, &data_size)) || type == REG_NONE)
        result = default_value ? 1 : 0;
    return result != 0;
}

static unsigned int RegReadInt(LPCWSTR name, unsigned int default_value)
{
    DWORD result;
    DWORD data_size = sizeof(DWORD);
    DWORD type = REG_DWORD;
    if (FAILED(RegGetValue(HKEY_CURRENT_USER, SETTINGS_LOCATION, name, RRF_RT_DWORD, &type, &result, &data_size)) || type == REG_NONE)
        result = default_value;
    return result;
}
static wchar_t* RegReadGlobalString(LPCWSTR name)
{
    DWORD size = 0;
    if (FAILED(RegGetValue(HKEY_LOCAL_MACHINE, SETTINGS_LOCATION, name, RRF_RT_REG_SZ, nullptr, nullptr, &size)))
        return nullptr;

    wchar_t* ret = new wchar_t[size / sizeof(wchar_t)];

    if (FAILED(RegGetValue(HKEY_LOCAL_MACHINE, SETTINGS_LOCATION, name, RRF_RT_REG_SZ, nullptr, ret, &size)))
    {
        delete[] ret;
        return nullptr;
    }

    return ret;
}

static void RegWriteBool(HKEY key, LPCWSTR name, bool value)
{

    DWORD data = value ? 1 : 0;

    RegSetValueExW(key, name, 0, REG_DWORD, (BYTE*)&data, sizeof(data));
}

static void RegWriteInt(HKEY key, LPCWSTR name, unsigned int value)
{
    DWORD data = value;

    RegSetValueExW(key, name, 0, REG_DWORD, (BYTE*)&data, sizeof(data));
}