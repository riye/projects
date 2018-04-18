using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPiece : ChessObject
{
    Color defaultColor;

    // Use this for initialization
    void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GridPrehighlight()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    public void GridRemovePrehighlight()
    {
        GetComponent<Renderer>().material.color = defaultColor;
    }
}
