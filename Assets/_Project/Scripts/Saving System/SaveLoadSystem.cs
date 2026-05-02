using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameData
{
    public string Name;
    public string CurrentLevelName;
    public PlayerData PlayerData;
    public CurrentDayData CurrentDayData;
}

public interface ISaveable
{
    SerializableGuid Id { get; set; }
}

public interface IBind<TData> where TData : ISaveable
{
    SerializableGuid Id { get; set; }
    void Bind(TData data);
}

public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
{
    [SerializeField] public GameData gameData;

    private IDataService dataService;
    private bool isLoadingGame;

    protected override void Awake()
    {
        base.Awake();
        dataService = new FileDataService(new JsonSerializer());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (gameData == null)
        {
            NewGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameData == null)
            return;

        gameData.CurrentLevelName = scene.name;

        Bind<PlayerController, PlayerData>(gameData.PlayerData);
        Bind<NpcCustomerSpawner, CurrentDayData>(gameData.CurrentDayData);

        isLoadingGame = false;
    }

    public void NewGame()
    {
        gameData = new GameData
        {
            Name = "SaveSlot1",
            CurrentLevelName = SceneManager.GetActiveScene().name,
            PlayerData = new PlayerData(),
            CurrentDayData = new CurrentDayData()
        };

        BindSceneObjects();
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("Cannot save because gameData is null.");
            return;
        }

        gameData.CurrentLevelName = SceneManager.GetActiveScene().name;

        dataService.Save(gameData);
        Debug.Log("Game saved.");
    }

    public void LoadGame(string gameName)
    {
        gameData = dataService.Load(gameName);

        if (gameData == null)
        {
            Debug.LogWarning($"No save data found for {gameName}.");
            return;
        }

        if (string.IsNullOrWhiteSpace(gameData.CurrentLevelName))
        {
            gameData.CurrentLevelName = SceneManager.GetActiveScene().name;
        }

        isLoadingGame = true;
        SceneManager.LoadScene(gameData.CurrentLevelName);
    }

    public void ChangeScene(string sceneName)
    {
        if (gameData == null)
        {
            NewGame();
        }

        SaveGame();

        gameData.CurrentLevelName = sceneName;

        SaveGame();

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadGame()
    {
        if (gameData == null)
            return;

        LoadGame(gameData.Name);
    }

    public void DeleteGame(string gameName)
    {
        dataService.Delete(gameName);
    }

    public void DeleteAllGames()
    {
        dataService.DeleteAll();
    }

    private void BindSceneObjects()
    {
        if (gameData == null)
            return;

        Bind<PlayerController, PlayerData>(gameData.PlayerData);
        Bind<NpcCustomerSpawner, CurrentDayData>(gameData.CurrentDayData);
    }

    private void Bind<T, TData>(TData data)
        where T : MonoBehaviour, IBind<TData>
        where TData : ISaveable, new()
    {
        var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();

        if (entity == null)
            return;

        if (data == null)
        {
            data = new TData
            {
                Id = entity.Id
            };
        }

        entity.Bind(data);
    }

    private void Bind<T, TData>(List<TData> datas)
        where T : MonoBehaviour, IBind<TData>
        where TData : ISaveable, new()
    {
        if (datas == null)
            return;

        var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

        foreach (var entity in entities)
        {
            var data = datas.FirstOrDefault(d => d.Id == entity.Id);

            if (data == null)
            {
                data = new TData
                {
                    Id = entity.Id
                };

                datas.Add(data);
            }

            entity.Bind(data);
        }
    }
}