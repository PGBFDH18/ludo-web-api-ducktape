using System.Collections.Generic;

namespace LudoGameEngine
{
    public interface ILudoGame
    {
        bool StartGame();
        Player AddPlayer(string name, PlayerColor color);
        List<Player> GetPlayers();
        GameState GetGameState();
        //void StartTurn(Player player);

        int RollDiece();

        void MovePiece(Player player, int pieceId, int numberOfFields);
        void EndTurn(Player player);
        Player GetCurrentPlayer();
        Piece[] GetAllPiecesInGame();
        Player ChangePlayerPiece(LudoGame game, string gameId, int playerId, int pieceId, int numberOfFields);
        Player GetWinner();
    }
}