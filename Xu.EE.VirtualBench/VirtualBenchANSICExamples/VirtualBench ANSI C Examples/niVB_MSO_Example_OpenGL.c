/******************************************************************************
*
* niVB_MSO_Example_OpenGL.c
*
* Description:
*    This examples demonstrates how to configure and acquire data from the
*    Mixed Signal Oscilloscope (MSO) instrument on a VirtualBench and 
*    display the data using OpenGL.
*
* Instructions for Running:
*    1. Modify the "deviceName" parameter to be the name of the device that is
*       being used.  By default it is set to "virtualbench".
*    2. Modify the acquisition configuration.  These are the defaults:
*          Analog Channel Configuration
*          ============================
*          Channels: mso/1, mso/2
*          Vertical Range: 10V
*          Vertical Offset: 0V
*          Probe Attenutation: 1x
*          Coupling: DC
*
*          Digital Channel Configuration
*          =============================
*          Channels: mso/d0 through mso/31, mso/clk0, and mso/clk1 (34 total)
*
*          Timing Configuration
*          ====================
*          Analog Sample Rate: 500MHz
*          Analog Sampling Mode: Sample
*          Digital Sample Rate: 1GHz
*          Acquisition Time: 12us
*          Pretrigger Time: 6us
*
*          Trigger Configuration
*          =====================
*          Type: Analog Edge Trigger
*          Source: mso/1
*          Level: 1V
*          Hysteresis: Minimum
*          Edge: Rising
*    3. Build and run the program.
*
* Notes:
*     This examples require the OpenGL Utility Toolkit which is available from:
*     http://www.opengl.org/resources/libraries/glut/
*
******************************************************************************/

#if defined(_WIN32)
#include <windows.h>
#endif

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <GL/glut.h>
#include <nivirtualbench/nivirtualbench.h>

/* Global Configuration */
const char *deviceName = "virtualbench";
niVB_LibraryHandle libHandle = NULL;
niVB_MSO_InstrumentHandle msoHandle = NULL;

/* Analog Channel Configuration */
const char *analogChannels = "mso/1:2";
const size_t numAnalogChannels = 2;
const double verticalRange = 10.0;
const double verticalOffset = 0.0;
const niVB_MSO_ProbeAttenuation probeAttenuation = niVB_MSO_ProbeAttenuation_1x;
const niVB_MSO_Coupling coupling = niVB_MSO_Coupling_DC;

/* Digital Channel Configuration */
const char *digitalChannels = "mso/d0:31, mso/clk0:1";
const size_t numDigitalChannelsToDraw = 4;

/* Timing Configuration */
double analogSampleRate = 500e6;
double digitalSampleRate = 1e9;
double acquisitionTime = 12e-6;
double pretriggerTime = 6e-6;
niVB_MSO_SamplingMode samplingMode = niVB_MSO_SamplingMode_Sample;
niVB_MSO_DigitalSampleRateControl digitalSampleRateControl = niVB_MSO_DigitalSampleRateControl_Manual;
niVB_MSO_BufferControl bufferControl = niVB_MSO_BufferControl_Automatic;
double preTriggerPercent = 0.0;

/* Trigger Configuration */
const char *triggerSource = "mso/1";
const niVB_EdgeWithEither triggerSlope = niVB_EdgeWithEither_Rising;
const double triggerLevel = 1.0;
const double triggerHysteresis = 0.0; /* Minimum */
const niVB_MSO_TriggerInstance triggerInstance = niVB_MSO_TriggerInstance_A;

/* Graph Configuration */
const int numHorizontalDivisions = 10;
const int numVerticalDivisions = 8;
double startTime, endTime, timePerDivision;
double startVoltage, endVoltage, voltsPerDivision;
double analogSamplePeriod, digitalSamplePeriod;

/* Data */
double analogStartTime = 0.0, digitalStartTime = 0.0;
size_t analogDataSize = 0, analogDataStride = 0, digitalDataSize = 0, digitalTimestampsSize = 0;
double *analogData = NULL;
uint64_t *digitalData = NULL;
uint32_t *digitalTimestamps = NULL;

#define niVB_ErrorCheck(function) if (niVB_Status_Assign(&status, (function)) && niVB_Status_Failed(status)) { goto Cleanup; }

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

static void handleError(niVB_LibraryHandle libHandle, niVB_Status status)
{
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
}

