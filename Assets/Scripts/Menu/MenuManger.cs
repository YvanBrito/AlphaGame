using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class MenuManger : MonoBehaviour {
	void Start()
	{
		Time.timeScale = 1.0f;
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowTwitter()
    {
        Application.OpenURL("https://twitter.com/britoyvan");
    }

    public void ShowAds()
    {
        if (Advertisement.IsReady())
            Advertisement.Show("", new ShowOptions() { resultCallback = HandleAdResult });
    }

    private void HandleAdResult (ShowResult result)
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
}
