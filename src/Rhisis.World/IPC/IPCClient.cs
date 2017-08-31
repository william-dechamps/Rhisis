﻿using Ether.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IPC.Packets;

namespace Rhisis.World.IPC
{
    public sealed class IPCClient : NetClient
    {
        private WorldConfiguration _worldConfiguration;

        public IPCClient(WorldConfiguration worldConfiguration) 
            : base(worldConfiguration.IPC.Host, worldConfiguration.IPC.Port, 1024)
        {
            this._worldConfiguration = worldConfiguration;
        }

        protected override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<IPCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warning("Unknown inter-server packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }

        protected override void OnConnected()
        {
        }

        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from Inter-Server.");
        }

        [PacketHandler(InterPacketType.WELCOME)]
        public void OnWelcome(NetPacketBase packet)
        {
            IPCPackets.SendAuthentication(this, this._worldConfiguration);
        }

        [PacketHandler(InterPacketType.AUTHENTICATION_RESULT)]
        public void OnAuthenticationResult(NetPacketBase packet)
        {
            var authenticationResult = packet.Read<uint>();

            Logger.Debug("Authentication result: {0}", (InterServerError)authenticationResult);
        }
    }
}
