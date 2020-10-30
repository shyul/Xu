using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NationalInstruments.VisaNS;
using Xu;

namespace TestFSQ
{
    public abstract class ViClient : IDisposable, IEquatable<ViClient>
    {
        public ViClient(string resourceName) => Open(resourceName);

        public virtual void Dispose() => Session?.Dispose();

        protected MessageBasedSession Session { get; set; }

        public string ResourceName { get; private set; }

        public string VendorName { get; private set; } = "Unknown";

        public string Model { get; private set; } = "Unknown";

        public string SerialNumber { get; private set; } = "Unknown";

        public string DeviceVersion { get; private set; } = "Unknown";

        protected void Open(string resourceName)
        {
            try
            {
                ResourceName = resourceName;
                Session = ResourceManager.GetLocalManager().Open(ResourceName) as MessageBasedSession;

                string[] result = Query("*IDN?\n").Split(',');
                if (result.Length > 3)
                {
                    VendorName = result[0].Trim();
                    Model = result[1].Trim();
                    SerialNumber = result[2].Trim();
                    DeviceVersion = result[3].Trim();
                }
            }
            catch (InvalidCastException iexp)
            {
                Session = null;
                Console.WriteLine("Type must be \"MessageBasedSession\": " + iexp.Message);
            }
            catch (Exception exp)
            {
                Session = null;
                Console.WriteLine("Opening: " + resourceName + " | " + exp.Message);
            }
            finally
            {
                // Send message connection is established
            }
        }

        public void Close() => Session.Dispose();

        public void Write(string cmd)
        {
            WriteNoErrorCheck(cmd);

            if (GetError() is ViException error && error.Code != 0)
            {
                Console.WriteLine(error.Code + " || " + error.Message);
                throw error;
            }
        }

        private void WriteNoErrorCheck(string cmd)
        {
            try
            {
                lock (Session)
                    Session.Write(ReplaceCommonEscapeSequences(cmd));
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        public string Read()
        {
            string res = ReadNoErrorCheck();

            if (GetError() is ViException error && error.Code != 0)
            {
                Console.WriteLine(error.Code + " || " + error.Message);
                throw error;
            }
            else
                return res;
        }

        private string ReadNoErrorCheck()
        {
            try
            {
                string res = null;

                lock (Session)
                {
                    res = Session.ReadString();
                }

                return res;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return null;
            }
            finally
            {
                // Send message
            }
        }

        public string Query(string cmd)
        {
            string res = QueryNoErrorCheck(cmd);

            if (GetError() is ViException error && error.Code != 0)
            {
                Console.WriteLine(error.Code + " || " + error.Message);
                throw error;
            }
            else
                return res;
        }

        private string QueryNoErrorCheck(string cmd)
        {
            try
            {
                string res = null;

                lock (Session)
                {
                    res = Session.Query(ReplaceCommonEscapeSequences(cmd));
                }

                return res;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return null;
            }
            finally
            {
                // Send message
            }
        }

        public void WriteAsync(string cmd)
        {
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(cmd);
                lock (Session)
                {
                    AsyncHandle = Session.BeginWrite(
                    textToWrite,
                    new AsyncCallback(OnWriteComplete),
                    textToWrite.Length as object);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OnWriteComplete(IAsyncResult result)
        {
            try
            {
                Session.EndWrite(result);
                string elementsTransferredTextBoxText = ((int)result.AsyncState).ToString();
                string lastIOStatusTextBoxText = Session.LastStatus.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void ReadAsync()
        {
            try
            {
                lock (Session)
                {
                    AsyncHandle = Session.BeginRead(
                    Session.DefaultBufferSize,
                    new AsyncCallback(OnReadComplete),
                    null);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OnReadComplete(IAsyncResult result)
        {
            try
            {
                string responseString = Session.EndReadString(result);
                string elementsTransferredTextBoxText = responseString.Length.ToString();
                string lastIOStatusTextBoxText = Session.LastStatus.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void TerminateAsync()
        {
            try
            {
                if (AsyncHandle is IAsyncResult res)
                    Session.Terminate(res);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public double GetNumber(string cmd) => double.Parse(Query(cmd).Trim());

        public ViException GetError() => new ViException(QueryNoErrorCheck("SYST:ERR?\n"));

        public void Reset() => Write("*RST\n");

        public void Trigger() => Write("*TRG\n");

        public void SyncWait() => Write("INIT;*WAI\n");

        public bool IsReady => Session is MessageBasedSession && Query("*OPC?\n") is string s && s.Trim() == "1";

        public bool SelfTest => Query("*TST?\n").Trim() == "1";

        public static string[] FindResources() => ResourceManager.GetLocalManager().FindResources("?*");

        private static string ReplaceCommonEscapeSequences(string s) => s.Replace("\\n", "\n").Replace("\\r", "\r");

        private IAsyncResult AsyncHandle { get; set; } = null;

        public override string ToString() => ResourceName + " | " + VendorName + " | " + Model + " | " + SerialNumber + " | " + DeviceVersion;
        public override int GetHashCode() => ResourceName.GetHashCode();
        public bool Equals(ViClient other) => ResourceName == other.ResourceName;
        public static bool operator !=(ViClient s1, ViClient s2) => !s1.Equals(s2);
        public static bool operator ==(ViClient s1, ViClient s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is ViClient sp && Equals(sp);
    }

    public class ViException : Exception
    {
        public ViException(string returnMessage)
        {
            if (returnMessage is string s)
            {
                var fields = s.Trim().CsvReadFields();

                if (fields.Length > 1)
                {
                    Code = fields[0].ToInt32(-1);
                    Message = fields[1].Trim().Trim('"').Trim();
                }
            }
        }

        public virtual int Code { get; } = 0;

        public override string Message { get; } = string.Empty;
    }
}
