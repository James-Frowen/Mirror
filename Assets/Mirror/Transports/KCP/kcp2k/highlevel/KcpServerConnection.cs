// server needs to store a separate KcpPeer for each connection.
// as well as remoteEndPoint so we know where to send data to.
using System.Net;
using Mirage.SocketLayer;

namespace kcp2k
{
    // struct to avoid memory indirection
    public struct KcpServerConnection
    {
        // peer can't be set from constructor at the moment.
        // because peer callbacks need to know 'connection'.
        // see KcpServer.CreateConnection.
        public KcpPeer peer;
        public readonly IEndPoint remoteEndPoint;

        public KcpServerConnection(IEndPoint remoteEndPoint)
        {
            peer = null;
            this.remoteEndPoint = remoteEndPoint;
        }
    }
}
