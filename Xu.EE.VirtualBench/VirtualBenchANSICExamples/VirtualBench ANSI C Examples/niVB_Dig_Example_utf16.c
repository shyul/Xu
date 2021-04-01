/******************************************************************************
*
* niVB_Dig_Example_utf16.c
*
* Description:
*    This examples demonstrates how to generate and read static digital signals
*    using the Digital (Dig) lines on a VirtualBench.
*
* Instructions for Running:
*    1. Modify the device name portion of the "channelsToWrite" and
*       "channelsToRead" parameters to be the name of the device that is being
*       used.  By default it is set to "virtualbench".
*    2. Modify the channel configuration.  These are the defaults:
*          Channels to Write: dig/0:3
*          Channels to Read: dig/4:7
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
#include <windows.h>

static bool* numberToBoolArray(size_t number, bool *data, size_t dataSize)
{
   size_t i = 0;
   for (i = 0; i < dataSize; ++i)
   {
      data[i] = (number & ((size_t)1 << i)) ? true : false;
   }
   return data;
}

static void printDigitalData(const bool *data, size_t dataSize)
{
   size_t i = 0;
   for (i = 0; i < dataSize; ++i)
   {
      if (data[i])
      {
         wprintf(L"T");
      }
      else
      {
         wprintf(L"F");
      }
   }
}

#define niVB_ErrorCheck(function) if (niVB_Status_Assign(&status, (function)) && niVB_Status_Failed(status)) { goto Cleanup; }

int __cdecl main(void)
{
   /* Global Configuration */
   niVB_Status status = niVB_Status_Success;
   niVB_LibraryHandle libHandle = NULL;
   niVB_Dig_InstrumentHandle digHandle = NULL;

   /* Channel Configuration */
   const wchar_t *channelsToWrite = L"virtualbench/dig/0:3";
   const size_t numChannelsToWrite = 4;
   const wchar_t *channelsToRead = L"virtualbench/dig/4:7";
   const size_t numChannelsToRead = 4;

   /* Data */
   bool *dataToWrite = (bool*)malloc(numChannelsToWrite * sizeof(bool));
   bool *dataRead = (bool*)malloc(numChannelsToRead * sizeof(bool));
   const size_t numMeasurements = 10;

   #if defined(_MSC_VER) && _MSC_VER >= 1400
   /* Enable UTF-16 on the console.  This is only available in Visual Studio 2005 and later. */
   _setmode(_fileno(stdout), _O_U16TEXT);
   #endif

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_Dig_InitializeW(libHandle, channelsToWrite, /* reset: */ true, &digHandle));

   /* Write and read the lines. */
   {
      size_t i = 0;

      for (i = 0; i < numMeasurements; ++i)
      {
         niVB_ErrorCheck(niVB_Dig_WriteW(digHandle, channelsToWrite, numberToBoolArray(i, dataToWrite, numChannelsToWrite), numChannelsToWrite));
         /* Note that the Read doesn't require a digital handle. */
         niVB_ErrorCheck(niVB_Dig_ReadW(libHandle, channelsToRead, dataRead, numChannelsToRead, NULL));

         wprintf(L"Iteration %d:\n", i);

         wprintf(L"Wrote to %ls: ", channelsToWrite);
         printDigitalData(dataToWrite, numChannelsToWrite);
         wprintf(L"\n");

         wprintf(L"Read from %ls: ", channelsToRead);
         printDigitalData(dataRead, numChannelsToRead);
         wprintf(L"\n");
      }
   }

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

   /* Clean up any memory that was allocated. */
   free(dataRead);
   free(dataToWrite);

   /* Clean up the handles that were initialized. */
   niVB_Dig_Close(digHandle);
   niVB_Finalize(libHandle);
   return 0;
}
