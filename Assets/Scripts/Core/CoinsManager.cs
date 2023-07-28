using System;


public class CoinsManager : Singleton<CoinsManager>
{
    public float CoinsCount;
    public Action<float> UpdateCount;

    private void Start()
    {
        UpdateCount?.Invoke(CoinsCount);
    }
    
    public bool TryRemoveMoney(float count)
    {
        if(CoinsCount >= count)
        {
            CoinsCount -= count;
            UpdateCount?.Invoke(CoinsCount);
            return true;
        }
        return false;
    }

    public void AddCoins(float count)
    {
        CoinsCount += count;
        UpdateCount?.Invoke(CoinsCount);
    }
    
}