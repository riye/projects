using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessObject : MonoBehaviour {

    public Vector2 gridPosition;

    public bool isSelected;
    public bool isHighlighted;


    // Use this for initialization
    void Start () {
        isSelected = false;
        isHighlighted = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //
    // Visual state setters only (edges of transition)
    //

    public virtual void OnHighlight()
    {
        isHighlighted = true;
    }

    public virtual void RemoveHighlight()
    {
        isHighlighted = false;
    }

    // chess piece only
    public virtual void OnSelected()
    {
        isHighlighted = false;
        isSelected = true;
    }

    public virtual void OnDeselected()
    {
        isSelected = false;
    }

    // change the colour specially for highlighting over selected ?
    // only for chess pieces
    public virtual void OnHighlightSelected()
    {

    }
}
