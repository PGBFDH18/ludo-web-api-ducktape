using LudoGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreWebAPI.Logic
{
    public class GamesContainer : IGamesContainer
    {
        private Dictionary<string, LudoGame> ludoGames;

        public GamesContainer()
        {
            ludoGames = new Dictionary<string, LudoGame>();
        }

        public LudoGame GetGame(string id)
        {
            if (!ludoGames.ContainsKey(id))
            {
                ludoGames.Add(id, new LudoGame(new Diece()));
            }

            return ludoGames[id];
        }

        public void CreateNewLudoGame()
        {
            var game = new LudoGame(new Diece());
            ludoGames.Add(Guid.NewGuid().ToString(), game);
        }
        public bool DeleteGameSession(string id)
        {
            return ludoGames.Remove(id);
        }

        public IEnumerable<string> GetAllGames()
        {
            return ludoGames.Select(d => d.Key).ToList();
        }
    }
}
