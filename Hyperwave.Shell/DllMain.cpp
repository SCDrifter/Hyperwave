#pragma unmanaged
#include <Windows.h>

HMODULE gLocalModule;

BOOL WINAPI DllMain(
    _In_ HINSTANCE inst,
    _In_ DWORD reason,
    _In_ LPVOID reserved)
{
    if (reason == DLL_PROCESS_ATTACH)
    {
        DisableThreadLibraryCalls(inst);
        gLocalModule = inst;
    }

    return TRUE;
}