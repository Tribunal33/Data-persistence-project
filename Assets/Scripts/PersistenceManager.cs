using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour
{
    public PersistenceManager Instance;
    public static string playerName;
    public static int playerHighScore;
    public int playerScore;
    [Serializable]
    public class SavePlayerData{
        public string playerName;
        public int playerScore;
        public int highScore;

    }
    void Awake(){
        if(Instance != null){
            Destroy(Instance);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }
    public void SavePlayerInfo(){
        SavePlayerData data = new SavePlayerData();
        data.playerName = playerName;
        data.playerScore = playerScore;
        data.highScore = playerHighScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveplayerdata.json", json);
    }

    public void LoadHighScore(){
        string path = Application.persistentDataPath + "/saveplayerdata.json";

        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            SavePlayerData data = JsonUtility.FromJson<SavePlayerData>(json);

            playerName = data.playerName;
            playerScore = data.playerScore;
        }
    }

    public void StartGame(){
        SavePlayerInfo();
        SceneManager.LoadScene("main");
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public int CheckHighScore(int currentScore){

        return 0;
    }
}
