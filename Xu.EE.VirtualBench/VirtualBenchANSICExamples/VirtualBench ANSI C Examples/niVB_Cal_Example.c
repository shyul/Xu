/******************************************************************************
*
* niVB_Cal_Example.c
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
*    Strings in the VirtualBench API are represented as UTF-8. For simplicity,
*    this example uses printf(), which may or may not be correct for UTF-8
*    output given your platform's locale settings.
*
*    Notably, on Windows, printf() uses the ANSI codepage, while the console
*    uses the OEM codepage; wprintf() may be more useful on that platform.
*    See niVB_Cal_Example_utf16.c for an example that does this.
******************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <nivirtualbench/nivirtualbench.h>

#if defined(_WIN32)
#include <windows.h>
#else
#include <time.h>
#endif

/* Note, this method ignores fractional seconds, they aren't typically useful for calibration timestamps. */
static void printTimestamp(niVB_Timestamp ts)
{
   int64_t seconds;
   double subSeconds;

   /* First, convert the VirtualBench specific timestamp to raw data. */
   if (niVB_Status_Failed(niVB_ConvertTimestampToValues(ts, &seconds, &subSeconds)))
   {
      printf("Error converting timestamp to values.\n");
      return;
   }

   /* Now convert to a user-visible time either using Windows or C functions. */
   {
      #if defined(_WIN32)
      FILETIME utcFileTime;
      FILETIME localFileTime;
      LONGLONG rawSystemTimeInNs;
      SYSTEMTIME systemTime;
      char *timeString;
      int timeStringSize;

      rawSystemTimeInNs = seconds * 10000000 /* nanoseconds per second */ + 116444736000000000 /* ns between Windows and Unix epochs */;
      utcFileTime.dwLowDateTime = (DWORD)rawSystemTimeInNs;
      utcFileTime.dwHighDateTime = (DWORD)(rawSystemTimeInNs >> 32);
      if (FileTimeToLocalFileTime(&utcFileTime, &localFileTime) == 0 ||
          FileTimeToSystemTime(&localFileTime, &systemTime) == 0)
      {
         printf("Error converting filetime to systemtime.\n");
         return;
      }

      timeStringSize = GetTimeFormatA(LOCALE_USER_DEFAULT, 0, &systemTime, NULL, NULL, 0);
      timeString = (char*)malloc(timeStringSize * sizeof(char));
      if (GetTimeFormatA(LOCALE_USER_DEFAULT, 0, &systemTime, NULL, timeString, timeStringSize) != 0)
      {
         printf("%d/%d/%d %s\n", systemTime.wMonth, systemTime.wDay, systemTime.wYear, timeString);
      }
      else
      {
         printf("Failed to convert systemtime to string.\n");
      }

      free(timeString);
      #else
      time_t rawtime;
      struct tm* localTime;

      rawtime = seconds;
      localTime = localtime(&rawtime);
      printf("%s", asctime(localTime), subSeconds);
      #endif
   }
}

#define niVB_ErrorCheck(function) if (niVB_Status_Assign(&status, (function)) && niVB_Status_Failed(status)) { goto Cleanup; }

#if defined(_MSC_VER)
int __cdecl main(void)
#else
int main(void)
#endif
{
   /* Global Configuration */
   niVB_Status status = niVB_Status_Success;
   niVB_LibraryHandle libHandle = NULL;
   const char *deviceName = "VB8012-309528E";

   /* Calibration Data */
   niVB_Timestamp calDate;
   int32_t recommendedCalInterval;
   int32_t calInterval;
   niVB_Timestamp adjustDate;
   double adjustTemp;

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Read and display calibration information. */
   niVB_ErrorCheck(niVB_Cal_GetCalibrationInformation(libHandle, deviceName, &calDate, &recommendedCalInterval, &calInterval));
   printf("Device was last calibrated: ");
   printTimestamp(calDate);
   printf("Recommended calibration interval: %d months, calibration interval: %d months\n", recommendedCalInterval, calInterval);

   niVB_ErrorCheck(niVB_MSO_GetCalibrationAdjustmentInformation(libHandle, deviceName, &adjustDate, &adjustTemp));
   printf("MSO was last adjusted: ");
   printTimestamp(adjustDate);
   printf("Temperature was: %g\n", adjustTemp);

   niVB_ErrorCheck(niVB_FGEN_GetCalibrationAdjustmentInformation(libHandle, deviceName, &adjustDate, &adjustTemp));
   printf("FGEN was last adjusted: ");
   printTimestamp(adjustDate);
   printf("Temperature was: %g\n", adjustTemp);

   niVB_ErrorCheck(niVB_DMM_GetCalibrationAdjustmentInformation(libHandle, deviceName, &adjustDate, &adjustTemp));
   printf("DMM was last adjusted: ");
   printTimestamp(adjustDate);
   printf("Temperature was: %g\n", adjustTemp);

   niVB_ErrorCheck(niVB_PS_GetCalibrationAdjustmentInformation(libHandle, deviceName, &adjustDate, &adjustTemp));
   printf("PS was last adjusted: ");
   printTimestamp(adjustDate);
   printf("Temperature was: %g\n", adjustTemp);

Cleanup:
   /* If an error or warning occurred print out any information that is available. */
   if (status != niVB_Status_Success)
   {
      if (libHandle)
      {
         size_t descrSize = 0, extendedErrSize = 0;
         char *descrBuf = NULL, *extendedErrBuf = NULL;

         niVB_GetErrorDescription(libHandle, status, niVB_Language_CurrentThreadLocale, NULL, 0, &descrSize);
         descrBuf = (char*)malloc(descrSize*sizeof(char));
         niVB_GetErrorDescription(libHandle, status, niVB_Language_CurrentThreadLocale, descrBuf, descrSize, NULL);

         niVB_GetExtendedErrorInformation(libHandle, niVB_Language_CurrentThreadLocale, NULL, 0, &extendedErrSize);
         extendedErrBuf = (char*)malloc(extendedErrSize*sizeof(char));
         niVB_GetExtendedErrorInformation(libHandle, niVB_Language_CurrentThreadLocale, extendedErrBuf, extendedErrSize, NULL);

         printf("Error/Warning %d occurred\n", status);
         if (descrSize != 0) printf("Error Description: %s\n", descrBuf);
         if (extendedErrSize != 0) printf("Extended Error Information: %s\n", extendedErrBuf);

         free(descrBuf);
         free(extendedErrBuf);
      }
      else
      {
         printf("Error/Warning %d occurred before library initialization succeeded.\n", status);
      }
   }

   /* Clean up the handles that were initialized. */
   niVB_Finalize(libHandle);
   return 0;
}
