/******************************************************************************
*
* niVB_MSO_Simple_Example.c
*
* Description:
*    This examples demonstrates how to configure and acquire data from the
*    Mixed Signal Oscilloscope (MSO) instrument on a VirtualBench using
*    the built in auto setup functionality.
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
*    See niVB_MSO_Example_utf16.c for an example that does this.
******************************************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <nivirtualbench/nivirtualbench.h>

#if defined(_WIN32)
#include <windows.h>
#else
#include <time.h>
#endif

static niVB_MSO_SamplingMode querySamplingMode(niVB_MSO_InstrumentHandle msoHandle)
{
   niVB_MSO_SamplingMode samplingMode = niVB_MSO_SamplingMode_Sample;
   if (niVB_MSO_QueryTiming(msoHandle, NULL, NULL, NULL, &samplingMode) != niVB_Status_Success)
   {
      printf("Error querying timing.\n");
   }
   return samplingMode;
}

static size_t queryNumChannels(niVB_LibraryHandle libHandle, niVB_MSO_InstrumentHandle msoHandle)
{
   size_t numChannels = 0, enabledChannelsSize = 0;
   char *enabledChannels = NULL;
   bool failed = true;

   if (!niVB_Status_Failed(niVB_MSO_QueryEnabledAnalogChannels(msoHandle, NULL, 0, &enabledChannelsSize)))
   {
      enabledChannels = (char*)malloc(enabledChannelsSize * sizeof(char));
      if (!niVB_Status_Failed(niVB_MSO_QueryEnabledAnalogChannels(msoHandle, enabledChannels, enabledChannelsSize, NULL)))
      {
         if (!niVB_Status_Failed(niVB_CollapseChannelString(libHandle, enabledChannels, NULL, 0, NULL, &numChannels)))
         {
            failed = false;
         }
      }
   }

   if (failed)
   {
      printf("Error querying number of channels!\n");
   }

   free(enabledChannels);
   return numChannels;
}

static const char* getTriggerReasonStr(niVB_MSO_TriggerReason triggerReason)
{
   switch (triggerReason)
   {
      case niVB_MSO_TriggerReason_Auto:   return "Auto";
      case niVB_MSO_TriggerReason_Forced: return "Forced";
      case niVB_MSO_TriggerReason_Normal: return "Normal";
      default: return "Unknown";
   }
}

static void printTriggerTimestamp(niVB_Timestamp ts)
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
         printf("Trigger Timestamp: %s\n\t%g fractional seconds\n", timeString, subSeconds);
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
      printf("Trigger Timestamp: %s\t%g fractional seconds\n", asctime(localTime), subSeconds);
      #endif
   }
}

static double getTimestampDifference(niVB_Timestamp a, niVB_Timestamp b)
{
   int64_t aSeconds, bSeconds;
   double aSubSeconds, bSubSeconds;

   if (niVB_Status_Failed(niVB_ConvertTimestampToValues(a, &aSeconds, &aSubSeconds)) ||
       niVB_Status_Failed(niVB_ConvertTimestampToValues(b, &bSeconds, &bSubSeconds)))
   {
      return 0.0;
   }

   return (double)(aSeconds - bSeconds) + aSubSeconds - bSubSeconds;
}

static void printAnalogData(double *data, size_t size, size_t stride, size_t numChannels, niVB_MSO_SamplingMode sampleMode, size_t numToPrint)
{
   size_t samp, chan, numSamplesAcquired;

   numSamplesAcquired = size / stride;
   if (numSamplesAcquired < numToPrint) numToPrint = numSamplesAcquired;

   printf("Analog Data (%d of %d):\n", numToPrint, numSamplesAcquired);

   /* Print channel header information. */
   printf("Channel:\t");
   for (chan = 0; chan < numChannels; ++chan)
   {
      if (chan != 0) printf("\t\t");
      printf("%d", chan);
      if (sampleMode == niVB_MSO_SamplingMode_PeakDetect)
      {
         printf(" (min)\t\t%d (max)", chan);
      }
   }
   printf("\n");

   /* Print the data itself. */
   for (samp = 0; samp < numToPrint; ++samp)
   {
      printf("Sample %d:\t", samp);
      for (chan = 0; chan < stride; ++chan)
      {
         if (chan != 0) printf("\t");
         printf("%4.6f", data[samp * stride + chan]);
      }
      printf("\n");
   }
}

