﻿using Ether.Network;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendPlayerSpawn(NetConnection client, IEntity player)
        {
            // Retrieve the component from the entity
            var objectComponent = player.GetComponent<ObjectComponent>();
            var humanComponent = player.GetComponent<HumanComponent>();
            var playerComponent = player.GetComponent<PlayerComponent>();

            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(objectComponent.ObjectId, SnapshotType.ENVIRONMENTALL, 0x0000FF00);
                packet.Write(0); // Get weather by season

                packet.StartNewMergedPacket(objectComponent.ObjectId, SnapshotType.WORLD_READINFO);
                packet.Write(objectComponent.MapId);
                packet.Write(objectComponent.Position.X);
                packet.Write(objectComponent.Position.Y);
                packet.Write(objectComponent.Position.Z);

                packet.StartNewMergedPacket(objectComponent.ObjectId, SnapshotType.ADD_OBJ);

                // Common object properties
                packet.Write((byte)objectComponent.Type);
                packet.Write(objectComponent.ModelId);
                packet.Write((byte)objectComponent.Type);
                packet.Write(objectComponent.ModelId);
                packet.Write(objectComponent.Size);
                packet.Write(objectComponent.Position.X);
                packet.Write(objectComponent.Position.Y);
                packet.Write(objectComponent.Position.Z);
                packet.Write((short)(objectComponent.Angle * 10));
                packet.Write(objectComponent.ObjectId);

                packet.Write<short>(0); // m_dwMotion
                packet.Write<byte>(1); // m_bPlayer
                packet.Write(230); // HP
                packet.Write(0); // moving flags
                packet.Write(0); // motion flags
                packet.Write<byte>(1); // m_dwBelligerence

                packet.Write(-1); // m_dwMoverSfxId

                packet.Write(objectComponent.Name);
                packet.Write(humanComponent.Gender);
                packet.Write((byte)humanComponent.SkinSetId);
                packet.Write((byte)humanComponent.HairId);
                packet.Write((int)humanComponent.HairColor);
                packet.Write((byte)humanComponent.FaceId);
                packet.Write(playerComponent.Id);
                packet.Write((byte)1); // Job
                packet.Write((short)15); // STR
                packet.Write((short)15); // STA
                packet.Write((short)15); // DEX
                packet.Write((short)15); // INT
                packet.Write((short)1); // Levels
                packet.Write(-1); // Fuel
                packet.Write(0); // Actuel fuel

                // Guilds

                packet.Write<byte>(0); // have guild or not
                packet.Write(0); // guild cloak

                // Party

                packet.Write<byte>(0); // have party or not

                packet.Write((byte)100); // authority
                packet.Write(0); // mode
                packet.Write(0); // state mode
                packet.Write(0x000001F6); // item used ??
                packet.Write(0); // last pk time.
                packet.Write(0); // karma
                packet.Write(0); // pk propensity
                packet.Write(0); // pk exp
                packet.Write(0); // fame
                packet.Write<byte>(0); // duel
                packet.Write(-1); // titles

                for (int i = 0; i < 31; i++)
                {
                    packet.Write(0);
                }
                //foreach (var item in this.Inventory.GetEquipedItems())
                //{
                //    if (item == null || item.Id < 0)
                //        packet.Write(0);
                //    else
                //    {
                //        packet.Write(item.Refine); // Refine
                //        packet.Write<byte>(0);
                //        packet.Write(item.Element); // element (fire, water, elec...)
                //        packet.Write(item.ElementRefine); // Refine element
                //    }
                //}

                packet.Write(0); // guild war state

                for (int i = 0; i < 26; ++i)
                    packet.Write(0);

                packet.Write((short)0); // MP
                packet.Write((short)0); // FP
                packet.Write(0); // tutorial state
                packet.Write(0); // fly experience
                packet.Write(0); // Gold
                packet.Write((long)0); // exp
                packet.Write(0); // skill level
                packet.Write(0); // skill points
                packet.Write<long>(0); // death exp
                packet.Write(0); // death level

                for (int i = 0; i < 32; ++i)
                    packet.Write(0); // job in each level

                packet.Write(0); // marking world id
                packet.Write(objectComponent.Position.X);
                packet.Write(objectComponent.Position.Y);
                packet.Write(objectComponent.Position.Z);

                // Quests
                packet.Write<byte>(0);
                packet.Write<byte>(0);
                packet.Write<byte>(0);

                packet.Write(42); // murderer id
                packet.Write<short>((short)0); // stat points
                packet.Write<short>(0); // always 0

                // item mask
                for (int i = 0; i < 31; i++)
                    packet.Write(0);

                // skills
                for (int i = 0; i < 45; ++i)
                {
                    packet.Write(-1); // skill id
                    packet.Write(0); // skill level
                }

                packet.Write<byte>(0); // cheer point
                packet.Write(0); // next cheer point ?

                // Bank
                packet.Write((byte)playerComponent.Slot);
                for (int i = 0; i < 3; ++i)
                    packet.Write(0); // gold
                for (int i = 0; i < 3; ++i)
                    packet.Write(0); // player bank ?

                packet.Write(1); //ar << m_nPlusMaxHitPoint;
                packet.Write<byte>(0);  //ar << m_nAttackResistLeft;				
                packet.Write<byte>(0);  //ar << m_nAttackResistRight;				
                packet.Write<byte>(0);  //ar << m_nDefenseResist;
                packet.Write<long>(0); //ar << m_nAngelExp;
                packet.Write(0); //ar << m_nAngelLevel;

                // Inventory
                //this.Inventory.Serialize(packet);
                for (int i = 0; i < 73; ++i)
                    packet.Write(i);
                packet.Write(0);
                for (int i = 0; i < 73; ++i)
                    packet.Write(i);

                // Bank

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 0x2A; ++j)
                        packet.Write(j);
                    packet.Write<byte>(0); // count
                    for (int j = 0; j < 0x2A; ++j)
                        packet.Write(j);
                }

                packet.Write(int.MaxValue); // pet id

                // Bag

                packet.Write<byte>(1);
                for (int i = 0; i < 6; i++)
                {
                    packet.Write(i);
                }
                packet.Write<byte>(0);                 // Base bag item count
                for (int i = 0; i < 0; i++)
                {
                    packet.Write((byte)i);             // Slot
                    packet.Write(i);            // Slot
                }
                for (int i = 0; i < 6; i++)
                {
                    packet.Write(i);
                }
                packet.Write(0);
                packet.Write(0);

                // premium bag
                packet.Write<byte>(0);

                packet.Write(0); // muted

                // Honor titles
                for (int i = 0; i < 150; ++i)
                    packet.Write(0);

                packet.Write(0); // id campus
                packet.Write(0); // campus points

                // buffs
                packet.Write(0); // count

                client.Send(packet);
            }
        }
    }
}
