using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Plataform platPrefab;
    public GameObject[] laterais;
    public GameObject barreiraPrefab;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject scorePanel;
    public Text afterResume;
    public Text points;
    public Text bestScorePanel;
    public Text scoreScorePanel;
    public string ballName;

    private static bool isBallAlive;
    private Plataform plat2;
    private Ball ball;
    private Vector2 firstPos;
    private float lastBarYPos;
    private bool isPaused = false;
    private float pointsHeight;
    private float pointsLights;
    private int score;
    private const int timeToNextAd = 300;
    private float timeSinceLastAd = 0;
    private bool flag;
    
    private void setIsBallAlive(bool b)
    {
        isBallAlive = b;
    }

    private void Awake()
    {
        Instantiate(Resources.Load("Prefabs/Balls/" + ballName));
    }

    void Start () {
        ball = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>();
        isBallAlive = true;
        Vector3 l1 = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));
        Vector3 l2 = new Vector3((l1.x + 0.375f) * -1, 0, 0);

        laterais[0].transform.position = l1;
        laterais[1].transform.position = l2;
    }
	
	void Update () {
        foreach(GameObject o in laterais) {
            o.transform.position = new Vector2(o.transform.position.x, ball.transform.position.y);
        }

        SpawnBlocks();

        score = (int)(pointsHeight + pointsLights);
        points.text = score.ToString();

        //print(Time.realtimeSinceStartup + "   |   " + (timeSinceLastAd + timeToNextAd));
        if (!isBallAlive)
        {
            if (Time.realtimeSinceStartup >= timeSinceLastAd + timeToNextAd)
            {
                timeSinceLastAd = Time.time;
                ShowAds();
            }
            if (score > PlayerPrefs.GetFloat("BestScore"))
            {
                PlayerPrefs.SetFloat("BestScore", score);
            }

            StartCoroutine(ScoreScreen());
        }
        else
        {
            VerifyInput();
        }
    }

    void SpawnBlocks()
    {
        if (transform.position.y >= lastBarYPos + UnityEngine.Random.Range(10.0f, 20.0f))
        {
            Instantiate(barreiraPrefab, new Vector2(UnityEngine.Random.Range(-1, 1), transform.position.y), Quaternion.identity);
            lastBarYPos = transform.position.y;
        }
    }

    void VerifyInput() {

        if (Input.GetMouseButtonDown(0) && !isPaused) {
            if (Input.mousePosition.y < Screen.height / 2)
            {
                firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                plat2 = Instantiate(platPrefab, firstPos, Quaternion.identity) as Plataform;
                plat2.GetComponent<Collider2D>().enabled = false;
                flag = true;
            }
        }

        if (Input.GetMouseButton(0) && !isPaused && flag) {
            Vector2 mouse2dPosScreen = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mouse2dPosWorld = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            //---------------Scale------------------------------
            var v3 = firstPos - mouse2dPosWorld;
            plat2.GetComponent<SpriteRenderer>().size = new Vector2 (v3.magnitude * 1.55f, plat2.GetComponent<SpriteRenderer>().size.y);
            plat2.GetComponent<BoxCollider2D>().size = new Vector2(v3.magnitude * 1.55f, plat2.GetComponent<BoxCollider2D>().size.y);
            plat2.GetComponent<BoxCollider2D>().offset = new Vector2(plat2.GetComponent<SpriteRenderer>().size.x / 2, 0);
            //plat2.transform.localScale = new Vector3(v3.magnitude * 1.55f, .3f, 0); //Existe uma gambiarra nesta linha

            //---------------Rotation------------------------------
            var object_pos = Camera.main.WorldToScreenPoint(plat2.transform.position);
            mouse2dPosScreen.x = mouse2dPosScreen.x - object_pos.x;
            mouse2dPosScreen.y = mouse2dPosScreen.y - object_pos.y;
            var angle = Mathf.Atan2(mouse2dPosScreen.y, mouse2dPosScreen.x) * Mathf.Rad2Deg;
            plat2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        if (Input.GetMouseButtonUp(0) && !isPaused && flag) {
            if (plat2)
            {
                if (Input.mousePosition.y < Screen.height / 2)
                {
                    plat2.GetComponent<Collider2D>().enabled = true;
                    plat2.GetComponent<SpriteRenderer>().color = new Color(plat2.GetComponent<SpriteRenderer>().color.r, plat2.GetComponent<SpriteRenderer>().color.g, plat2.GetComponent<SpriteRenderer>().color.b, 255);
                    plat2.ready = true;
                    flag = false;
                }
                else
                {
                    Destroy(plat2.gameObject);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isBallAlive)
        {
            StartCoroutine(Pause());
        }
    }
    
    IEnumerator DestroyPalataform()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    public void SumPointsLights(float addPoints)
    {
        this.pointsLights += addPoints;
    }

    public void SumPointsHeight(float cameraPos)
    {
        this.pointsHeight = cameraPos;
    }

    public void PauseGame()
    {
        StartCoroutine(Pause());
    }

    public IEnumerator Pause()
    {
        pausePanel.SetActive(!isPaused);
        if (isPaused)
        {
            afterResume.gameObject.SetActive(true);
            afterResume.text = "3";
            yield return WaitForRealSeconds(1);
            afterResume.text = "2";
            yield return WaitForRealSeconds(1);
            afterResume.text = "1";
            yield return WaitForRealSeconds(1);
            afterResume.gameObject.SetActive(false);
        }
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        
        yield return null;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        ShowAds();
        SceneManager.LoadScene(0);
    }

    public void Share()
    {

    }
    
    public void ShowAds()
    {
        if (Advertisement.IsReady())
            Advertisement.Show("", new ShowOptions() { resultCallback = HandleAdResult });
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Player Gains +5 Gems");
                break;
            case ShowResult.Skipped:
                Debug.Log("Player did not fully watched ad");
                break;
            case ShowResult.Failed:
                Debug.Log("Player failed to launch the ad");
                break;
        }
    }
    

    public static IEnumerator WaitForRealSeconds(float time) //Works even with Time.timeScale is set to 0
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    IEnumerator ScoreScreen()
    {
        yield return null;
        scorePanel.SetActive(true);
        bestScorePanel.text = "BEST: " + PlayerPrefs.GetFloat("BestScore").ToString();
        scoreScorePanel.text = "Score: " + score.ToString();
        pauseButton.SetActive(false);
    }
}
