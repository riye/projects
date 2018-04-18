using UnityEngine;
using System.Collections;

public class GameObjectBase : MonoBehaviour {

    // running if true
    bool gameState = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.timeScale == 0)
        {
            // do nothing ! paused
        }
        else
        {
            RunUpdate();
        }
	    //if (gameState && !PawWarsRef.IsGameRunning)
     //   {
     //       gameState = false;
     //       SaveObjectState();
     //       FreezeObject();
     //   }
     //   else if (!gameState && PawWarsRef.IsGameRunning)
     //   {
     //       ReloadObjectState();
     //   }
        //else if (gameState)
        //{
        //    RunUpdate();
        //}
	}

    public virtual void RunUpdate()
    {

    }
}
