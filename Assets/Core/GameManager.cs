using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Round Settings")]
    public int maxRounds = 10;
    public float preparationTime = 15f;

    [Header("References")]
    public EconomyManager economyManager;
    public BaseHealth baseHealth;
    public EnemySpawner enemySpawner;
    public WaveBuilderAI waveBuilderAI;
    public UIHUD ui;
    public AudioSource gameMusic;
    public GameObject startPanel;

    [Header("Music UI")]
    public TMP_Text musicText;

    public GameState CurrentState { get; private set; } = GameState.Menu;
    public int CurrentRound { get; private set; } = 0;

    private bool roundResolved;
    private bool isMusicOn = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Menu;

        if (startPanel != null)
            startPanel.SetActive(true);

        if (ui != null)
            ui.SetStateLabel("Menu");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        CurrentRound = 0;
        roundResolved = false;

        if (startPanel != null)
            startPanel.SetActive(false);

        baseHealth.ResetHealth();
        economyManager.ResetEconomy();

        if (gameMusic != null)
        {
            gameMusic.Stop();
            gameMusic.loop = true;

            if (isMusicOn)
                gameMusic.Play();
        }

        UpdateMusicText();

        StopAllCoroutines();
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (CurrentState != GameState.GameOver)
        {
            CurrentRound++;

            if (CurrentRound > maxRounds)
            {
                Victory();
                yield break;
            }

            Time.timeScale = 1f;
            SetState(GameState.Preparation);
            ui?.ShowPreparation(CurrentRound, preparationTime);

            float timer = preparationTime;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                ui?.UpdatePreparationTimer(timer);
                yield return null;
            }

            Time.timeScale = 1f;
            SetState(GameState.Battle);

            var wave = waveBuilderAI.BuildWave(CurrentRound);
            yield return StartCoroutine(enemySpawner.SpawnWave(wave));

            while (enemySpawner.HasAliveEnemies())
                yield return null;

            Time.timeScale = 1f;
            SetState(GameState.RoundEnd);
            economyManager.AdvanceRoundBudget(CurrentRound);
            ui?.Refresh();
            yield return new WaitForSeconds(2f);

            if (baseHealth.CurrentHP <= 0)
            {
                Defeat();
                yield break;
            }
        }
    }

    public void NotifyEnemyKilled(int reward)
    {
        economyManager.AddGold(reward);
        ui?.Refresh();
    }

    public void NotifyBaseDamaged()
    {
        ui?.Refresh();

        if (baseHealth.CurrentHP <= 0 && !roundResolved)
        {
            roundResolved = true;
            Defeat();
        }
    }

    private void SetState(GameState state)
    {
        CurrentState = state;
        ui?.SetStateLabel(state.ToString());
    }

    public bool CanBuild()
    {
        return CurrentState == GameState.Preparation;
    }

    public void PauseBattle()
    {
        if (CurrentState != GameState.Battle) return;

        Time.timeScale = 0f;

        if (gameMusic != null && gameMusic.isPlaying)
            gameMusic.Pause();
    }

    public void PlayBattle()
    {
        if (CurrentState != GameState.Battle) return;

        Time.timeScale = 1f;

        if (gameMusic != null && isMusicOn)
            gameMusic.UnPause();
    }

    public void FastBattle()
    {
        if (CurrentState != GameState.Battle) return;

        if (Time.timeScale == 2f)
            Time.timeScale = 1f;
        else
            Time.timeScale = 2f;

        if (gameMusic != null && isMusicOn)
            gameMusic.UnPause();
    }

    // 🔥 ОСЬ КНОПКА МУЗИКИ
    public void ToggleMusic()
    {
        if (gameMusic == null) return;

        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            gameMusic.UnPause();
        }
        else
        {
            gameMusic.Pause();
        }

        UpdateMusicText();
    }

    private void UpdateMusicText()
    {
        if (musicText != null)
        {
            musicText.text = isMusicOn ? "Music: ON" : "Music: OFF";
        }
    }

    private void Victory()
    {
        Time.timeScale = 1f;

        if (gameMusic != null)
            gameMusic.Stop();

        SetState(GameState.GameOver);
        ui?.ShowEndScreen(true);
    }

    private void Defeat()
    {
        Time.timeScale = 1f;

        if (gameMusic != null)
            gameMusic.Stop();

        SetState(GameState.GameOver);
        ui?.ShowEndScreen(false);
        StopAllCoroutines();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}