static niVB_Status readData(
   niVB_MSO_InstrumentHandle msoHandle,
   double *analogStartTime,
   double **analogData,
   size_t *analogDataSize,
   double *digitalStartTime,
   uint64_t **digitalData,
   size_t *digitalDataSize,
   uint32_t **digitalTimestamps,
   size_t *digitalTimestampsSize)
{
   niVB_Status status = niVB_Status_Success;
   niVB_Timestamp analogT0, digitalT0, triggerTimestamp;
   niVB_MSO_TriggerReason triggerReason;

   /* Start the acquisition.  Auto triggering is enabled to catch a misconfigured trigger condition. */
   niVB_ErrorCheck(niVB_MSO_Run(msoHandle, /* autoTrigger: */ true));

   /* Read the data by first querying how big the data needs to be, allocating the memory, and finally performing the read. */
   niVB_ErrorCheck(niVB_MSO_ReadAnalogDigitalU64(
      msoHandle,
      NULL,
      0,
      analogDataSize,
      NULL,
      NULL,
      NULL,
      0,
      digitalDataSize,
      NULL,
      0,
      digitalTimestampsSize,
      NULL,
      NULL,
      NULL));

   *analogData = (double*)realloc(*analogData, *analogDataSize*sizeof(double));
   *digitalData = (uint64_t*)realloc(*digitalData, *digitalDataSize*sizeof(uint64_t));
   *digitalTimestamps = (uint32_t*)realloc(*digitalTimestamps, *digitalTimestampsSize*sizeof(uint32_t));

   niVB_ErrorCheck(niVB_MSO_ReadAnalogDigitalU64(
      msoHandle,
      *analogData,
      *analogDataSize,
      NULL,
      &analogDataStride,
      &analogT0,
      *digitalData,
      *digitalDataSize,
      NULL,
      *digitalTimestamps,
      *digitalTimestampsSize,
      NULL,
      &digitalT0,
      &triggerTimestamp,
      &triggerReason));

   *analogStartTime = getTimestampDifference(analogT0, triggerTimestamp);
   *digitalStartTime = getTimestampDifference(digitalT0, triggerTimestamp);

Cleanup:
   return status;
}

static void drawGraticule(double startTime, double endTime, double startVoltage, double endVoltage)
{
   double time, voltage;

   glColor3ub(128, 128, 128);

   glBegin(GL_LINES);
   for (time = startTime; time < endTime; time += timePerDivision)
   {
      glVertex2d(time, startVoltage);
      glVertex2d(time, endVoltage);
   }

   for (voltage = endVoltage; voltage < startVoltage; voltage += voltsPerDivision)
   {
      glVertex2d(startTime, voltage);
      glVertex2d(endTime, voltage);
   }
   glEnd();
}

static void drawAnalogData(
   double analogStartTime,
   double analogSamplePeriod,
   double *analogData,
   size_t analogDataSize,
   size_t analogDataStride,
   size_t numAnalogChannels,
   niVB_MSO_SamplingMode sampleMode)
{
   size_t samp, chan, samplesPerChannel, numSamplesAcquired;
   double time, voltage;

   switch (sampleMode)
   {
   case niVB_MSO_SamplingMode_Sample:     samplesPerChannel = 1; break;
   case niVB_MSO_SamplingMode_PeakDetect: samplesPerChannel = 2; break;
   default: printf("Error: Unknown sample mode!\n"); return;
   }

   numSamplesAcquired = analogDataSize / analogDataStride;

   for (chan = 0; chan < numAnalogChannels; ++chan)
   {
      switch (chan)
      {
      case 0: glColor3ub(255, 0, 0); break;
      case 1: glColor3ub(255, 255, 0); break;
      default: printf("Error: Unknown channel!\n"); return;
      }

      glBegin(GL_LINE_STRIP);
      for (samp = 0; samp < numSamplesAcquired; ++samp)
      {
         time = analogStartTime + samp * analogSamplePeriod;
         voltage = analogData[samp * analogDataStride + chan * samplesPerChannel];
         glVertex2d(time, voltage);
      }
      glEnd();

      if (sampleMode == niVB_MSO_SamplingMode_PeakDetect)
      {
         glBegin(GL_LINE_STRIP);
         for (samp = 0; samp < numSamplesAcquired; ++samp)
         {
            time = analogStartTime + samp * analogSamplePeriod;
            voltage = analogData[samp * analogDataStride + chan * samplesPerChannel + 1];
            glVertex2d(time, voltage);
         }
         glEnd();
      }
   }
}

static void drawDigitalData(
   double digitalStartTime,
   double digitalSamplePeriod,
   uint64_t *digitalData,
   size_t digitalDataSize,
   uint32_t *digitalTimestamps,
   size_t numDigitalChannelsToDraw)
{
   const double voltsPerLine = voltsPerDivision / 2;
   const double lineOffset = voltsPerLine * 0.1;
   const double lineHeight = voltsPerLine * 0.8;

   size_t chan, samp;
   bool value;
   double time, voltage, prevVoltage = 0.0;

   glColor3ub(0, 128, 0);

   for (chan = 0; chan < numDigitalChannelsToDraw; ++chan)
   {
      glBegin(GL_LINE_STRIP);
      for (samp = 0; samp < digitalDataSize; ++samp)
      {
         value = digitalData[samp] & (1i64 << chan) ? true : false;
         time = digitalStartTime + digitalTimestamps[samp] * digitalSamplePeriod;
         voltage = startVoltage - voltsPerLine * (chan + 1) + lineOffset;

         if (value)
         {
            voltage += lineHeight;
         }

         if (samp != 0 && voltage != prevVoltage)
         {
            glVertex2d(time, prevVoltage);
         }

         prevVoltage = voltage;

         glVertex2d(time, voltage);
      }
      glEnd();
   }
}

