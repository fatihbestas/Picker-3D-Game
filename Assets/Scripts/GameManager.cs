using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>, IDataPersistence
{
    // levelin numarasıyşa dizideki index'i arasında uyuşmazlık olmaması için
    // bu diziye başka scriptlerden direk erişilmesini istemiyorum.
    // erişim için GetLevelGO fonksiyonu kullanılacak.
    [SerializeField] private GameObject[] levels;
    [System.NonSerialized] public int currentLevel;
    [System.NonSerialized] public bool stagePassed;
    private int passedStageCount;
    public GameObject startScreen;
    public GameObject playScreen;
    public GameObject levelFailedScreen;
    public GameObject levelCompleteScreen;
    private bool gamePaused;

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
        // başlangıçta oyun kullanıcıdan input gelmesini bekliyor.
        gamePaused = true;
        Time.timeScale = 0;

        stagePassed = false;
        passedStageCount = 0;
        
    }

    
    void Update()
    {
        if(gamePaused && Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            StartGame();
        }

        // for editor debug 
        if(gamePaused && Input.GetKey(KeyCode.A))
        {
            StartGame();
        }


        if(stagePassed)
        {

            passedStageCount += 1;
            if(passedStageCount == 3)
            {
                Victory();
            }
        }
    }

    void StartGame()
    {
        gamePaused = false;
        startScreen.SetActive(false);
        playScreen.SetActive(true);
        Time.timeScale = 1;
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
