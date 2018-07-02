using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject MainCamera;
    List<Transform> children = new List<Transform>();
    string state = "A";
    string prevState = "A";
    public GameObject sphere;
    public static Menu menu;
    public GameObject player;
    public GameObject nugget;

    public Text[] FinalScores;

    float defaultTimeScale = 0.8f;
    float returnTimeScale = 1f;

	// Use this for initialization
	void Awake () {
        Time.timeScale = 0;

        if (menu != null)
            Destroy(gameObject);
        else
            menu = this;


        int screenSize = Mathf.Min(Screen.width, Screen.height);

        RecursiveChildFinder(transform);

        foreach (Transform child in children)
        {
            if (child.GetComponent<Text>() != null)
                child.GetComponent<Text>().fontSize = (int)(screenSize / 25f);
        }

    }

    public void Play()
    {
        Player.player.transform.position = new Vector3(0f, -5f, 0f);
        MainCamera.GetComponent<Cam>().ResetPosition();

        Time.timeScale = defaultTimeScale;
        
        if (GameControl.control.showIntro)
        {
            stateChange("B");
            IntroCutScene.introcutscene.CutScene();
        }
        else
        {
            Player.player.GetComponent<AudioSource>().Play();
            stateChange("C");
            player.transform.GetComponent<Collider2D>().enabled = true;
            player.transform.GetComponent<Player>().enabled = true;
            MovingParts.movingparts.gamePlaying = true;

            GameObject[] Tunnels = GameObject.FindGameObjectsWithTag("Tunnel");
            foreach (GameObject tunnel in Tunnels)
            {
                tunnel.GetComponent<TranslateDestroy>().enabled = true;
            }

            player.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }

    }

    public void UnPause()
    {
        Time.timeScale = returnTimeScale;
        stateChange("C");
    }

    public void Pause()
    {
        returnTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        stateChange("D");
        Player.player.GetComponent<AudioSource>().Pause();
    }

    public void Quit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void crashReplay()
    {
        Time.timeScale = 0f;
        returnTimeScale = defaultTimeScale;
        stateChange("E");
        
        Player.player.GetComponent<AudioSource>().Stop();

        int screenSize = Mathf.Min(Screen.width, Screen.height);

        foreach (Text item in FinalScores)
        {
            item.fontSize = (int) (screenSize / 25f);
        }

        int score = (int) Player.player.score;
        int runTime = (int) Player.player.runTime;
        int M = runTime / 60;
        int S = runTime % 60;
        string seconds;
                
        if (S < 10)
            seconds = "0" + S.ToString();
        else
            seconds = S.ToString();
        
        FinalScores[0].text = "Score: " + score.ToString();
        FinalScores[1].text = "Biggest Combo: " + Player.player.highCombo.ToString();
        FinalScores[2].text = "Time survived: " + M.ToString()+":" + seconds.ToString();
                
        if (Player.player.score > GameControl.control.highScore || Player.player.highCombo > GameControl.control.highCombo || Player.player.runTime > GameControl.control.highRunTime)
            newHighScore();
    }

    void newHighScore()
    {

        if (Player.player.score > GameControl.control.highScore)
        {
            GameControl.control.highScore = (int)Player.player.score;

            foreach (Transform child in transform)
            {
                if (child.CompareTag("J") && child.name.Contains("Score"))
                    child.gameObject.SetActive(true);
            }
        }        

        if (Player.player.highCombo > GameControl.control.highCombo)
        {
            GameControl.control.highCombo = Player.player.highCombo;

            foreach (Transform child in transform)
            {
                if (child.CompareTag("J") && child.name.Contains("Combo"))
                    child.gameObject.SetActive(true);
            }
        }

        if (Player.player.runTime > GameControl.control.highRunTime)
        {
            GameControl.control.highRunTime = Player.player.runTime;

            foreach (Transform child in transform)
            {
                if (child.CompareTag("J") && child.name.Contains("Time"))
                    child.gameObject.SetActive(true);
            }
        }


        GameControl.control.Save();

    }

    public void Replay()
    {
        Player.player.GetComponent<AudioSource>().pitch = 0.9f;

        foreach (GameObject obstacle in MovingParts.movingparts.ActiveObstacleInstances)
            Destroy(obstacle);


        stateChange("C");
        
        Player.player.Reset();

        player.gameObject.SetActive(false);
        player.transform.position = new Vector3(0, -5f, 0f);
        player.gameObject.SetActive(true);

        Instantiate(nugget, new Vector3(0f, 0f, 50f), Quaternion.Euler(0f, 0f, 180f));

        Time.timeScale = defaultTimeScale;
        Player.player.GetComponent<AudioSource>().Play();

    }

    public void toggleIntro()
    {
        GameControl.control.showIntro = !GameControl.control.showIntro;
#if !UNITY_EDITOR
        GameControl.control.Save();
#endif
    }

    public void Back()
    {
        stateChange(prevState);
    }

    public void HighScore()
    {
        int score = GameControl.control.highScore;
        int runTime = (int) GameControl.control.highRunTime;
        int M = runTime / 60;
        int S = runTime % 60;
        string seconds;
        
        if (S < 10)
            seconds = "0" + S.ToString();
        else
            seconds = S.ToString();
        
        stateChange("G");

        foreach (Transform child in children)
        {
            if (child.CompareTag("G"))
            {
                child.GetChild(0).GetComponent<Text>().text = "High Score: " + score.ToString();
                child.GetChild(1).GetComponent<Text>().text = "Biggest Combo: " + GameControl.control.highCombo.ToString();
                child.GetChild(2).GetComponent<Text>().text = "longest time survived: " + M.ToString() + ":" + seconds.ToString();
            }
        }
    }

    public void Credits()
    {
        stateChange("F");
    }

    public void stateChange(string newState)
    {
        prevState = state;
        state = newState;

        foreach (Transform child in transform)
        {
            if (child.tag.Contains(newState))
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

        if (newState == "C")
        {
            if (!Player.player.GetComponent<AudioSource>().isPlaying)
                Player.player.GetComponent<AudioSource>().Play();

            screenUnlocked(false);
        }
        else
            screenUnlocked(true);
    }

    void RecursiveChildFinder(Transform thisObject)
    {
        foreach (Transform child in thisObject)
        {
            children.Add(child);
            if (child.childCount > 0)
                RecursiveChildFinder(child);
        }
    }

    void screenUnlocked (bool allowScreenRotation)
    {
        Screen.autorotateToLandscapeLeft = allowScreenRotation;
        Screen.autorotateToLandscapeRight = allowScreenRotation;
        Screen.autorotateToPortrait = allowScreenRotation;
        Screen.autorotateToPortraitUpsideDown = allowScreenRotation;
    }
}
