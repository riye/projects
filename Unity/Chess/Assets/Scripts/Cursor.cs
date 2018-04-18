using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    bool isActiveState;

    ParticleSystem childParticleSystem;
    GameObject inactiveCursor;

    // Use this for initialization
    void Start()
    {
        childParticleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
        particleMain = childParticleSystem.main;
        colorGradient = childParticleSystem.colorOverLifetime.color;
        lastAlphaKey = colorGradient.gradient.alphaKeys[colorGradient.gradient.alphaKeys.Length - 1];

        inactiveCursor = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            //transform.position = hitInfo.point + hitInfo.normal;
            //transform.LookAt(hitInfo.point);
            transform.position = headPosition + 5f * gazeDirection;
            transform.LookAt(headPosition);
        }
        else
        {
            transform.position = headPosition - 5f * gazeDirection;
        }


        SetParticleSystemBehaviour();
    }

    // means don't show the cursor at all
    bool isInvisible;

    public void SetInvisible(bool invis)
    {
        isInvisible = invis;
        if (isInvisible)
            print("is invisible");
        else
            print("is visible now");
    }

    public void SetActiveState(bool value)
    {
        //print("changing cursor active to: " + value.ToString());
        if (isActiveState != value)
        {
            isActiveState = value;
        }
        print("set state to active: " + isActiveState);
    }

    ParticleSystem.MainModule particleMain;
    ParticleSystem.MinMaxGradient colorGradient;
    GradientAlphaKey lastAlphaKey;

    // KEYS OFF isActiveState
    void SetParticleSystemBehaviour()
    {
        var emission = childParticleSystem.emission;

        if (isInvisible)
        {
            inactiveCursor.SetActive(false);
            emission.enabled = false;

        }
        // ACTIVE: set the scale to be 1 and start speed to be 0.2

        else if (isActiveState)
        {
            //childParticleSystem.gameObject.SetActive(true);
            emission.enabled = true;
            inactiveCursor.SetActive(false);

            //childParticleSystem.transform.localScale = 0.7f*Vector3.one;
            //particleMain.startSpeed = 0.2f;
            //lastAlphaKey.alpha = 12;
        }

        // INACTIVE: set scale to be 1.2 and start speed to be 0.1 (and the colouring? don't fade so much ???)
        else
        {
            //childParticleSystem.transform.localScale = 1.3f * Vector3.one;
            //particleMain.startSpeed = 0f;
            //lastAlphaKey.alpha = 214;
            
            emission.enabled = false;
            //childParticleSystem.gameObject.SetActive(false);
            inactiveCursor.SetActive(true);
        }
    }
}
