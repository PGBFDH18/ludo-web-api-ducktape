using CoreWebAPI.Logic;
using LudoGameEngine;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LudoController : ControllerBase
    {
        private readonly ILudoGame _LudoGame;
        private readonly IGamesContainer _games;
        public LudoController(ILudoGame ludoGame, IGamesContainer games)
        {
            _games = games;
            _LudoGame = ludoGame;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _games.GetAllGames();
        }

        [HttpGet("{guid}", Name = "Get")]
        public IEnumerable<Piece> Get(string guid)
        {
            return _games.GetGame(guid).GetAllPiecesInGame();
        }

        [HttpPost("{guid}")]
        public IEnumerable<Player> Post(string guid)
        {
            var players = new List<Player>()
            {
            _games.GetGame(guid).AddPlayer(Guid.NewGuid().ToString(), PlayerColor.Green),
            _games.GetGame(guid).AddPlayer(Guid.NewGuid().ToString(), PlayerColor.Red)
            };

            return players;
        }

        [HttpGet("{gameId}/players")]
        public IEnumerable<Player> ListOfPlayers(string gameId)
        {
            return _games.GetGame(gameId).GetPlayers();
        }

        [HttpPost("{gameId}/players")]
        public IEnumerable<Player> AddNewPlayers(string gameId, PlayerColor color)
        {
            _games.GetGame(gameId).AddPlayer(Guid.NewGuid().ToString(), color);
            return _games.GetGame(gameId).GetPlayers();
        }

        [HttpGet("{gameId}/players/{playerId}")]
        public Player GetPlayerDetails(string gameId, int playerId)
        {
            return _games.GetGame(gameId).GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
        }

        [HttpPut("{gameId}/players/{playerId}")]
        public Player UpdatePlayer(string gameId, int playerId, string name, PlayerColor color)
        {
            var player = _games.GetGame(gameId).GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
            player.Name = name;
            player.PlayerColor = color;
            return player;
        }

        [HttpDelete("{gameId}/players/{playerId}")]
        public bool DeletePlayer(string gameId, int playerId)
        {
            var player = _games.GetGame(gameId).GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
            return _games.GetGame(gameId).GetPlayers().Remove(player);
        }
        [HttpDelete("{gameId}")]
        public bool DeleteGameSession(string gameId)
        {
            return _games.DeleteGameSession(gameId);
        }

        [HttpPut("{gameId}")]
        public Player ChangePlayerPiece(string gameId, int playerId, int pieceId, int numberOfFields)
        {
            var game = _games.GetGame(gameId);
            var gamestate = game.GetGameState();
            if ( gamestate != GameState.Started)
            {
                game.StartGame();
            }else if(gamestate != GameState.Ended)
            {
                var player = game.GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
                game.MovePiece(player, pieceId, numberOfFields);
            }
            
            var winner = game.GetWinner();
            if (winner is null)
            {
                return null;
            }
            
            return winner;
        }

    }
}
