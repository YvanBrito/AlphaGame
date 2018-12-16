using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    SpriteRenderer m_spriteRenderer;
    public Color[] colors;

	// Use this for initialization
	void Start () {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        transform.tag = "Block";
        /*if(Random.Range(0, 9) >= 8) {
            transform.tag = "EspecialBlock";
            //StartCoroutine(ChangeColor());
        }else {
            transform.tag = "Block";
        }*/
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }

    IEnumerator ChangeColor() {
        while (true) {
            m_spriteRenderer.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
