/******************************************************************************
*
* niVB_Dig_Example.c
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
*    Strings in the VirtualBench API are represented as UTF-8. For simplicity,
*    this example uses printf(), which may or may not be correct for UTF-8
*    output given your platform's locale settings.
*
*    Notably, on Windows, printf() uses the ANSI codepage, while the console
*    uses the OEM codepage; wprintf() may be more useful on that platform.
*    See niVB_Dig_Example_utf16.c for an example that does this.
******************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <nivirtualbench/nivirtualbench.h>

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
         printf("T");
      }
      else
      {
         printf("F");
      }
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
   niVB_Dig_InstrumentHandle digHandle = NULL;

   /* Channel Configuration */
   const char *channelsToWrite = "VB8012-309528E/dig/0:3";
   const size_t numChannelsToWrite = 4;
   const char *channelsToRead = "VB8012-309528E/dig/4:7";
   const size_t numChannelsToRead = 4;

   /* Data */
   bool *dataToWrite = (bool*)malloc(numChannelsToWrite * sizeof(bool));
   bool *dataRead = (bool*)malloc(numChannelsToRead * sizeof(bool));
   const size_t numMeasurements = 10;

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_Dig_Initialize(libHandle, channelsToWrite, /* reset: */ true, &digHandle));

   /* Write and read the lines. */
   {
      size_t i = 0;

      for (i = 0; i < numMeasurements; ++i)
      {
         niVB_ErrorCheck(niVB_Dig_Write(digHandle, channelsToWrite, numberToBoolArray(i, dataToWrite, numChannelsToWrite), numChannelsToWrite));
         /* Note that the Read doesn't require a digital handle. */
         niVB_ErrorCheck(niVB_Dig_Read(libHandle, channelsToRead, dataRead, numChannelsToRead, NULL));

         printf("Iteration %d:\n", i);

         printf("Wrote to %s: ", channelsToWrite);
         printDigitalData(dataToWrite, numChannelsToWrite);
         printf("\n");

         printf("Read from %s: ", channelsToRead);
         printDigitalData(dataRead, numChannelsToRead);
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
   niVB_Dig_Close(digHandle);
   niVB_Finalize(libHandle);
   return 0;
}
