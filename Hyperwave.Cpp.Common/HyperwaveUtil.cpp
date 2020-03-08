#include "pch.h"

#include "HyperwaveUtil.h"
#include <Windows.h>
wchar_t* HyperwaveUtil::GetApplicationDirectory()
{
    wchar_t* ret;
    wchar_t text[MAX_PATH + 1] = L"";
    DWORD len = GetModuleFileName(NULL, text, MAX_PATH + 1);
    if (len == 0)
        return nullptr;

	ret = new wchar_t[len + 1];

    if (len < MAX_PATH)            
        lstrcpyn(ret, text, len + 1);    
    else    
        GetModuleFileName(NULL, ret, len + 1);
	
    wchar_t* item = wcsrchr(ret, '\\');
    *item = 0;
    return ret;
}
