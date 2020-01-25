﻿using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyChangeItemModePacket"/> structure.
    /// </summary>
    public struct PartyChangeItemModePacket : IEquatable<PartyChangeItemModePacket>
    {

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the item mode.
        /// </summary>
        public int ItemMode { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyChangeItemModePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyChangeItemModePacket(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ItemMode = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="PartyChangeItemModePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyChangeItemModePacket"/></param>
        public bool Equals(PartyChangeItemModePacket other)
        {
            return PlayerId == other.PlayerId && ItemMode == other.ItemMode;
        }
    }
}