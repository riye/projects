using UnityEngine;
using System.Collections;

public class ChessBoard : MonoBehaviour
{
    // Piece prefabs
    public GameObject PawnPrefab;
    public GameObject RookPrefab;
    public GameObject BishopPrefab;
    public GameObject KnightPrefab;
    public GameObject QueenPrefab;
    public GameObject KingPrefab;

    // Grid prefabs
    public GameObject BlackPrefab;
    public GameObject WhitePrefab;
    public GridPieceData[,] grid;
    //  [letters (column), numbers (row)]

    const int gridWidth = 10;
    const int length = 8;
    
    Quaternion initialPieceRotation = Quaternion.Euler(-90f, 0f, 0f);

    // Use this for initialization
    void Start()
    {
        GameObject instantiatePrefab;

        grid = new GridPieceData[length, length];

        // initialize the board layout
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if ((IsEven(i) && !IsEven(j)) || (IsEven(j) && (!IsEven(i))))
                {
                    instantiatePrefab = WhitePrefab;
                }
                else
                {
                    instantiatePrefab = BlackPrefab;
                }
                grid[i, j] = new GridPieceData();
                grid[i, j].ChessPiece = null;
                grid[i, j].GridObject = (GameObject)Instantiate(instantiatePrefab, new Vector3(gridWidth * i, 0, gridWidth * j), Quaternion.identity);
                grid[i, j].GridObject.transform.parent = transform;
                grid[i, j].GridObject.GetComponent<GridPiece>().gridPosition = new Vector2(i, j);
                grid[i, j].gridPosition = new Vector2(i, j);
            }
        }

        // Initialize Chess Pieces:
        InitializeBlackTeam();
        InitializeWhiteTeam();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsEven(int i)
    {
        return i % 2 == 0;
    }

    
    #region Initialize

    // White team is on rows 1 and 2, spanning A - H
    public void InitializeWhiteTeam()
    {
        for (int i = 0; i < 8; i++)
        {
            InitializeWhitePiece(PawnPrefab, i, 1);
        }
        InitializeWhitePiece(RookPrefab, 0, 0);
        InitializeWhitePiece(RookPrefab, 7, 0);
        InitializeWhitePiece(KnightPrefab, 1, 0);
        InitializeWhitePiece(KnightPrefab, 6, 0);
        InitializeWhitePiece(BishopPrefab, 2, 0);
        InitializeWhitePiece(BishopPrefab, 5, 0);
        InitializeWhitePiece(KingPrefab, 3, 0);
        InitializeWhitePiece(QueenPrefab, 4, 0);
    }

    public void InitializeWhitePiece(GameObject pieceType, int column, int row)
    {
        GameObject obj = (GameObject)Instantiate(pieceType, UnitToPosition(column, row), initialPieceRotation);
        ChessPiece pieceScript = obj.GetComponent<ChessPiece>();
        pieceScript.InitializeGridPosition(column, row);
        grid[column, row].ChessPiece = obj;
    }

    // White team is on rows 7 and 8, spanning A - H
    public void InitializeBlackTeam()
    {
        for (int i = 0; i < 8; i++)
        {
            InitializeBlackPiece(PawnPrefab, i, 6);
        }
        InitializeBlackPiece(RookPrefab, 0, 7);
        InitializeBlackPiece(RookPrefab, 7, 7);
        InitializeBlackPiece(KnightPrefab, 1, 7);
        InitializeBlackPiece(KnightPrefab, 6, 7);
        InitializeBlackPiece(BishopPrefab, 2, 7);
        InitializeBlackPiece(BishopPrefab, 5, 7);
        InitializeBlackPiece(KingPrefab, 3, 7);
        InitializeBlackPiece(QueenPrefab, 4, 7);
    }

    public void InitializeBlackPiece(GameObject pieceType, int column, int row)
    {
        GameObject obj = (GameObject)Instantiate(pieceType, UnitToPosition(column, row), initialPieceRotation);
        obj.GetComponent<Renderer>().material.color = Color.black;
        ChessPiece pieceScript = obj.GetComponent<ChessPiece>();
        pieceScript.IsWhiteTeam = false;
        pieceScript.Initialize(column, row, true);
        grid[column, row].ChessPiece = obj;
    }

    #endregion
    

    // x ->    y ^ for the specific unit, so the same as the position
    Vector3 UnitToPosition(int column, int row)
    {
        // invalid input?
        // where the bottom right corner is 0, 0
        return new Vector3(gridWidth * column, 5.6f, gridWidth * row);
    }
}

public class GridPieceData
{
    public GameObject ChessPiece;
    public GameObject GridObject;
    public bool isHighlighted = false;
    public Vector2 gridPosition;
}