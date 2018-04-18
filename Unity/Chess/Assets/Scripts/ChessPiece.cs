using UnityEngine;
using System.Collections;
using UnityEditor;

// scale by 500
// rotate x by -90
//

public class ChessPiece : ChessObject
{
    public bool IsWhiteTeam;
    public ChessPieceEnum pieceType;
    public bool hasMoved = false;
    public bool justFinishedFirstMove = false;
    public int direction;

    public void Initialize(int row, int column, bool isBlack)
    {
        if (isBlack)
        {
            transform.Rotate(0f, 0f, 180f);
        }
        InitializeGridPosition(row, column);

    }

    public void InitializeGridPosition(int row, int column)
    {
        gridPosition = new Vector2(row, column);
    }

    // set the gridPosition and also set the REAL X Y COORDINATE POSITION
    public void MakeMove(Vector2 newPosition)
    {
        if (!hasMoved)
        {
            justFinishedFirstMove = true;
        }
        else if (justFinishedFirstMove)
        {
            justFinishedFirstMove = false;
        }
        hasMoved = true;

        gridPosition = newPosition;
        transform.position = new Vector3(10 * newPosition.x, 5.6f, 10 * newPosition.y);
        isSelected = false;
    }

    // Use this for initialization
    void Start()
    {
        tag = IsWhiteTeam ? "Player" : "Enemy";
        direction = IsWhiteTeam ? 1 : -1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnHighlight()
    {
        base.OnHighlight();
        isHighlighted = true;
        print("highlighting " + pieceType.ToString());
        // if it's currently the piece's team's turn, and this piece becomes highlighted
        GetComponent<Renderer>().material.color = Color.magenta;

    }

    public override void RemoveHighlight()
    {
        base.RemoveHighlight();
        if (!isSelected)
        {
            isHighlighted = false;
            if (IsWhiteTeam)
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }

    public override void OnSelected()
    {
        base.OnSelected();
        print("selected");
        GetComponent<Renderer>().material.color = Color.red;
        isSelected = true;
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        GetComponent<Renderer>().material.color = Color.magenta;
        print("deselected");
        RemoveHighlight();
        isSelected = false;
    }

}