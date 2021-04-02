using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.VirtualBench
{
    public partial class NiVB 
    {
        public void NiVB_ErrorCheck(int statusCode) 
        {
            
        
        }

        public NiVB_Status Status
        {
            get => m_Status;

            set
            {
                m_Status = value;
                Console.WriteLine("Status is updated: " + m_Status);
            }
        }

        private NiVB_Status m_Status = NiVB_Status.Success;

    }

    public enum NiVB_Status : int
    {
        Success = 0,
        ErrorCalFunctionNotSupported = -375995,
        ErrorInputTerminationOverloaded = -375993,
        ErrorArbClipping = -375992,
        ErrorInvalidOperationForMultipleChansEdgeTrigger = -375991,
        ErrorI2CArbLost = -375990,
        ErrorI2CNak = -375989,
        ErrorI2CTimeout = -375988,
        ErrorUnknownDevicePidOrVid = -375987,
        ErrorCannotStartTransferWhileInProgress = -375986,
        ErrorInvalidPointer = -375985,
        ErrorInvalidFrameSize = -375984,
        ErrorInvalidNextCalDate = -375983,
        ErrorSetNextCalDateWithLastCalDate = -375982,
        ErrorLastCalDateBlank = -375981,
        ErrorDeviceNotInStorage = -375980,
        ErrorDeviceDidNotReboot = -375979,
        ErrorInvalidConfigurationFileWrongModel = -375978,
        ErrorInvalidDeviceNameHasAllNumbers = -375977,
        ErrorHostnameResolutionTimeout = -375976,
        ErrorHostnameResolutionFailure = -375975,
        ErrorDigitalInitializationFailed = -375974,
        ErrorFirmwareIsTooNew = -375971,
        ErrorFirmwareIsTooOld = -375970,
        ErrorInvalidMethod = -375969,
        ErrorOvertemp = -375968,
        ErrorInvalidDeviceName = -375967,
        ErrorFGENOvervoltage = -375966,
        ErrorCannotRenameDeviceBecauseNameInUse = -375965,
        ErrorDeviceWithSameNameAlreadyExists = -375964,
        ErrorInternalStorageFailure = -375963,
        ErrorLAAcquisitionLength = -375962,
        ErrorScopeAcquisitionLength = -375961,
        ErrorCannotDeleteUsb = -375960,
        ErrorInvalidNetworkPathSyntax = -375959,
        ErrorCannotRunWhenNoChannelsEnabled = -375958,
        ErrorTriggerPatternSize = -375957,
        ErrorTransportTimeout = -375956,
        ErrorPSCurrentCalNeedsShortCircuit = -375955,
        ErrorPSVoltageCalNeedsOpenCircuit = -375954,
        ErrorHardwareFault = -375950,
        ErrorNoPermissionForOperationWhenNotLoggedIn = -375949,
        ErrorNoPermissionForOperation = -375948,
        ErrorAuthenticationFailure = -375947,
        ErrorAuthenticationCredentialsInvalid = -375946,
        ErrorPSReadWhenDisabled = -375945,
        ErrorDeviceIsNotAuthentic = -375944,
        ErrorCalibrationSignalInvalid = -375943,
        ErrorInvalidCalibrationOrder = -375942,
        ErrorUnknownDevice = -375941,
        ErrorCalFailed = -375940,
        ErrorNotEnoughCalRefPoints = -375939,
        ErrorNoCalRefPoint = -375938,
        ErrorInvalidCalRefPoint = -375937,
        ErrorPSInitializationFailed = -375936,
        ErrorFGENInitializationFailed = -375935,
        ErrorScopeInitializationFailed = -375934,
        ErrorDMMInitializationFailed = -375933,
        ErrorConfigDataIsCorrupt = -375932,
        ErrorPNGFileDoesNotContainConfigurationData = -375931,
        ErrorPNGFileIsCorrupt = -375930,
        ErrorNoPermissionToWriteFile = -375929,
        ErrorNoPermissionToReadFile = -375928,
        ErrorFileIOError = -375927,
        ErrorNoSpaceLeftOnDevice = -375926,
        ErrorInvalidFileName = -375925,
        ErrorUnknownConfigurationFileFormat = -375924,
        ErrorCalibrationCorrupt = -375923,
        ErrorInvalidCalibrationPassword = -375922,
        ErrorTooManySavedConfigurations = -375921,
        ErrorConfigurationNameIsTooLong = -375920,
        ErrorSavedConfigurationDataIsTooLarge = -375919,
        ErrorSavedConfigurationAlreadyExists = -375918,
        ErrorSavedConfigurationDoesNotExist = -375917,
        ErrorInvalidConfigurationFileFormat = -375916,
        ErrorInvalidOperationForMultipleChans = -375914,
        ErrorCannotForceTriggerWhenStopped = -375913,
        ErrorOnlyOneChannelValidForRate = -375912,
        ErrorMultipleTriggerSources = -375911,
        ErrorTriggerChannelsNotEnabled = -375910,
        ErrorCannotReadWhenStopped = -375909,
        ErrorNotConnected = -375908,
        ErrorCannotChangeConfigWhileRunning = -375907,
        ErrorInvalidSession = -375906,
        ErrorInvalidChannelName = -375905,
        ErrorDigNumLinesDoesntMatchData = -375904,
        ErrorReservationFailed = -375903,
        ErrorInvalidConfiguration = -375902,
        ErrorOutOfMemory = -375901,
        ErrorInternal = -375900,
        WarningCApiHeaderOutOfDate = 374310,
        WarningNotEnoughMemory = 374309,
        WarningDeviceNameUsedAsHostname = 374308,
        WarningDMMHardwareOverrange = 374307,
        WarningDeviceDifferentFromExpected = 374306,
        WarningDeviceHasBeenRenamed = 374305,
        WarningNoSignalSuitableForSampleRateInAutoSetup = 374304,
        WarningNoSignalsFoundForAutoSetup = 374303,
        WarningImportArbMode = 374302,
        WarningDMMOverrange = 374301,
        WarningArbClipping = 374300,
    }
}
