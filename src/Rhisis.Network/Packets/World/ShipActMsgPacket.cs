﻿using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ShipActMsgPacket"/> structure.
    /// </summary>
    public struct ShipActMsgPacket : IEquatable<ShipActMsgPacket>
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        public uint Message { get; set; }

        /// <summary>
        /// Gets the first parameter.
        /// </summary>
        public int Parameter1 { get; set; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; set; }

        /// <summary>
        /// Gets the ship.
        /// </summary>
        public uint Ship { get; set; }

        /// <summary>
        /// Creates a new <see cref="ShipActMsgPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ShipActMsgPacket(INetPacketStream packet)
        {
            Message = packet.Read<uint>();
            Parameter1 = packet.Read<int>();
            Parameter2 = packet.Read<int>();
            Ship = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="ShipActMsgPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ShipActMsgPacket"/></param>
        public bool Equals(ShipActMsgPacket other)
        {
            return Message == other.Message && Parameter1 == other.Parameter1 && Parameter2 == other.Parameter2 && Ship == other.Ship;
        }
    }
}