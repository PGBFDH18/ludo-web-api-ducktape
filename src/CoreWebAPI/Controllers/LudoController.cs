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
        /// <summary>
        /// List all games
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IEnumerable<string> Get()
        {
            return _games.GetAllGames();
        }
        /// <summary>
        /// Get all pieces in game
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        // GET: api/ludo/{id}
        [HttpGet("{guid}"/*, Name = "Get"*/)]
        public IEnumerable<Piece> Get(string guid)
        {
            return _games.GetGame(guid).GetAllPiecesInGame();
        }
        /// <summary>
        /// 1#  Add two players and create game if not exsisting
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// List all players
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}/players")]
        public IEnumerable<Player> ListOfPlayers(string gameId)
        {
            return _games.GetGame(gameId).GetPlayers();
        }
        /// <summary>
        /// List all players
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpPost("{gameId}/players")]
        public IEnumerable<Player> AddNewPlayers(string gameId, PlayerColor color)
        {
            _games.GetGame(gameId).AddPlayer(Guid.NewGuid().ToString(), color);
            return _games.GetGame(gameId).GetPlayers();
        }
        /// <summary>
        /// Get player details
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}/players/{playerId}")]
        public Player GetPlayerDetails(string gameId, int playerId)
        {
            return _games.GetGame(gameId).GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
        }
        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpPut("{gameId}/players/{playerId}")]
        public Player UpdatePlayer(string gameId, int playerId, string name, PlayerColor color)
        {
            var player = _games.GetGame(gameId).GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
            player.Name = name;
            player.PlayerColor = color;
            return player;
        }
        /// <summary>
        /// Delete player
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpDelete("{gameId}/players/{playerId}")]
        public bool DeletePlayer(string gameId, int playerId)
        {
            var player = _games.GetGame(gameId).GetPlayers().FirstOrDefault(x => x.PlayerId == playerId);
            return _games.GetGame(gameId).GetPlayers().Remove(player);
        }

        /// <summary>
        /// Delete game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpDelete("{gameId}")]
        public bool DeleteGameSession(string gameId)
        {
            return _games.DeleteGameSession(gameId);
        }
        /// <summary>
        /// Use to move piece
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <param name="pieceId"></param>
        /// <param name="numberOfFields"></param>
        /// <response code="200">Returns the winner if anyone has wone</response>
        /// <response code="204">Returns null if no winner</response>
        /// <returns>Returns winner of game, returns null if no winner</returns>
        [HttpPut("{gameId}")]
        [ProducesResponseType(typeof(Player), 200)]
        [ProducesResponseType(typeof(Player), 204)]
        public Player ChangePlayerPiece(string gameId, int playerId, int pieceId, int numberOfFields)
        {
            var game = _games.GetGame(gameId);
            return _LudoGame.ChangePlayerPiece(game, gameId, playerId, pieceId, numberOfFields);
        }

    }
}
