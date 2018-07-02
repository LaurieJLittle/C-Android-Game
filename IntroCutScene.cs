using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutScene : MonoBehaviour {
    
    AudioSource[] Sounds;
    float t = 0;
    enum TUTORIAL { NotStarted, CutScene, GrabNuggets, Movement, Jump, Finished };
    TUTORIAL tutorial = TUTORIAL.NotStarted;

    public static IntroCutScene introcutscene;
    List<Transform> wheels = new List<Transform>();

    GameObject player;
    public GameObject MainCamera;
    bool rotateCamera;
    public GameObject CutSceneNugget;

    public Transform camRefBehindPlayer;
    public Transform camRefInFrontOfPlayer;
    public Transform camRefAboveHole;
    public Transform Tunnels;

    void Awake ()
    {
        if (introcutscene != null)
            Destroy(gameObject);
        else
            introcutscene = this;
    }

    void Start()
    {
        player = Player.player.gameObject;
        wheels = Player.player.wheels;
        Sounds = GetComponents<AudioSource>();
    }
	

	void Update ()
    {
        switch (tutorial)
        {
            case TUTORIAL.CutScene:

                player.transform.Translate(new Vector3(0f, 0f, Time.deltaTime * 3f));

                foreach (Transform wheel in wheels)
                    wheel.Rotate(new Vector3(transform.localRotation.eulerAngles.x - 90f, 0f, 0f), (-2f * 400f / 6.283f) * Time.fixedDeltaTime);

                if (rotateCamera)
                {
                    MainCamera.transform.Rotate(new Vector3(20f * Time.deltaTime, 0f, 0f));
                }
                break;        

            case TUTORIAL.GrabNuggets:
                showingTutorial("X");
                break;
            case TUTORIAL.Movement:
                showingTutorial("Y");
                break;
            case TUTORIAL.Jump:
                showingTutorial("Z");
                break;
            case TUTORIAL.Finished:
                Menu.menu.stateChange("C");
                tutorial = (TUTORIAL)((int)tutorial + 1);
                break;            
        }
    }
        

    IEnumerator CameraCoroutine()
    {
        updateCameraPosition(camRefInFrontOfPlayer, true);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(3.5f);
                
        updateCameraPosition(camRefBehindPlayer, true);
        yield return new WaitForSeconds(4.0f);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);

        updateCameraPosition(camRefAboveHole, false);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.0f);
        rotateCamera = true;
        yield return new WaitForSeconds(3.0f);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(3.0f);

        MainCamera.GetComponent<Cam>().ResetPosition();
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeIn");
        player.transform.GetComponent<Player>().enabled = true;
        player.transform.GetComponent<Collider2D>().enabled = true;
        MovingParts.movingparts.gamePlaying = true;
        
        foreach (Transform Tunnel in Tunnels)
            Tunnel.GetComponent<TranslateDestroy>().enabled = true;        
    }


    IEnumerator AnimationCoroutine ()
    {
        PlaySound(Sounds, "Wheel_Roll_slow");
        player.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        player.transform.position = new Vector3(0f, -50f, -11.6f); 
        yield return new WaitForSeconds(2.5f);
        
        player.GetComponent<Animator>().SetTrigger("LookAround");
        yield return new WaitForSeconds(6f);
        
        PlaySound(Sounds, "clink");
        player.GetComponent<Animator>().SetTrigger("ExamineNugget");
        CutSceneNugget.transform.SetParent(player.transform);
        CutSceneNugget.transform.localPosition = new Vector3(0.4f, 10.46f, 1.79f);
        yield return new WaitForSeconds(1.5f);

        PlaySound(Sounds, "Hmm");
        yield return new WaitForSeconds(3.5f);
        StopSound(Sounds);
        yield return new WaitForSeconds(0.2f);
        PlaySound(Sounds, "falling");
        yield return new WaitForSeconds(1.8f);

        Destroy(CutSceneNugget);
        tutorial = TUTORIAL.GrabNuggets;
        player.transform.position = new Vector3(0,-5f,0);
        player.transform.rotation = new Quaternion(0, 0, 0, 1);
        player.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        player.GetComponent<Animator>().SetTrigger("Idle");

        Menu.menu.stateChange("C");
        StopSound(Sounds);
    }


    public void CutScene()
    {
        tutorial = TUTORIAL.CutScene;
        StartCoroutine(CameraCoroutine());
        StartCoroutine(AnimationCoroutine());
    }


    void updateCameraPosition(Transform reference, bool referenceAsParent)
    {
        MainCamera.transform.position = reference.position;
        MainCamera.transform.rotation = reference.rotation;

        if (referenceAsParent)
            MainCamera.transform.SetParent(reference);
        else
            MainCamera.transform.SetParent(null);
    }


    void PlaySound(AudioSource[] Sources, string clipToPlay)
    {
        foreach (AudioSource source in Sources)
        {
            if (source.clip.ToString().Contains(clipToPlay))
                source.Play();
        }
    }


    void StopSound(AudioSource[] Sources)
    {
        foreach (AudioSource source in Sources)
            source.Stop();
    }


    void showingTutorial(string stage)
    {
        foreach (Transform child in Menu.menu.transform)
        {
            if (child.tag.Contains("CD"))
                child.gameObject.SetActive(false);
        }

        Color color = Color.red;
        t += Time.deltaTime;
        color.a = Mathf.Sin(t);
        
        foreach (Transform child in Menu.menu.transform)
        {
            if (child.tag.Contains(stage))
            {
                if (child.GetComponent<Text>() != null)
                    child.GetComponent<Text>().color = color;
                if (child.GetComponent<Image>() != null)
                    child.GetComponent<Image>().color = color;
            }
        }

        if (Mathf.Sin(t) < 0f)
        {
            tutorial = (TUTORIAL)((int)tutorial + 1);
            t = 0f;
        }
    }
}