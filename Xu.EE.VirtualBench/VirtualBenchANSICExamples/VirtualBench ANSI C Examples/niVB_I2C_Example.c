/******************************************************************************
*
* niVB_I2C_Example.c
*
* Description:
*    This examples demonstrates how to master an I2C bus using the Digital
*    lines on a VirtualBench.
*
* Instructions for Running:
*    1. Modify the device name portion of the "bus" parameter to be the name of
*       the device that is being used.  By default it is set to "virtualbench".
*    2. Modify the bus configuration.  These are the defaults:
*          Bus: i2c/0
*          Clock Rate: 100kHz
*          Address: 0x50
*          Address Size: 7 Bits
*          Enable Pullups: False
*    3. Modify the data configuration.  These are the defaults:
*          Write Transaction Size: 8 bytes
*          Read Transaction Size: 8 bytes
*          Timeout: 10s
*    4. Build and run the program.
*
* Notes:
*    Strings in the VirtualBench API are represented as UTF-8. For simplicity,
*    this example uses printf(), which may or may not be correct for UTF-8
*    output given your platform's locale settings.
*
*    Notably, on Windows, printf() uses the ANSI codepage, while the console
*    uses the OEM codepage; wprintf() may be more useful on that platform.
*    See niVB_I2C_Example_utf16.c for an example that does this.
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
   niVB_I2C_InstrumentHandle i2cHandle = NULL;

   /* Channel Configuration */
   const char *bus = "virtualbench/i2c/0";
   const niVB_I2C_ClockRate clockRate = niVB_I2C_ClockRate_100kHz;
   const uint16_t address = 0x50;
   const niVB_I2C_AddressSize addressSize = niVB_I2C_AddressSize_7Bits;
   const bool enablePullUps = false;

   /* Data */
   const size_t writeTransactionSize = 8;
   const size_t readTransactionSize = 8;
   uint8_t *dataToWrite = (uint8_t*)malloc(writeTransactionSize * sizeof(uint8_t));
   uint8_t *dataRead = (uint8_t*)malloc(readTransactionSize * sizeof(uint8_t));
   const double timeout = 10.0;
   const size_t numTransactions = 10;
   int32_t numberOfBytesWritten;

   /* Initialize the Data to be written. */
   dataToWrite = initializeDataArray(dataToWrite, writeTransactionSize);

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_I2C_Initialize(libHandle, bus, /* reset: */ true, &i2cHandle));

   /* Configure the bus. */
   niVB_ErrorCheck(niVB_I2C_ConfigureBus(i2cHandle, clockRate, address, addressSize, enablePullUps));

   /* Write and read from the bus. */
   {
      size_t i = 0;

      for (i = 0; i < numTransactions; ++i)
      {
         niVB_ErrorCheck(niVB_I2C_WriteRead(i2cHandle, dataToWrite, writeTransactionSize, timeout, &numberOfBytesWritten, dataRead, readTransactionSize));

         printf("Iteration %d:\n", i);

         printf("Wrote: ");
         printDataArray(dataToWrite, writeTransactionSize);
         printf("\n");

         printf("Read: ");
         printDataArray(dataRead, readTransactionSize);
         printf("\n");
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
   niVB_I2C_Close(i2cHandle);
   niVB_Finalize(libHandle);
   return 0;
}
