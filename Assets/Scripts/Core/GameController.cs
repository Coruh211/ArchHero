using System;

namespace Core
{
    public class GameController : Singleton<GameController>
    {
        public Action OnGameStarted;
    }
}