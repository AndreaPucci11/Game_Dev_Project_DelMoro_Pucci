using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static int loadedCoins;
    public static int loadedGems;
    public static int loadedRecord;
    public static bool saveData;
    [SerializeField] int internalCoin;
    [SerializeField] int internalGem;
    [SerializeField] int internalRecord;

    void Start()
    {
        loadedCoins = PlayerPrefs.GetInt("COINSAVE");
        loadedGems = PlayerPrefs.GetInt("GEMSAVE");
        loadedRecord = PlayerPrefs.GetInt("RECORDSAVE");
        internalRecord = loadedRecord;
    }

    
    void Update()
    {
        internalCoin = loadedCoins + MasterInfo.coinCount;
        internalGem = loadedGems + MasterInfo.gemCount;
        if(MasterInfo.internalDistance > internalRecord)
        {
            internalRecord = MasterInfo.internalDistance;
        }
        if (saveData == true)
        {
            saveData = false;
            PlayerPrefs.SetInt("COINSAVE", internalCoin);
            PlayerPrefs.SetInt("GEMSAVE", internalGem);
            PlayerPrefs.SetInt("RECORDSAVE", internalRecord);
            internalRecord = loadedRecord;
        }

        }
}
