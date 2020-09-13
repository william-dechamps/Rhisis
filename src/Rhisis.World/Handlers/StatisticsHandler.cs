﻿using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.World.Systems.Statistics;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class StatisticsHandler
    {
        private readonly IStatisticsSystem _statisticsSystem;

        /// <summary>
        /// Creates a new <see cref="StatisticsHandler"/> instance.
        /// </summary>
        /// <param name="statisticsSystem">Statistics system.</param>
        public StatisticsHandler(IStatisticsSystem statisticsSystem)
        {
            _statisticsSystem = statisticsSystem;
        }

        /// <summary>
        /// Handles the MODIFY_STATUS for updating a player's statistics.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.MODIFY_STATUS)]
        public void OnModifyStatus(IPlayer player, ModifyStatusPacket packet)
        {
            player.Statistics.UpdateStatistics(packet.Strength, packet.Stamina, packet.Dexterity, packet.Intelligence);

            // TODO: send packet ?

            //_statisticsSystem.UpdateStatistics(serverClient.Player, 
            //    packet.Strength, packet.Stamina, packet.Dexterity, packet.Intelligence);
        }
    }
}
