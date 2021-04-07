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
*    Strings in the VirtualBench API are represented as UTF-8. For simplicity,
*    this example uses printf(), which may or may not be correct for UTF-8
*    output given your platform's locale settings.
*
*    Notably, on Windows, printf() uses the ANSI codepage, while the console
*    uses the OEM codepage; wprintf() may be more useful on that platform.
*    See niVB_SPI_Example_utf16.c for an example that does this.
******************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <nivirtualbench/nivirtualbench.h>

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
      printf("%d ", data[i]);
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
   niVB_SPI_InstrumentHandle spiHandle = NULL;

   /* Channel Configuration */
   const char *bus = "VB8012-309528E/spi/0";
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

   /* Initialize the Data to Write. */
   dataToWrite = initializeDataArray(dataToWrite, transactionSize);

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_SPI_Initialize(libHandle, bus, /* reset: */ true, &spiHandle));

   /* Configure the bus. */
   niVB_ErrorCheck(niVB_SPI_ConfigureBus(spiHandle, clockRate, clockPolarity, clockPhase, chipSelectPolarity));

   /* Write and read from the bus. */
   {
      size_t i = 0;

      for (i = 0; i < numTransactions; ++i)
      //while(true)
      {
         niVB_ErrorCheck(niVB_SPI_WriteRead(spiHandle, dataToWrite, transactionSize, bytesPerFrame, dataRead, transactionSize));

         printf("Iteration %d:\n", i);

         printf("Wrote: ");
         printDataArray(dataToWrite, transactionSize);
         printf("\n");

         printf("Read: ");
         printDataArray(dataRead, transactionSize);
         printf("\n");
         //i++;
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

   /* Clean up any memory that was allocated. */
   free(dataRead);
   free(dataToWrite);

   /* Clean up the handles that were initialized. */
   niVB_SPI_Close(spiHandle);
   niVB_Finalize(libHandle);
   return 0;
}
