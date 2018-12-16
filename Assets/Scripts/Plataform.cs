using UnityEngine;
using System.Collections;

public class Plataform : MonoBehaviour {

    [HideInInspector]
    public bool ready;

	// Use this for initialization
	void Start () {
        transform.tag = "Plataform";
        ready = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (ready) {
            StartCoroutine(DestroyPalataform());
            ready = false;
        }
	}

    IEnumerator DestroyPalataform() {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
