using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour {

    public static GameControl control;

    public bool showIntro = false;
    public int highScore = 0;
    public int highCombo = 0;
    public float highRunTime = 0;

    // Use this for initialization
    void Awake ()
    {
		if (control == null)
            control = this;
        else if (control != this)
            Destroy(gameObject);
        
#if !UNITY_EDITOR
        Load();
#endif
    }
	
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameDataPref.dat");
        GameData data = new GameData();
                
        data.showIntro = showIntro;
        data.highScore = highScore;
        data.highCombo = highCombo;
        data.highRunTime = highRunTime;

        bf.Serialize(file, data);
        file.Close();
    }


    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/GameDataPref.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameDataPref.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            showIntro = data.showIntro;
            highScore = data.highScore;
            highCombo = data.highCombo;
            highRunTime = data.highRunTime;
        }
    }
}

[Serializable]
class GameData
{
    public bool showIntro;
    public int highScore;
    public int highCombo;
    public float highRunTime;
}
