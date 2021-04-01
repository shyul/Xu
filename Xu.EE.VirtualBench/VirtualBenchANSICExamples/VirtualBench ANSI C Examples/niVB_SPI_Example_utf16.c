/******************************************************************************
*
* niVB_SPI_Example.c
*
* Description:
*    This examples demonstrates how to master a SPI bus using the Digital lines
*    on a VirtualBench.
*
* Instructions for Running:
*    1. Modify the device name portion of the "bus" parameter to be the name of
*       the device that is being used.  By default it is set to "virtualbench".
*    2. Modify the bus configuration.  These are the defaults:
*          Bus: spi/0
*          Clock Rate: 10MHz
*          Clock Polarity: Low
*          Clock Phase: First Edge
*          Chip Select Polarity: High
*    3. Modify the data configuration.  These are the defaults:
*          Transaction Size: 8 bytes
*          Bytes Per Frame: -1 (one frame)
*    4. Build and run the program.
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

static uint8_t* initializeDataArray(uint8_t *data, size_t dataSize)
{
   size_t i = 0;
   for (i = 0; i < dataSize; ++i)
   {
      data[i] = (uint8_t)i;
   }
   return data;
}

static void printDataArray(const uint8_t *data, size_t dataSize)
{
   size_t i = 0;
   for (i = 0; i < dataSize; ++i)
   {
      wprintf(L"%d ", data[i]);
   }
}

#define niVB_ErrorCheck(function) if (niVB_Status_Assign(&status, (function)) && niVB_Status_Failed(status)) { goto Cleanup; }

int __cdecl main(void)
{
   /* Global Configuration */
   niVB_Status status = niVB_Status_Success;
   niVB_LibraryHandle libHandle = NULL;
   niVB_SPI_InstrumentHandle spiHandle = NULL;

   /* Channel Configuration */
   const wchar_t *bus = L"virtualbench/spi/0";
   const double clockRate = 10000000.0;
   const niVB_Polarity clockPolarity = niVB_Polarity_IdleLow;
   const niVB_ClockPhase clockPhase = niVB_ClockPhase_FirstEdge;
   const niVB_Polarity chipSelectPolarity = niVB_Polarity_IdleHigh;

   /* Data */
   const size_t transactionSize = 8;
   const int32_t bytesPerFrame = -1;
   uint8_t *dataToWrite = (uint8_t*)malloc(transactionSize * sizeof(uint8_t));
   uint8_t *dataRead = (uint8_t*)malloc(transactionSize * sizeof(uint8_t));
   const size_t numTransactions = 10;

   #if defined(_MSC_VER) && _MSC_VER >= 1400
   /* Enable UTF-16 on the console.  This is only available in Visual Studio 2005 and later. */
   _setmode(_fileno(stdout), _O_U16TEXT);
   #endif

   /* Initialize the Data to Write. */
   dataToWrite = initializeDataArray(dataToWrite, transactionSize);

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_SPI_InitializeW(libHandle, bus, /* reset: */ true, &spiHandle));

   /* Configure the bus. */
   niVB_ErrorCheck(niVB_SPI_ConfigureBus(spiHandle, clockRate, clockPolarity, clockPhase, chipSelectPolarity));

   /* Write and read from the bus. */
   {
      size_t i = 0;

      for (i = 0; i < numTransactions; ++i)
      {
         niVB_ErrorCheck(niVB_SPI_WriteRead(spiHandle, dataToWrite, transactionSize, bytesPerFrame, dataRead, transactionSize));

         wprintf(L"Iteration %d:\n", i);

         wprintf(L"Wrote: ");
         printDataArray(dataToWrite, transactionSize);
         wprintf(L"\n");

         wprintf(L"Read: ");
         printDataArray(dataRead, transactionSize);
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
   niVB_SPI_Close(spiHandle);
   niVB_Finalize(libHandle);
   return 0;
}
