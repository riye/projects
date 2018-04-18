using System.Collections.Generic;
using UnityEngine;


// This is the game script, the game controller
public class PawWars : MonoBehaviour
{
    // corresponding prefabs to the WarriorType ENUM
    public Object basicWarriorCat, heavyWarriorCat, diveBomberCat;

    public GameState GameState;

    public GameObject PauseMenu;

    // another correspondence to a script type for the type of cat?

    void Awake()
    {
         // Initialize the army and their type properties in the global reference file
        PawWarsRef.BasicCatPrefabs = new Dictionary<WarriorType, Object>();
        PawWarsRef.BasicCatPrefabs.Add(WarriorType.BasicWarrior, basicWarriorCat);
        PawWarsRef.BasicCatPrefabs.Add(WarriorType.HeavyWarrior, heavyWarriorCat);
        PawWarsRef.BasicCatPrefabs.Add(WarriorType.DiveBomber, diveBomberCat);

        PawWarsRef.PropertiesList = new Dictionary<WarriorType, Properties>();
        PawWarsRef.PropertiesList.Add(WarriorType.BasicWarrior, new Properties(100, 30, 0, 1, 10, "cat", 0, 100, 50, false, false));
        PawWarsRef.PropertiesList.Add(WarriorType.HeavyWarrior, new Properties(100, 100, 0, 1, 10, "heavyCat", 0, 100, 50, false, false));
        PawWarsRef.PropertiesList.Add(WarriorType.DiveBomber, new Properties(100, 30, 0, 1, 10, "", 0, 100, 50, true, true));
    }

    void Start()
    {
        GameState = GameState.Beginning;
        PauseMenu.SetActive(false);
    }

    // checking for status of the game:
    // 0. Pre Game
    // 1. Game begin
    // 2. In Game
    // 3. Pause
    // 4. Game End - player wins or loses
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameState != GameState.Paused)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame(); 
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        GameState = GameState.Paused;
        PauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        GameState = GameState.Running;
        PauseMenu.SetActive(false);
    }
}

public enum GameState
{
    Beginning = 0,
    Running = 1,
    Paused = 2,
    EndedSuccess = 3,
    EndedFailure = 4,
}