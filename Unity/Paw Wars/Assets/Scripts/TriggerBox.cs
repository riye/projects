using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.layer = 11; //aerialCats layer
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        var collTag = coll.gameObject.tag;
        if (collTag != "Aerial" && collTag != "Ground" && collTag != "NotActive" && transform.parent.tag != "NotActive")
        {
            if ((transform.parent.tag == "Ally" && collTag == "Enemy") || (transform.parent.tag == "Enemy" && collTag == "Ally"))
            {
                coll.gameObject.SendMessage("TakeDamage", 200);
            }
        }
    }

    void TakeDamange()
    {
        transform.parent.SendMessageUpwards("TakeDamage", 200);
    }
}