static void printDigitalData(uint64_t *data, uint32_t *timestamps, size_t size, size_t numToPrint)
{
   size_t samp;

   if (size < numToPrint) numToPrint = size;

   printf("Digital Data (%d of %d):\n", numToPrint, size);
   printf("\t\tTimestamp\tData\n");
   for (samp = 0; samp < numToPrint; ++samp)
   {
      printf("Sample %d:\t%d\t\t0x%09llX\n", samp, timestamps[samp], data[samp]);
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
   niVB_MSO_InstrumentHandle msoHandle = NULL;

   /* Data */
   size_t analogDataSize = 0, analogDataStride = 0, digitalDataSize = 0, digitalTimestampsSize = 0;
   double *analogData = NULL;
   uint64_t *digitalData = NULL;
   uint32_t *digitalTimestamps = NULL;
   niVB_Timestamp analogT0, digitalT0, triggerTimestamp;
   niVB_MSO_TriggerReason triggerReason;
   size_t numSamplesToPrint = 10;

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_MSO_Initialize(libHandle, deviceName, /* reset: */ true, &msoHandle));

   /* Configure the acquisition using auto setup. */
   niVB_ErrorCheck(niVB_MSO_Autosetup(msoHandle));
   /* Failure to find suitable signals is indicated by a warning. */
   if (status == niVB_Status_Success)
   {
      /* Query the configuration that was chosen to properly interpret the data. */
      niVB_MSO_SamplingMode samplingMode = querySamplingMode(msoHandle);
      size_t numAnalogChannels = queryNumChannels(libHandle, msoHandle);

      /* Start the acquisition.  Auto triggering is enabled to catch a misconfigured trigger condition. */
      niVB_ErrorCheck(niVB_MSO_Run(msoHandle, /* autoTrigger: */ true));

      /* Read the data by first querying how big the data needs to be, allocating the memory, and finally performing the read. */
      niVB_ErrorCheck(niVB_MSO_ReadAnalogDigitalU64(
         msoHandle,
         NULL,
         0,
         &analogDataSize,
         NULL,
         NULL,
         NULL,
         0,
         &digitalDataSize,
         NULL,
         0,
         &digitalTimestampsSize,
         NULL,
         NULL,
         NULL));

      if (analogDataSize != 0)
      {
         analogData = (double*)malloc(analogDataSize*sizeof(double));
      }
      if (digitalDataSize != 0)
      {
         digitalData = (uint64_t*)malloc(digitalDataSize*sizeof(uint64_t));
         digitalTimestamps = (uint32_t*)malloc(digitalTimestampsSize*sizeof(uint32_t));
      }

      niVB_ErrorCheck(niVB_MSO_ReadAnalogDigitalU64(
         msoHandle,
         analogData,
         analogDataSize,
         NULL,
         &analogDataStride,
         &analogT0,
         digitalData,
         digitalDataSize,
         NULL,
         digitalTimestamps,
         digitalTimestampsSize,
         NULL,
         &digitalT0,
         &triggerTimestamp,
         &triggerReason));

      /* Finally, print some information about the data. */
      printf("Trigger Reason: %s\n", getTriggerReasonStr(triggerReason));
      printTriggerTimestamp(triggerTimestamp);
      if (analogDataSize != 0)  printf("Analog t0 is %g seconds before trigger\n", getTimestampDifference(triggerTimestamp, analogT0));
      if (digitalDataSize != 0) printf("Digital t0 is %g seconds before trigger\n", getTimestampDifference(triggerTimestamp, digitalT0));
      if (analogDataSize != 0)  printAnalogData(analogData, analogDataSize, analogDataStride, numAnalogChannels, samplingMode, numSamplesToPrint);
      if (digitalDataSize != 0) printDigitalData(digitalData, digitalTimestamps, digitalDataSize, numSamplesToPrint);
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
   free(digitalTimestamps);
   free(digitalData);
   free(analogData);

   /* Clean up the handles that were initialized. */
   niVB_MSO_Close(msoHandle);
   niVB_Finalize(libHandle);
   return 0;
}
