using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartieManager : Manager
{
    public MapManager map;
    public List<Player> players;
    public Player playerPrefab;

    private void Awake()
    {
        manager = this;
    }

    public void MakePlayers(List<PlayerSpawner> spawners)
    {
        foreach(PlayerSpawner spawner in spawners)
        {
            Player player = Instantiate(playerPrefab, transform);
            player.Apply(spawner);
            players.Add(player);
        }
    }
}
