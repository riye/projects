using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.timeScale == 0)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
