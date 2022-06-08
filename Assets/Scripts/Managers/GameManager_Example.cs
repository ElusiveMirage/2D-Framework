using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager_Example : MonoBehaviour
{
    public PlayerData_Example playerData;
    //=============================================//
    public bool gamePaused;
    public bool gameOver;
    public bool playerWins;
    public bool playerDied;
    //=============================================//
    [Header("UI Elements")]
    [SerializeField] private UI_Bar playerHPBar;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI maxHPText;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    //=============================================//
    public NodeGrid2D levelGrid;
    //==================================================//
    private static GameManager_Example _instance;

    public static GameManager_Example Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePaused || gameOver || playerWins)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
      
        if (!gamePaused)
        {
            UpdateUI();
        }

        if (playerData.HP <= 0 && !playerDied)
        {
            Invoke("GameOver", 2.5f);
            playerDied = true;
        }
    }

    private void GameStart()
    {

    }

    private void UpdateUI() //really ought to move this into a separate class
    {
        //Player stats
        playerHPBar.SetMaxValue(playerData.maxHP);
        playerHPBar.SetMinValue(playerData.HP);
        HPText.text = playerData.HP.ToString();
        maxHPText.text = playerData.maxHP.ToString();
    }

    public void PauseGame()
    {
        if (gamePaused)
        {
            SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Resume"));
        }
        else
        {
            SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Pause"));
        }

        gamePaused = !gamePaused;
        pauseScreen.SetActive(gamePaused);
    }

    public void Return()
    {
        SoundManager.Instance.StopBGM();
        Destroy(gameObject.transform.parent.gameObject);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("MainMenuScene");
    }

    public void Restart()
    {
        Destroy(gameObject.transform.parent.gameObject);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        SoundManager.Instance.StopBGM();
        gameOver = true;
        gameOverScreen.SetActive(true);
    }
}
