using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DataPlotter
{
    class UDPDataListener : DataListener
    {
        #region private varaibles
        private UdpClient client_;
        #endregion

        #region public constructors
        public UDPDataListener(int port)
        {
            client_ = new UdpClient(port);
        }
        #endregion

        #region public methods
        public override string WaitForData()
        {
            string str;

            do
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] data;
                try
                {
                    data = client_.Receive(ref ep);
                }
                catch
                {
                    return string.Empty;
                }
                if (data[0] == 0xFF && data[data.Length - 1] == 0xFE)
                    str = DecodeBinaryPacket(data);
                else
                    str = Encoding.ASCII.GetString(data);
            } while (!str.StartsWith("$") || !str.EndsWith("$"));

            return str;
        }

        public override void ShutDown()
        {
            client_.Close();
        }
        #endregion

        #region private methods
        private string DecodeBinaryPacket(byte [] data)
        {
            string str = "$data,";
            int index = 1;
            int v;

            v = data[index++] << 24;
            v |= data[index++] << 16;
            v |= data[index++] << 8;
            v |= data[index++];
            str += v.ToString() + ",";

            v = data[index++] << 24;
            v |= data[index++] << 16;
            v |= data[index++] << 8;
            v |= data[index++];
            str += v.ToString() + ",";

            v = data[index++];
            str += v.ToString() + ",";

            double value = BitConverter.ToDouble(data, index);
            str += value.ToString() + "$";

            return str;
        }
        #endregion

    }
}
