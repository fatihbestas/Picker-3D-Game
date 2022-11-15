using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject gameCompleteScreen;
    private bool gamePaused;
    [SerializeField] TextMeshProUGUI currentLevelTxt;
    [SerializeField] TextMeshProUGUI nextLevelTxt;

    [SerializeField] GameObject[] emptyLights;
    [SerializeField] GameObject[] greenLights;
    [SerializeField] GameObject[] yellowLights;
    [SerializeField] GameObject[] redLights;

    Dictionary<int, GameObject[]> lights = new Dictionary<int, GameObject[]>();

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

        // tüm light objelerini bir sözlükte tut.
        lights[0] = emptyLights;
        lights[1] = greenLights;
        lights[2] = yellowLights;
        lights[3] = redLights;

        UpdateLevelTxt();

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

            if(passedStageCount == 1)
            {
                emptyLights[0].SetActive(false);
                greenLights[0].SetActive(true);
                Invoke("MovePickerToNextStage", 1.5f);
            }
            else if(passedStageCount == 2)
            {
                emptyLights[1].SetActive(false);
                greenLights[1].SetActive(true);
                Invoke("MovePickerToNextStage", 1.5f);
            }
            else if(passedStageCount == 3)
            {
                emptyLights[2].SetActive(false);
                greenLights[2].SetActive(true);
                passedStageCount = 0;
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

    void MovePickerToNextStage()
    {
        Picker.Instance.Move();
    }

    public void LevelFailed()
    {
        if(passedStageCount == 0)
        {
            emptyLights[0].SetActive(false);
            redLights[0].SetActive(true);
        }
        else if(passedStageCount == 1)
        {
            emptyLights[1].SetActive(false);
            redLights[1].SetActive(true);
        }
        else if(passedStageCount == 2)
        {
            emptyLights[2].SetActive(false);
            redLights[2].SetActive(true);
        }

        Invoke("LevelFailedScreen", 0.5f);
    }

    void LevelFailedScreen()
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
        // level yoksa GameComplete ekranını aç.
        if(levels.Length > currentLevel)
        {
            currentLevel += 1;
            DataPersistenceManager.instance.SaveGame();
            Invoke("MovePickerToNextLevel", 1.5f);
        }
        else
        {
            OpenGameCompleteScreen();
        }
    }

    void MovePickerToNextLevel()
    {
        Picker.Instance.GoToNextLevel();
    }

    public void OpenLevelCompleteScreen()
    {
        playScreen.SetActive(false);
        levelCompleteScreen.SetActive(true);
    }

    public void Continue()
    {
        levelCompleteScreen.SetActive(false);
        playScreen.SetActive(true);
        Picker.Instance.Move();
        UpdateLevelTxt();
    }

    void OpenGameCompleteScreen()
    {
        playScreen.SetActive(false);
        gameCompleteScreen.SetActive(true);
    }

    void UpdateLevelTxt()
    {
        SetLightsToInitialState();

        currentLevelTxt.text = currentLevel.ToString();

        if(levels.Length > currentLevel)
        {
            nextLevelTxt.text = (currentLevel + 1).ToString();
        }
        else
        {
            nextLevelTxt.text = "?";
        }
    }

    void SetLightsToInitialState()
    {
        // tüm ışık objelerini kapat.
        for (int i = 0; i < lights.Count; i++)
        {
            for (int i2 = 0; i2 < lights[i].Length; i2++)
            {
                lights[i][i2].SetActive(false);
            }
        }

        // boş ışıkları aç.
        emptyLights[0].SetActive(true);
        emptyLights[1].SetActive(true);
        emptyLights[2].SetActive(true);
    }
}
