using System;

namespace Core.Tools
{
    public class Ticker: Singleton<Ticker>
    {
        public Action UpdateTick;

        public void Update()
        {
            UpdateTick?.Invoke();
        }
    }
}