using gomoku.Game.Positioning;
using gomoku.JobsAPI.Entities;
using gomoku.JobsAPI.Entities.Game;
using gomoku.JobsAPI.Entities.Responses;
using gomoku.JobsAPI.Entities.Responses.Subs;

namespace gomoku.JobsAPI
{
    class Jobs
    {
        internal static GameInfo CreateGame()
        {
            ResponseEntity response = Connector.Instance.Post(new Connect());
                    
            if (response is GameInfo)
            {
                return (GameInfo)response;
            }
            else
            {
                return null;
            }

        }

        internal static GameStatus GameStatus(GameInfo game)
        {
            ResponseEntity response = Connector.Instance.Post(new CheckStatus() { gameToken = game.gameToken });

            if (response is GameStatus)
            {
                return (GameStatus)response;
            }
            else
            {
                return null;
            }
        }



        internal static string UserId()
        {
            return Connector.Instance.UserId();
        }

        internal static GameStatus Play(GameLoc loc, GameInfo game)
        {
            Move m = loc.ToMove();

            ResponseEntity response = Connector.Instance.Post(new Play() { gameToken = game.gameToken, positionX = m.x, positionY = m.y });

            if (response is GameStatus)
            {
                return (GameStatus)response;
            }
            else
            {
                return null;
            }
        }
    }
}
