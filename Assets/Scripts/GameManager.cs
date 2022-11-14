using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>, IDataPersistence
{
    // levelin numarasıyşa dizideki index'i arasında uyuşmazlık olmaması için
    // bu diziye başka scriptlerden direk erişilmesini istemiyorum.
    // erişim için GetLevelGO fonksiyonu kullanılacak.
    [SerializeField] private GameObject[] levels;
    public int currentLevel;
    public bool stagePassed;
    private int passedStageCount;

    public void LoadData(PlayerData data) 
    {
        // kaçıncı level'de olduğumuzu kayıtlı verilerden oku.
        currentLevel = data.currentLevel;
    }

    public void SaveData(PlayerData data) 
    {
        // kaçıncı levelde olduğumuzu dosyaya kaydet.
        data.currentLevel = currentLevel;
    }

    void Start()
    {
        stagePassed = false;
        passedStageCount = 0;
        
    }

    
    void Update()
    {
         if(stagePassed)
         {

            passedStageCount += 1;
            if(passedStageCount == 3)
            {
                Victory();
            }
         }
    }

    public GameObject GetLevelGO(int levelNumber)
    {
        // istenen levelin gameobjesini döndür.
        // diziye index olarak levelin numarasının bir eksiği gönderilmeli.
        return levels[levelNumber - 1];
    }

    void Victory()
    {
        // level başarıyla geçildi.
        // Eğer oynadığımız levelin ilerisinde başka leveller varsa
        // currentLevel değişkenini 1 arttırıp oyunu kaydet. Eğer başka 
        // level yoksa " tebrikler oyunu bitirdiniz" ekranını aç.
        if(levels.Length > currentLevel)
        {
            currentLevel += 1;
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            
        }
    }
}
