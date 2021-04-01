/******************************************************************************
*
* niVB_FGEN_Example_utf16.c
*
* Description:
*    This examples demonstrates how to configure and generate a standard
*    waveform from the Function Generator (FGEN) on a VirtualBench.
*
* Instructions for Running:
*    1. Modify the "deviceName" parameter to be the name of the device that is
*       being used.  By default it is set to "virtualbench".
*    2. Modify the generation configuration.  These are the defaults:
*          Waveform: Square
*          Amplitude: 10V
*          DC Offset: 0V
*          Frequency: 500kHz
*          Duty Cycle: 50% (Used for Square and Triangle waveforms)
*    3. Build and run the program.
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
#error This example requires the Windows API.
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

#define niVB_ErrorCheck(function) if (niVB_Status_Assign(&status, (function)) && niVB_Status_Failed(status)) { goto Cleanup; }

int __cdecl main(void)
{
   /* Global Configuration */
   niVB_Status status = niVB_Status_Success;
   niVB_LibraryHandle libHandle = NULL;
   const wchar_t *deviceName = L"virtualbench";
   niVB_FGEN_InstrumentHandle fgenHandle = NULL;

   /* Waveform Configuration */
   const niVB_Waveform waveform = niVB_Waveform_Square;
   const double amplitude = 10.0;
   const double dcOffset = 0.0;
   const double frequency = 500000.0;
   const double dutyCycle = 50.0;

   #if defined(_MSC_VER) && _MSC_VER >= 1400
   /* Enable UTF-16 on the console.  This is only available in Visual Studio 2005 and later. */
   _setmode(_fileno(stdout), _O_U16TEXT);
   #endif

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_FGEN_InitializeW(libHandle, deviceName, /* reset: */ true, &fgenHandle));

   /* Configure the standard waveform. */
   niVB_ErrorCheck(niVB_FGEN_ConfigureStandardWaveform(fgenHandle, waveform, amplitude, dcOffset, frequency, dutyCycle));

   /* Start driving the signal.  The waveform will continue until Stop is called, even if you close the session. */
   niVB_ErrorCheck(niVB_FGEN_Run(fgenHandle));

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
   niVB_FGEN_Close(fgenHandle);
   niVB_Finalize(libHandle);
   return 0;
 }
