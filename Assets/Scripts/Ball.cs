using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ball : MonoBehaviour {
    
    private Rigidbody2D mRigidBody2D;
    
    private GameManager man;
    private static int tillAd = 15;

    void Start() {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mRigidBody2D.AddForce(new Vector2(0.0f, 300.0f));

        man = GameObject.FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Finish") {
            transform.gameObject.SetActive(false);
            tillAd--;
            Debug.Log("Perdeu. " + tillAd);
            if (tillAd == 0)
            {
                tillAd = 15;
                ShowAds();
            }
            //Invoke("RestartGame", 2);
            man.SendMessage("setIsBallAlive", false);
        }
        else if(other.tag == "Block") {
            man.SendMessage("SumPointsLights", 10);
        }
        else if (other.tag == "EspecialBlock") {
            man.SendMessage("SumPointsLights", 50);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.transform.tag == "Plataform") {
            if (coll.transform.localScale.x <= 1)
                mRigidBody2D.AddForce(new Vector2(0, 450));
            else
                mRigidBody2D.AddForce(new Vector2(0, 450/coll.transform.localScale.x));
        }
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
}
