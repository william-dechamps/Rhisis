﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Packets.Login.Server;

public class ServerListPacket : FFPacket
{
    public ServerListPacket(string username, IEnumerable<Cluster> clusters)
        : base(PacketType.SRVR_LIST)
    {
        WriteInt32(0);
        WriteByte(1);
        WriteString(username);
        WriteInt32(clusters.Sum(x => x.Channels.Count) + clusters.Count());

        foreach (Cluster cluster in clusters)
        {
            WriteInt32(-1);
            WriteInt32(cluster.Id);
            WriteString(cluster.Name);
            WriteString(cluster.Ip);
            WriteInt32(0);
            WriteInt32(0);
            WriteInt32(Convert.ToInt32(cluster.IsEnabled));
            WriteInt32(0);

            foreach (WorldChannel world in cluster.Channels)
            {
                WriteInt32(cluster.Id);
                WriteInt32(world.Id);
                WriteString(world.Name);
                WriteString(world.Ip);
                WriteInt32(0);
                WriteInt32(world.ConnectedUsers);
                WriteInt32(Convert.ToInt32(world.IsEnabled));
                WriteInt32(world.MaximumUsers);
            }
        }
    }
}