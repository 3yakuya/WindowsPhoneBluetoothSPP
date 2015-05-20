using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Streamoli.Utilities
{
    class BluetoothHelper
    {
        private static BluetoothHelper instance = null;
        private string deviceName;
        private int rfcommPortNumber;
        private DataWriter outputWriter;

        public static async Task<BluetoothHelper> GetInstance(string deviceName, int rfcommPortNumber)
        {
            if (instance == null)
            {
                instance = new BluetoothHelper(deviceName, rfcommPortNumber);
                instance.outputWriter = await instance.GetOutputDataWriter(deviceName, rfcommPortNumber);
            }

            return instance;
        }

        private BluetoothHelper(string deviceName, int rfcommPortNumber)
        {
            this.deviceName = deviceName;
            this.rfcommPortNumber = rfcommPortNumber;
        }

        public static void ShowBluetoothSettings()
        {
            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
            connectionSettingsTask.Show();
        }

        public async Task WriteData(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data.ToCharArray());
            this.outputWriter.WriteBytes(byteData);
            await this.outputWriter.StoreAsync();
            await this.outputWriter.FlushAsync();
        }

        public void Disconnect()
        {
            this.outputWriter.DetachStream();
            this.outputWriter.Dispose();
        }

        private async Task<DataWriter> GetOutputDataWriter(string name, int rfcommPortNumber)
        {
            PeerInformation peerInformation = await GetPeerInformation(name);
            StreamSocket connectedSocket = await Connect(peerInformation, rfcommPortNumber);
            DataWriter streamWriter = new DataWriter(connectedSocket.OutputStream);
            streamWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            return streamWriter;
        }

        private async Task<PeerInformation> GetPeerInformation(string name)
        {
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            var allPeers = await PeerFinder.FindAllPeersAsync();
            var filteredPeers = allPeers.Where(x => x.DisplayName == name).ToList();
            return filteredPeers[0];
        }

        private async Task<StreamSocket> Connect(PeerInformation peerInformation, int rfcommPortNumber)
        {
            StreamSocket socket = GetStreamSocket();
            await socket.ConnectAsync(peerInformation.HostName, rfcommPortNumber.ToString());
            return socket;
        }

        private StreamSocket GetStreamSocket()
        {
            return new StreamSocket();
        }
    }
}
