using System.Collections.Generic;
using LudoGameEngine;

namespace CoreWebAPI.Logic
{
    public interface IGamesContainer
    {
        void CreateNewLudoGame();
        IEnumerable<string> GetAllGames();
        LudoGame GetGame(string id);
        bool DeleteGameSession(string id);

    }
}