static void GLUTCALLBACK reshape(int width, int height)
{
   glViewport(0, 0, width, height);
   glMatrixMode(GL_PROJECTION);
   glLoadIdentity();
   gluOrtho2D(startTime, endTime, endVoltage, startVoltage);
   glMatrixMode(GL_MODELVIEW);
}

static void GLUTCALLBACK idle(void)
{
   niVB_Status status = niVB_Status_Success;

   niVB_ErrorCheck(readData(
      msoHandle,
      &analogStartTime,
      &analogData,
      &analogDataSize,
      &digitalStartTime,
      &digitalData,
      &digitalDataSize,
      &digitalTimestamps,
      &digitalTimestampsSize));

   glutPostRedisplay();

Cleanup:
   handleError(libHandle, status);
}

static void GLUTCALLBACK display(void)
{
   glClear(GL_COLOR_BUFFER_BIT);

   drawGraticule(startTime, endTime, startVoltage, endVoltage);
   drawAnalogData(analogStartTime, analogSamplePeriod, analogData, analogDataSize, analogDataStride, numAnalogChannels, samplingMode);
   drawDigitalData(digitalStartTime, digitalSamplePeriod, digitalData, digitalDataSize, digitalTimestamps, numDigitalChannelsToDraw);

   glutSwapBuffers();
}

static void GLUTCALLBACK finalize(void)
{
   /* Clean up any memory that was allocated. */
   free(digitalTimestamps);
   free(digitalData);
   free(analogData);

   /* Clean up the handles that were initialized. */
   niVB_MSO_Close(msoHandle);
   niVB_Finalize(libHandle);
}

int main(int argc, char **argv)
{
   niVB_Status status = niVB_Status_Success;

   niVB_ErrorCheck(niVB_Initialize(NIVB_LIBRARY_VERSION, &libHandle));

   /* Initialize the instrument, this can fail if the device is unreachable or the instrument is reserved. */
   niVB_ErrorCheck(niVB_MSO_Initialize(libHandle, deviceName, /* reset: */ true, &msoHandle));

   /* Configure the analog and digital channels. */
   niVB_ErrorCheck(niVB_MSO_ConfigureAnalogChannel(msoHandle, analogChannels, /* enable: */ true, verticalRange, verticalOffset, probeAttenuation, coupling));
   niVB_ErrorCheck(niVB_MSO_EnableDigitalChannels(msoHandle, digitalChannels, /* enable: */ true));

   /* Configure timing. */
   niVB_ErrorCheck(niVB_MSO_ConfigureTiming(msoHandle, analogSampleRate, acquisitionTime, pretriggerTime, samplingMode));
   niVB_ErrorCheck(niVB_MSO_ConfigureAdvancedDigitalTiming(msoHandle, digitalSampleRateControl, digitalSampleRate, bufferControl, preTriggerPercent));

   /* Configure trigger condition. */
   niVB_ErrorCheck(niVB_MSO_ConfigureAnalogEdgeTrigger(msoHandle, triggerSource, triggerSlope, triggerLevel, triggerHysteresis, triggerInstance));

   /* Query timing to update coerced values. */
   niVB_ErrorCheck(niVB_MSO_QueryTiming(msoHandle, &analogSampleRate, &acquisitionTime, &pretriggerTime, &samplingMode));
   niVB_ErrorCheck(niVB_MSO_QueryAdvancedDigitalTiming(msoHandle, &digitalSampleRateControl, &digitalSampleRate, &bufferControl, &preTriggerPercent));

   timePerDivision = acquisitionTime / (double)numHorizontalDivisions;
   startTime = -pretriggerTime;
   endTime = startTime + acquisitionTime;
   voltsPerDivision = 1.0;
   startVoltage = (double)numVerticalDivisions / 2.0;
   endVoltage = -startVoltage;
   analogSamplePeriod = 1.0 / analogSampleRate;
   digitalSamplePeriod = 1.0 / digitalSampleRate;

   glutInit(&argc, argv);
   glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB);
   glutInitWindowSize(800, 600);
   glutCreateWindow("MSO Example");
   glClearColor(0.0, 0.0, 0.0, 1.0);
   glutIdleFunc(idle);
   glutReshapeFunc(reshape);
   glutDisplayFunc(display);
   atexit(finalize);
   glutMainLoop();

Cleanup:
   handleError(libHandle, status);

   return 0;
}
