﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.IO;
using Rhisis.Game.IO.Dyo;
using Rhisis.Game.IO.Rgn;
using Rhisis.Game.IO.World;
using Rhisis.Game.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Map
{
    public class MapManager : IMapManager
    {
        private readonly ILogger<MapManager> _logger;
        private readonly IGameResources _gameResources;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IDictionary<int, IMap> _maps;

        public MapManager(ILogger<MapManager> logger, IOptions<WorldConfiguration> worldConfiguration, IGameResources gameResources)
        {
            _logger = logger;
            _gameResources = gameResources;
            _worldConfiguration = worldConfiguration.Value;
            _maps = new ConcurrentDictionary<int, IMap>();
        }

        public IMap GetMap(int mapId)
        {
            return _maps.TryGetValue(mapId, out IMap map) ? map : throw new KeyNotFoundException($"Cannot find map with id: {mapId}");
        }

        public void Load()
        {
            var worldScriptPath = GameResourcesConstants.Paths.WorldScriptPath;
            var worldsPaths = new Dictionary<string, string>();

            using (var textFile = new TextFile(worldScriptPath))
            {
                foreach (var text in textFile.Texts)
                {
                    worldsPaths.Add(text.Key, text.Value.Replace('"', ' ').Trim());
                }
            }

            foreach (var mapDefineName in _worldConfiguration.Maps)
            {
                if (!worldsPaths.TryGetValue(mapDefineName, out var mapName))
                {
                    _logger.LogWarning(GameResourcesConstants.Errors.UnableLoadMapMessage, mapDefineName, $"map is not declared inside '{worldScriptPath}' file");
                    continue;
                }

                if (!_gameResources.Defines.TryGetValue(mapDefineName, out var mapId))
                {
                    _logger.LogWarning(GameResourcesConstants.Errors.UnableLoadMapMessage, mapDefineName, $"map has no define id inside '{GameResourcesConstants.Paths.DataSub0Path}/defineWorld.h' file");
                    continue;
                }

                if (_maps.ContainsKey(mapId))
                {
                    _logger.LogWarning(GameResourcesConstants.Errors.UnableLoadMapMessage, mapDefineName, $"another map with id '{mapId}' already exist.");
                    continue;
                }

                WldFileInformations worldInformation = LoadWorldInformation(mapName);

                var map = new Map(mapId, mapName, worldInformation);

                LoadObjects(map);
                LoadRegions(map);
                LoadHeightMap(map);
                map.GenerateNewLayer();

                _maps.Add(mapId, map);
            }

            _logger.LogInformation("-> {0} maps loaded.", _maps.Count);
        }

        private WldFileInformations LoadWorldInformation(string mapName)
        {
            var wldFilePath = Path.Combine(GameResourcesConstants.Paths.MapsPath, mapName, $"{mapName}.wld");
            using var wldFile = new WldFile(wldFilePath);

            return wldFile.WorldInformations;
        }

        private void LoadObjects(IMap map)
        {
            var dyo = Path.Combine(GameResourcesConstants.Paths.MapsPath, map.Name, $"{map.Name}.dyo");
            using var dyoFile = new DyoFile(dyo);
            IEnumerable<NpcDyoElement> npcElements = dyoFile.GetElements<NpcDyoElement>();

            foreach (NpcDyoElement element in npcElements)
            {
                // TODO: create NPC on map
                //map.AddEntity(_npcFactory.CreateNpc(map, element));
            }
        }

        private void LoadRegions(Map map)
        {
            var regions = new List<IMapRegion>();
            var rgn = Path.Combine(GameResourcesConstants.Paths.MapsPath, map.Name, $"{map.Name}.rgn");

            using (var rgnFile = new RgnFile(rgn))
            {
                IEnumerable<IMapRespawnRegion> respawnersRgn = rgnFile.GetElements<RgnRespawn7>()
                    .Select(x => MapRespawnRegion.FromRgnElement(x));

                regions.AddRange(respawnersRgn);

                foreach (RgnRegion3 region in rgnFile.GetElements<RgnRegion3>())
                {
                    IMapRegion mapRegion = (RegionInfoType)region.Index switch
                    {
                        RegionInfoType.Revival => MapRevivalRegion.FromRgnElement(region, map.RevivalMapId),
                        RegionInfoType.Trigger => MapTriggerRegion.FromRgnElement(region),
                        // TODO: load collector regions
                        _ => null
                    };

                    if (region != null)
                    {
                        regions.Add(mapRegion);
                    }
                }
            }

            map.SetRegions(regions);
        }

        private void LoadHeightMap(Map map)
        {
            // TODO: load lnd files
        }
    }
}
