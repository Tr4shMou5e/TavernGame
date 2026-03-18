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
public class SaveLoadSystem : PersistentSingleton< SaveLoadSystem>
{
    [SerializeField] public GameData gameData;
    
    IDataService dataService;
    
    protected override void Awake()
    {
        base.Awake();
        dataService = new FileDataService(new JsonSerializer());
    }
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Added Guard clause if you don't binding to happen in certain scenes, e.g., during the menu scene or when the game is pause maybe
        // if (scene.name == gameData.CurrentLevelName)
        // {
        //     return;
        // }
        Bind<PlayerController, PlayerData>(gameData.PlayerData);
    }
    
    void Start()
    {
        Debug.Log(gameData);
        NewGame();
        Debug.Log(gameData);
    }
    
    void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
    {
        var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
        if (entity != null)
        {
            if (data == null)
            {
                data = new TData { Id = entity.Id };
            }
            entity.Bind(data);
        }
    }
    void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
    {
        var entities = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (var entity in entities)
        {
            var data = datas.FirstOrDefault(d => d.Id == entity.Id);
            if (data == null)
            {
                data = new TData { Id = entity.Id };
                datas.Add(data);
            }
            entity.Bind(data);
        }
    }
    public void NewGame()
    {
        gameData = new GameData
        {
            Name = "Tavern",
            CurrentLevelName = "Tavern"
        };
        SceneManager.LoadScene(gameData.CurrentLevelName);
    }

    public void SaveGame() => dataService.Save(gameData);

    public void LoadGame(string gameName)
    {
        gameData = dataService.Load(gameName);
        
        if(String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
        {
            gameData.CurrentLevelName = "Tavern";
        }
        
        SceneManager.LoadScene(gameData.CurrentLevelName);
    }
    public void ReloadGame() => LoadGame(gameData.Name);
    public void DeleteGame(string gameName) => dataService.Delete(gameName);
    public void DeleteAllGames() => dataService.DeleteAll();
}