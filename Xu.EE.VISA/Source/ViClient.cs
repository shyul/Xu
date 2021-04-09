using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;
using Xu;

namespace Xu.EE.Visa
{
    public abstract class ViClient : IDisposable, IEquatable<ViClient>
    {
        public ViClient(string resourceName)
        {
            ResourceName = resourceName;
        }

        public virtual void Dispose() 
        {
            Close();
        }

        protected MessageBasedSession Session { get; set; }

        public string ResourceName { get; private set; }

        public string VendorName { get; private set; } = "Unknown";

        public string Model { get; private set; } = "Unknown";

        public string SerialNumber { get; private set; } = "Unknown";

        public string DeviceVersion { get; private set; } = "Unknown";

        public virtual void Open()
        {
            try
            {
                using var rm = new ResourceManager();
                Session = rm.Open(ResourceName) as MessageBasedSession;

                //Reset();
                ClearStatus();
                while (!IsReady) { Thread.Sleep(200); }

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
                Console.WriteLine("Opening: " + ResourceName + " | " + exp.Message);
            }
            finally
            {
                Console.WriteLine("Connected: " + VendorName + " | " + Model + " | " + SerialNumber + " | " + DeviceVersion);
                // Send message connection is established
            }
        }

        public virtual void Close() => Session?.Dispose();

        public void Write(string cmd, Dictionary<string, string> paramList)
        {
            string s = cmd + ":";
            foreach (var sc in paramList)
            {
                s += sc.Key + " " + sc.Value + "\n";
            }

            Write(s.Trim(';') + "\n");
        }

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
            Console.WriteLine("Write Only: " + cmd);
            try
            {
                lock (Session)
                    Session.RawIO.Write(ReplaceCommonEscapeSequences(cmd));
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
                    res = Session.RawIO.ReadString();
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

        public void Query(string cmd, Dictionary<string, string> paramList)
        {
            string s = cmd + ":";
            foreach (var sc in paramList.Keys)
            {
                s += sc + "?;";
            }

            var list = Query(s.Trim(';') + "\n").Split(';');

            int i = 0;
            foreach (var sc in paramList.Keys)
            {
                paramList[sc] = list[i].Trim();
                i++;
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
            Console.WriteLine("Query: " + cmd);
            try
            {
                string res = null;

                lock (Session)
                {
                    Session.RawIO.Write(ReplaceCommonEscapeSequences(cmd));
                    res = Session.RawIO.ReadString();
                }

                Console.WriteLine("Query Result: " + res);

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
                    VisaAsyncResult = Session.RawIO.BeginWrite(
                    textToWrite,
                    new VisaAsyncCallback(OnWriteComplete),
                    textToWrite.Length as object);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }



        private void OnWriteComplete(IVisaAsyncResult result)
        {
            try
            {
                Session.RawIO.EndWrite(result);
                string elementsTransferredTextBoxText = ((int)result.AsyncState).ToString();
                //string lastIOStatusTextBoxText = Session.LastStatus.ToString();
                VisaAsyncResult = result;
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
                    VisaAsyncResult = Session.RawIO.BeginRead(
                    1024,
                    new VisaAsyncCallback(OnReadComplete),
                    null);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OnReadComplete(IVisaAsyncResult result)
        {
            try
            {
                string responseString = Session.RawIO.EndReadString(result);
                string elementsTransferredTextBoxText = responseString.Length.ToString();
                //VisaAsyncResult = result;
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
                if (VisaAsyncResult is IVisaAsyncResult res)
                    //Session.RawIO.Terminate(res);
                    Session.RawIO.AbortAsyncOperation(res);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public double GetNumber(string cmd) => double.Parse(Query(cmd).Trim());

        public ViException GetError() => new(QueryNoErrorCheck("SYST:ERR?\n"));

        public void Reset() => Write("*RST\n");

        public void ClearStatus() => Write("*CLS\n");

        public void Trigger() => Write("*TRG\n");

        public void SyncWait() => Write("INIT;*WAI\n");

        public bool IsReady => Session is MessageBasedSession && Query("*OPC?\n") is string s && s.Trim() == "1";

        public bool SelfTest => Query("*TST?\n").Trim() == "1";

        public static IEnumerable<string> FindResources()
        {
            using var rm = new ResourceManager();
            return rm.Find("?*");
        }

        private static string ReplaceCommonEscapeSequences(string s) => s.Replace("\\n", "\n").Replace("\\r", "\r");

        public IVisaAsyncResult VisaAsyncResult { get; private set; } = null;

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
