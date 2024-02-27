using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance;
    public TextMeshProUGUI topScoreText;
    public int playerHighScore;
    private GameObject playerNameObject;
    public string playerInput;
    public string playerName;
    public int playerScore = 0;
    [Serializable]
    public class SavePlayerData{
        public string playerName;
        public int playerScore;
        public int currentHighScore;

    }
    void Awake(){
        playerNameObject = GameObject.Find("Player Name");
        if(Instance != null){
            Destroy(Instance);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
        Debug.Log("Finished Loading");
    }
    public void PlayerInputChange(string text){
        Debug.Log(text);
        playerInput = playerNameObject.GetComponent<TMP_InputField>().text;
    }
    public void SavePlayerInfo(){
        Debug.Log("Saving Player...");
        SavePlayerData data = new SavePlayerData();
        data.playerName = playerName;
        data.playerScore = playerScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveplayerdata.json", json);
    }

    public void LoadHighScore(){
        string path = Application.persistentDataPath + "/saveplayerdata.json";
        Debug.Log("Loading Player");
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            SavePlayerData data = JsonUtility.FromJson<SavePlayerData>(json);
            if(data != null){
                Debug.Log("Gathering data...");
                playerName = data.playerName;
                playerScore = data.playerScore;
                topScoreText.text = "Best Score : " + playerScore + " By: " + playerName ;
            }
            
        }
    }

    public void StartGame(){
        Debug.Log("Clicked Start Button");
        playerName = playerInput;
        if(playerName != null){
            Debug.Log("My Name is: " + playerName);
            SceneManager.LoadScene("main");
        }
        
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public int CheckHighScore(int currentScore){

        return currentScore > playerHighScore ? currentScore : playerHighScore;
    }
}
