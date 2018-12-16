using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {

    public Ball ball;
    public Vector3 velocity = Vector3.zero;
    private GameManager man;

    private void Start()
    {
        man = this.GetComponentInChildren<GameManager>();
        ball = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>();
    }

    void Update()
    {
        man.SendMessage("SumPointsHeight", transform.position.y);
    }

    void FixedUpdate () {
        if (ball != null) {
            if (ball.transform.position.y >= transform.position.y) {
                transform.position = new Vector3(0, Vector3.SmoothDamp(transform.position, ball.transform.position, ref velocity, 0.5f).y, -10);
            }
        }
	}
}
