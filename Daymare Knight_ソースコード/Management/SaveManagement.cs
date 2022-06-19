using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManagement : Singleton<SaveManagement>
{
    string sceneName = "sceneName";
    public string SceneName {get {return PlayerPrefs.GetString(sceneName);}}

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("baocun");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }

    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
        
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStates.characterData, GameManager.Instance.playerStates.characterData.name);
    }

    public void Save(object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }


}
