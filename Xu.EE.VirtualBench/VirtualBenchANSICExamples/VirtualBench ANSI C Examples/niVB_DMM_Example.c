/******************************************************************************
*
* niVB_DMM_Example.c
*
* Description:
*    This examples demonstrates how to make measurements using the Digital
*    Multimeter (DMM) on a VirtualBench.
*
* Instructions for Running:
*    1. Modify the "deviceName" parameter to be the name of the device that is
*       being used.  By default it is set to "virtualbench".
*    2. Modify the generation configuration.  These are the defaults:
*          Function: DC Volts
*          Auto Ranging: True
*          Manual Range: 10V (unused if Auto Ranging is true)
*    3. Build and run the program.
*
* Notes:
*    Strings in the VirtualBench API are represented as UTF-8. For simplicity,
*    this example uses printf(), which may or may not be correct for UTF-8
*    output given your platform's locale settings.
*
*    Notably, on Windows, printf() uses the ANSI codepage, while the console
*    uses the OEM codepage; wprintf() may be more useful on that platform.
*    See niVB_DMM_Example_utf16.c for an example that does this.
******************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <nivirtualbench/nivirtualbench.h>

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
   const char *deviceName = "virtualbench";
   niVB_DMM_InstrumentHandle dmmHandle = NULL;

   /* Channel Configuration */
   const niVB_DMM_Function function = niVB_DMM_Function_DCVolts;
   const bool autoRange = true;
   const double manualRange = 10.0;

   /* Data */
   double measurement = 0.0;
   const size_t numMeasurements = 10;

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_DMM_Initialize(libHandle, deviceName, /* reset: */ true, &dmmHandle));

   /* Configure the measurement. */
   niVB_ErrorCheck(niVB_DMM_ConfigureMeasurement(dmmHandle, function, autoRange, manualRange));

   /* Take the measurements. */
   {
      size_t i = 0;
      for (i = 0; i < numMeasurements; ++i)
      {
         niVB_ErrorCheck(niVB_DMM_Read(dmmHandle, &measurement));
         if (status == niVB_Status_WarningDMMOverrange)
         {
            printf("Measurement %d: Overrange\n", i);
         }
         else
         {
            printf("Measurement %d: %g\n", i, measurement);
         }
      }
   }

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
   niVB_DMM_Close(dmmHandle);
   niVB_Finalize(libHandle);
   return 0;
}