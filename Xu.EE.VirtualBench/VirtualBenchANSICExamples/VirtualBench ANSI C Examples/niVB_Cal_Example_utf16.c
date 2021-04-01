/******************************************************************************
*
* niVB_Cal_Example_utf16.c
*
* Description:
*    This examples demonstrates how to query calibration information from a 
*    VirtualBench device.
*
* Instructions for Running:
*    1. Modify the "deviceName" parameter to be the name of the device that is
*       being used.  By default it is set to "virtualbench".
*    2. Build and run the program.
*
* Notes:
*    This example utilizes the UTF-16 VirtualBench APIs that are available on
*    Windows.  This example uses wprintf(), which expects UTF-16.  However, the
*    console that is being used may not be able to render Unicode characters
*    correctly.  In particular the default raster font of the Windows console
*    won't render most code points.  Furthermore, the console itself can't
*    handle any code points that require more than one 16-bit (wchar_t) code
*    unit.
******************************************************************************/

#if !defined(_WIN32)
#error This example is only supported on Windows.
#endif

#if defined(_CVI_)
#error This example is not supported in CVI.
#endif

#define UNICODE
#include <fcntl.h>
#include <io.h>
#include <stdio.h>
#include <stdlib.h>
#include <nivirtualbench/nivirtualbench.h>
#include <windows.h>

/* Note, this method ignores fractional seconds, they aren't typically useful for calibration timestamps. */
static void printTimestamp(niVB_Timestamp ts)
{
   int64_t seconds;
   double subSeconds;

   /* First, convert the VirtualBench specific timestamp to raw data. */
   if (niVB_Status_Failed(niVB_ConvertTimestampToValues(ts, &seconds, &subSeconds)))
   {
      wprintf(L"Error converting timestamp to values.\n");
      return;
   }

   /* Now convert to a user-visible time either using Windows or C functions. */
   {
      FILETIME utcFileTime;
      FILETIME localFileTime;
      LONGLONG rawSystemTimeInNs;
      SYSTEMTIME systemTime;
      wchar_t *timeString;
      int timeStringSize;

      rawSystemTimeInNs = seconds * 10000000 /* nanoseconds per second */ + 116444736000000000 /* ns between Windows and Unix epochs */;
      utcFileTime.dwLowDateTime = (DWORD)rawSystemTimeInNs;
      utcFileTime.dwHighDateTime = (DWORD)(rawSystemTimeInNs >> 32);
      if (FileTimeToLocalFileTime(&utcFileTime, &localFileTime) == 0 ||
          FileTimeToSystemTime(&localFileTime, &systemTime) == 0)
      {
         wprintf(L"Error converting filetime to systemtime.\n");
         return;
      }

      timeStringSize = GetTimeFormatW(LOCALE_USER_DEFAULT, 0, &systemTime, NULL, NULL, 0);
      timeString = (wchar_t*)malloc(timeStringSize * sizeof(wchar_t));
      if (GetTimeFormatW(LOCALE_USER_DEFAULT, 0, &systemTime, NULL, timeString, timeStringSize) != 0)
      {
         wprintf(L"%d/%d/%d %ls\n", systemTime.wMonth, systemTime.wDay, systemTime.wYear, timeString);
      }
      else
      {
         wprintf(L"Failed to convert systemtime to string.\n");
      }

      free(timeString);
   }
}

#define niVB_ErrorCheck(function) if (niVB_Status_Assign(&status, (function)) && niVB_Status_Failed(status)) { goto Cleanup; }

int __cdecl main(void)
{
   /* Global Configuration */
   niVB_Status status = niVB_Status_Success;
   niVB_LibraryHandle libHandle = NULL;
   const wchar_t *deviceName = L"virtualbench";

   /* Calibration Data */
   niVB_Timestamp calDate;
   int32_t recommendedCalInterval;
   int32_t calInterval;
   niVB_Timestamp adjustDate;
   double adjustTemp;

   #if defined(_MSC_VER) && _MSC_VER >= 1400
   /* Enable UTF-16 on the console.  This is only available in Visual Studio 2005 and later. */
   _setmode(_fileno(stdout), _O_U16TEXT);
   #endif

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Read and display calibration information. */
   niVB_ErrorCheck(niVB_Cal_GetCalibrationInformationW(libHandle, deviceName, &calDate, &recommendedCalInterval, &calInterval));
   wprintf(L"Device was last calibrated: ");
   printTimestamp(calDate);
   wprintf(L"Recommended calibration interval: %d months, calibration interval: %d months\n", recommendedCalInterval, calInterval);

   niVB_ErrorCheck(niVB_MSO_GetCalibrationAdjustmentInformationW(libHandle, deviceName, &adjustDate, &adjustTemp));
   wprintf(L"MSO was last adjusted: ");
   printTimestamp(adjustDate);
   wprintf(L"Temperature was: %g\n", adjustTemp);

   niVB_ErrorCheck(niVB_FGEN_GetCalibrationAdjustmentInformationW(libHandle, deviceName, &adjustDate, &adjustTemp));
   wprintf(L"FGEN was last adjusted: ");
   printTimestamp(adjustDate);
   wprintf(L"Temperature was: %g\n", adjustTemp);

   niVB_ErrorCheck(niVB_DMM_GetCalibrationAdjustmentInformationW(libHandle, deviceName, &adjustDate, &adjustTemp));
   wprintf(L"DMM was last adjusted: ");
   printTimestamp(adjustDate);
   wprintf(L"Temperature was: %g\n", adjustTemp);

   niVB_ErrorCheck(niVB_PS_GetCalibrationAdjustmentInformationW(libHandle, deviceName, &adjustDate, &adjustTemp));
   wprintf(L"PS was last adjusted: ");
   printTimestamp(adjustDate);
   wprintf(L"Temperature was: %g\n", adjustTemp);

Cleanup:
   /* If an error or warning occurred print out any information that is available. */
   if (status != niVB_Status_Success)
   {
      if (libHandle)
      {
         size_t descrSize = 0, extendedErrSize = 0;
         wchar_t *descrBuf = NULL, *extendedErrBuf = NULL;

         niVB_GetErrorDescriptionW(libHandle, status, niVB_Language_CurrentThreadLocale, NULL, 0, &descrSize);
         descrBuf = (wchar_t*)malloc(descrSize*sizeof(wchar_t));
         niVB_GetErrorDescriptionW(libHandle, status, niVB_Language_CurrentThreadLocale, descrBuf, descrSize, NULL);

         niVB_GetExtendedErrorInformationW(libHandle, niVB_Language_CurrentThreadLocale, NULL, 0, &extendedErrSize);
         extendedErrBuf = (wchar_t*)malloc(extendedErrSize*sizeof(wchar_t));
         niVB_GetExtendedErrorInformationW(libHandle, niVB_Language_CurrentThreadLocale, extendedErrBuf, extendedErrSize, NULL);

         wprintf(L"Error/Warning %d occurred\n", status);
         if (descrSize != 0) wprintf(L"Error Description: %ls\n", descrBuf);
         if (extendedErrSize != 0) wprintf(L"Extended Error Information: %ls\n", extendedErrBuf);

         free(descrBuf);
         free(extendedErrBuf);
      }
      else
      {
         wprintf(L"Error/Warning %d occurred before library initialization succeeded.\n", status);
      }
   }

   /* Clean up the handles that were initialized. */
   niVB_Finalize(libHandle);
   return 0;
}
