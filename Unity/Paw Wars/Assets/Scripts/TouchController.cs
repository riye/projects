using UnityEngine;
using System.Collections;
using System;

public class TouchController : MonoBehaviour
{
    private bool swiping = false;
    private Vector3 startPosition; //change to vector2 for touch input
    private bool catLaunched = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount == 0)
        //    return;

        //the same moving swipe's continuous events
        //if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
        if (Input.GetMouseButton(0))
        {
            print("touch detected");

            //initial touch down
            if (!swiping)
            {
                swiping = true;
                //startPosition = Input.GetTouch(0).position;
                startPosition = Input.mousePosition;
                return;
            }
            //continuous touch down
            else if (!catLaunched)
            {
                print("swipe detected");

                //Vector2 direction = Input.GetTouch(0).position - startPosition;
                Vector3 direction = Input.mousePosition - startPosition;
                // swiping up (+y) and right (+x)
                //if the angle over a span of 1cm of distance accumulated
                if (direction.x > 1 && direction.y > 2)
                {
                    print("launch detected");
                    LaunchCats();
                }
                else if (direction.x > 0 && Mathf.Abs(direction.y) < 2)
                {
                    print("hasten detected");
                    HastenCats();
                }
                startPosition = Input.mousePosition;
            }
        }
        // Reset swipe
        else
        {
            swiping = false;
            catLaunched = false;
        }
    }

    public void LaunchCats()
    {
        //find the closest cat at the basepoint of the touch (where it started)
        //send event to launch it if the distance is close enough
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(startPosition), -Vector2.up);
        if (hit.collider != null)
        {
            hit.collider.gameObject.SendMessage("SetState", CatState.Launch);
            catLaunched = true;
        }
    }

    public void HastenCats()
    {
        //find the closest cat at the basepoint of the touch (where it started)
        //send event to launch it if the distance is close enough
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(startPosition), -Vector2.up);
        if (hit.collider != null)
        {
            hit.collider.gameObject.SendMessage("SetState", CatState.Run);
            catLaunched = true;
        }
    }
}
