﻿using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class KeepAlivePacket : IPacketDeserializer
    {
        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            throw new System.NotImplementedException();
        }
    }
}