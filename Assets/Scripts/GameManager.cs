using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(gamePaused && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            StartGame();
        }


        if(stagePassed)
        {
            passedStageCount += 1;
            stagePassed = false;

            if(passedStageCount == 3)
            {
                passedStageCount = 0;
                Victory();
            }
            else
            {
                Invoke("MovePickerToNextStage", 1.5f);
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

    void MovePickerToNextStage()
    {
        Picker.Instance.GoToNextStage();
    }

    public void LevelFailed()
    {
        Time.timeScale = 0;
        playScreen.SetActive(false);
        levelFailedScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            Invoke("MovePickerToNextLevel", 1.5f);
        }
        else
        {

        }
    }

    void MovePickerToNextLevel()
    {
        Picker.Instance.GoToNextLevel();
    }
}
