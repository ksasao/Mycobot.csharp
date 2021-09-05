using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Mycobot
{
    public class MyCobot : IDisposable
    {
        public const string name = "mycobot";
        private static SerialPort serialPort;
        private bool disposedValue;

        public MyCobot(string port, int baud=115200)
        {
            serialPort = new SerialPort(port, baud){DtrEnable = true, RtsEnable = true};
        }

        public bool IsOpen => serialPort != null && serialPort.IsOpen;

        public bool Open()
        {
            try
            {
                if (serialPort == null)
                {
                    return this.IsOpen;
                }
                if (serialPort.IsOpen)
                {
                    this.Close();
                }
                serialPort.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                serialPort.Close();
            }

            return this.IsOpen;
        }

        public void Close()
        {
            if (this.IsOpen)
            {
                serialPort.Close();
            }
        }

        /// <summary>
        /// Write byte[] to buffer, when port is open
        /// </summary>
        /// <param name="send">byte[]</param>
        /// <param name="offset">offset of 0</param>
        /// <param name="count">data count</param>
        public void Write(byte[] send, int offset, int count)
        {
            if (this.IsOpen)
            {
                serialPort.Write(send, offset, count);
            }
        }
        /// <summary>
        /// Write byte[] to buffer, when port is open
        /// </summary>
        /// <param name="send">byte[]</param>
        public void Write(byte[] send)
        {
            Write(send, 0, send.Length);
        }


        #region [Overall status]

        /// <summary>
        /// get robot version
        /// </summary>
        public int GetRobotVersion()
        {
            Message(Code.GetRobotVersion);
            var data = WaitForReply();
            return (int)data[0];
        }
        /// <summary>
        /// get system version
        /// </summary>
        public int GetSystemVersion()
        {
            Message(Code.GetSystemVersion);
            var data = WaitForReply(40);
            return (int)data[0];
        }

        /// <summary>
        /// arm power on
        /// </summary>
        public void PowerOn()
        {
            Message(Code.PowerOn);
        }
        
        /// <summary>
        /// arm power off
        /// </summary>
        public void PowerOff()
        {
            Message(Code.PowerOff);
        }

        /// <summary>
        /// Arm power status
        /// </summary>
        /// <returns>True when arm power supplied</returns>
        public bool IsPowerOn()
        {
            Message(Code.IsPoweredOn);
            var data = WaitForReply(120);
            return data.Length != 0;
        }

        /// <summary>
        /// Release All Servos
        /// </summary>
        public void ReleaseAllServos()
        {
            Message(Code.ReleaseAllServos);
        }

        /// <summary>
        /// Atom connected
        /// </summary>
        /// <returns>True when arm power supplied</returns>
        public bool IsControllerConnected()
        {
            Message(Code.IsControllerConnected);
            var data = WaitForReply(50);
            return data.Length != 0;
        }

        #endregion

        #region [MDI mode and operation]
        /// <summary>
        /// Get all angles
        /// </summary>
        /// <returns>int[], length: 6</returns>
        public int[] GetAngles()
        {
            Message(Code.GetAngles);
            var data = WaitForReply(40);

            if (data.Length != 6 * 2)
            {
                return Array.Empty<int>();
            }
            var res = new int[6];
            for (var i = 0; i < 6; i++)
            {
                short v = (short)(data[i * 2] << 8 | data[i * 2 + 1]);
                res[i] = v / 100;
            }
            return res;
        }

        /// <summary>
        /// Send one angle value
        /// </summary>
        /// <param name="jointNo">joint number: 1 ~ 6</param>
        /// <param name="angle">angle value: -180 ~ 180 </param>
        /// <param name="speed">speed value: 0 ~ 100</param>
        public void SendAngle(int jointNo, int angle, int speed)
        {
            int _angle = angle * 100;
            var data = new List<byte>();
            data.Add((byte)jointNo);
            data.Add((byte)((_angle >> 8) & 0xff));
            data.Add((byte)(_angle & 0xff));
            data.Add((byte)speed);
            Message(Code.WriteAngle, data);
        }

        /// <summary>
        /// Send all angles
        /// </summary>
        /// <param name="angles">angles[], length: 6</param>
        /// <param name="speed">speed value: 0 ~ 100</param>
        public void SendAngles(int[] angles, int speed)
        {
            int[] sendData = new int[6];
            var data = new List<byte>();
            for (var j = 0; j < angles.Length; ++j)
            {
                sendData[j] = angles[j]*100;
            }
            var a = Int16ArrToBytes(sendData);
            data.AddRange(a);
            data.Add((byte)speed);
            Message(Code.WriteAngles, data);
        }

        /// <summary>
        /// Get all coord
        /// </summary>
        /// <returns>int[], length: 6</returns>
        public int[] GetCoords()
        {
            Message(Code.GetCoords);
            var data = WaitForReply();

            if (data.Length != 6 * 2)
            {
                return Array.Empty<int>();
            }

            var res = new int[6];
            for (var i = 0; i < 6; i++)
            {
                short v = (short)(data[i*2] << 8 | data[i*2+1]);
                res[i] = i < 3 ? v / 10 : v / 100;
            }
            return res;
        }

        /// <summary>
        /// Send one coord
        /// </summary>
        /// <param name="coord">coord No: 1 - 6</param>
        /// <param name="value">coord value</param>
        /// <param name="speed">speed: 0 ~ 100</param>
        public void SendCoord(int coord, int value, int speed)
        {
            int val = coord <= 3 ? value * 10 : value * 100 ;
            var data = new List<byte>();
            // process data
            data.Add((byte)(coord - 1));
            data.Add((byte)((val >> 8) & 0xff));
            data.Add((byte)(val & 0xff));
            data.Add((byte)speed);
            Message(Code.WriteCoord, data);
        }

        /// <summary>
        /// Send all coords to arm
        /// </summary>
        /// <param name="coords">int[], length: 6</param>
        /// <param name="speed">speed: int, value: 0 ~ 100</param>
        /// <param name="mode">mode:  0 - angular, 1 - linear</param>
        public void SendCoords(int[] coords, int speed, int mode)
        {
            // process coords
            for (var i = 0; i < 3; ++i)
            {
                coords[i] *= 10;
            }
            for (var i = 3; i < 6; ++i)
            {
                coords[i] *= 100;
            }

            // append to command
            var data = new List<byte>();
            var a = Int16ArrToBytes(coords);
            data.AddRange(a);
            data.Add((byte)speed);
            data.Add((byte)mode);

            Message(Code.WriteCoords, data);
        }
        #endregion

        #region [Servo control]
        /// <summary>
        /// Get state of servo enabled
        /// </summary>
        /// <param name="servoId">1 - 6</param>
        /// <returns></returns>
        public bool IsServoEnable(int servoId)
        {
            Message(Code.IsServoEnabled,(byte)servoId);
            var data = WaitForReply();
            return data.Length != 0;
        }
        /// <summary>
        /// Get All Status of Servos
        /// </summary>
        /// <returns></returns>
        public bool IsAllServoEnable()
        {
            Message(Code.IsAllServoEnabled);
            var data = WaitForReply();
            return data.Length != 0;
        }

        public byte GetServoData(int servoNo, int dataId)
        {
            Message(Code.GetServoData,new byte[] {(byte)servoNo, (byte)dataId });
            var data = WaitForReply();
            if(data.Length == 0)
            {
                return 0;
            }
            return data[0];
        }

        /// <summary>
        /// Power off designated servo
        /// </summary>
        /// <param name="servoId">1 ~ 6</param>
        public void ReleaseServo(int servoId)
        {
            Message(Code.ReleaseServo, (byte)servoId);
        }

        /// <summary>
        /// Power on designated serv
        /// </summary>
        /// <param name="servoId">1 ~ 6</param>
        public void FocusServo(int servoId)
        {
            Message(Code.FocusServo, (byte)servoId);
        }
        #endregion

        #region [Atom IO]
        /// <summary>
        /// Set pin mode
        /// </summary>
        /// <param name="pinNo">GPIO Pin (ATOM)</param>
        /// <param name="pinState">true: OUTPUT / false: INPUT</param>
        public void SetPinMode(int pinNo, bool pinState)
        {
            Message(Code.SetPinMode, new byte[] { (byte)pinNo, (byte)(pinState ? 1 : 0) });
        }

        /// <summary>
        /// Set digital output
        /// </summary>
        /// <param name="pinNo">GPIO Pin (ATOM)</param>
        /// <param name="pinState">true: HIGH(+3.3V) / false: LOW(0V)</param>
        public void SetDigitalOuput(int pinNo, bool pinState)
        {
            Message(Code.SetDigitalOutput, new byte[] { (byte)pinNo, (byte)(pinState ? 1 : 0) });
        }

        /// <summary>
        /// Get digital input
        /// </summary>
        /// <param name="pinNo">GPIO Pin (ATOM)</param>
        /// <returns>true: HIGH(+3.3V) / false: LOW(0V)</returns>
        public bool GetDigitalInput(int pinNo)
        {
            Message(Code.GetDigitalInput, (byte)pinNo);
            var data = WaitForReply(40);
            if (data.Length == 0)
            {
                return false;
            }
            return (data[0] > 0);
        }

        /// <summary>
        /// Set PWM output
        /// </summary>
        /// <param name="pinNo">GPIO Pin (ATOM)</param>
        /// <param name="freq">Frequency (Hz)</param>
        /// <param name="duty">Duty (0-254)</param>
        public void SetPwmOutput(int pinNo, short freq, byte duty)
        {
            Message(Code.SetPwmOutput, new byte[] {
                (byte)pinNo,
                (byte)((freq >> 8) & 0xFF),
                (byte)(freq & 0xFF),
                duty
            });
        }

        /// <summary>
        /// Set the light color
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public void SetColor(byte r, byte g, byte b)
        {
            Message(Code.SetLed, new byte[] { r, g, b });
        }

        #endregion

        #region [BASIC]
        /// <summary>
        /// Set digital output
        /// </summary>
        /// <param name="pinNo">GPIO Pin (BASIC)</param>
        /// <param name="pinState">true: HIGH(+3.3V) / false: LOW(0V)</param>
        public void SetBasicDigitalOutput(int pinNo, bool pinState)
        {
            Message(Code.SetBasicDigitalOutput, new byte[] { (byte)pinNo, (byte)(pinState ? 1 : 0) });
        }
        #endregion

        private static byte[] Int16ToBytes(int v)
        {
            var res = new byte[2];
            res[0] = (byte)((v >> 8) & 0xff);
            res[1] = (byte)((v) & 0xff);
            return res;
        }

        private static IEnumerable<byte> Int16ArrToBytes(IReadOnlyCollection<int> vs)
        {
            var res = new byte[vs.Count * 2];
            var idx = 0;
            foreach (var t in vs)
            {
                var one = Int16ToBytes(t);
                res[idx++] = one[0];
                res[idx++] = one[1];
            }

            return res;
        }

        private byte[] WaitForReply(int millisec)
        {
            Thread.Sleep(millisec);
            var receivedBytes = new byte[serialPort.BytesToRead];
            var result = serialPort.Read(receivedBytes, 0, receivedBytes.Length);
            if (result <= 0)
                return Array.Empty<byte>();

            // get valid index
            var idx = GetValidIndex(receivedBytes);
            if (idx == -1)
                return Array.Empty<byte>();

            // process data
            var len = (int)receivedBytes[idx] - 2;
            if (len > receivedBytes.Length - idx - 2)
                return Array.Empty<byte>();

            byte[] data = new byte[len];
            Array.Copy(receivedBytes, idx + 2, data, 0, len);
            return data;
        }
        private byte[] WaitForReply()
        {
            return WaitForReply(200);
        }

        private static int GetValidIndex(IReadOnlyList<byte> bs)
        {
            for (var i = 0; i < bs.Count - 1; ++i)
            {
                if (bs[i] == 0xfe && bs[i + 1] == 0xfe)
                    return i + 2;
            }
            return -1;
        }

        private void Message(byte command, byte[] data)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Code.Header);
            bytes.Add(Code.Header);
            bytes.Add((byte)(data.Length + 2));
            bytes.Add(command);
            bytes.AddRange(data);
            bytes.Add(Code.Footer);
            Write(bytes.ToArray());
        }

        private void Message(byte command, List<byte> data)
        {
            Message(command, data.ToArray());
        }
        private void Message(byte command)
        {
            Message(command, new byte[] { });
        }
        private void Message(byte command, byte data)
        {
            Message(command, new byte[] { data });
        }

        // Dispose pattern
        // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (serialPort != null)
                    {
                        serialPort.Dispose();
                        serialPort = null;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
