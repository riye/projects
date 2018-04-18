using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessGame : MonoBehaviour
{

    /// <summary>
    /// Master board that keeps track of all piece positions
    /// </summary>
    public ChessPieceEnum?[,] chessBoard;

    public bool isWhitePlayerTurn = true;

    int UnitWidth = 10;
    //int gridLength = 8;
    ChessBoard ChessBoardGrid;

    Cursor cursor;

    // Use this for initialization
    void Start()
    {
        //chessBoard = new ChessPieceEnum?[gridLength, gridLength];

        ChessBoardGrid = GameObject.Find("ChessBoard").GetComponent<ChessBoard>();

        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
    }

    ChessObject prevHighlightedObject = null;
    ChessObject prevSelectedObject = null;
    ChessPiece lastMovedPiece = null;
    bool pieceSelected = false;

    // Update is called once per frame
    void Update()
    {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        // #0: if an object has been hit by raycast:
        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            cursor.SetInvisible(false);
            var hitObj = hitInfo.collider.gameObject;

            // Try to get the object hit - its script. It must have this script.
            var pieceScript = hitObj.GetComponent<ChessObject>();
            if (pieceScript == null)
                return;

            // #1, remove highlight from old object if we're on a new object
            if (prevHighlightedObject != null && pieceScript != prevHighlightedObject)
            {
                cursor.SetActiveState(false);
                prevHighlightedObject.GetComponent<ChessObject>().RemoveHighlight();
                prevHighlightedObject = null;
            }


            // #3, check for clicking - a selection action
            if (Input.GetMouseButtonDown(0))
            {
                // #1: select a piece to move if it is A. not selected, B. currently that player's turn
                // #2: select piece to deselect if it is A. already selected (and currently that player's turn)
                if ((hitObj.tag == "Enemy" && !isWhitePlayerTurn) ||
                    (hitObj.tag == "Player" && isWhitePlayerTurn))
                {
                    if (!pieceScript.isSelected)
                    {
                        if (prevSelectedObject != null)
                        {
                            HidePossibleMoves();
                            prevSelectedObject.OnDeselected();
                        }

                        pieceScript.OnSelected();
                        prevSelectedObject = pieceScript;
                        pieceSelected = true;
                        ShowPossibleMoves();
                    }
                    else
                    {
                        HidePossibleMoves();
                        pieceScript.OnDeselected();
                        pieceSelected = false;
                        prevSelectedObject = null;
                    }
                    return;
                }

                // #3: select enemy piece to move to if it is on a highlighted grid object and it is on opposite of player turn
                // #4: select grid piece to move to if A. a piece has been selected, B. it is already highlighted/allowed
                else if (IsGridAtPositionHighlighted(pieceScript.gridPosition))
                {
                    MoveSelectedPieceToPosition(pieceScript.gridPosition);
                    pieceSelected = false;
                    prevSelectedObject = null;
                    prevHighlightedObject = null;
                    return;
                }
            }
            else
            {
                // highlighting an already-selected item: keep cursor on for selected item
                if (pieceScript == prevSelectedObject && pieceScript.isSelected)
                {
                    cursor.SetActiveState(true);
                    pieceScript.OnHighlightSelected();
                }

                // #2, check for highlights
                if (pieceScript.isSelected || pieceScript.isHighlighted)
                {
                    return;
                }

                // a piece is highlighted if it is A. not selected, B. currently that player's turn C. an enemy piece that is in line of fire, aka the position it's on is a highlighted grid
                else if (hitObj.tag == "Enemy" || hitObj.tag == "Player")
                {

                    if ((hitObj.tag == "Enemy" && !isWhitePlayerTurn) ||
                        (hitObj.tag == "Player" && isWhitePlayerTurn) ||
                        IsGridAtPositionHighlighted(pieceScript.gridPosition))
                    {
                        cursor.SetActiveState(true);
                        pieceScript.OnHighlight();
                        prevHighlightedObject = pieceScript;
                        return;
                    }
                }

                // a grid is hover highlighted if A. a piece has been selected and B. it is an allowed move aka it's already in a highlighted state
                else if (hitObj.tag == "Grid" && pieceSelected)
                {
                    if (IsGridAtPositionHighlighted(pieceScript.gridPosition))
                    {
                        cursor.SetActiveState(true);
                        pieceScript.OnHighlight();
                        prevHighlightedObject = pieceScript;
                        return;
                    }
                }
            }
        }
        // if raycast didn't hit object
        else
        {
            cursor.SetInvisible(true);

            if (prevHighlightedObject != null)
            {
                prevHighlightedObject.RemoveHighlight();
                prevHighlightedObject = null;
            }
            return;
        }

        cursor.SetActiveState(false);

        // if none of the above conditions were met for setting things, then just remove highlights and cursor
        if (prevHighlightedObject != null)
        {
            prevHighlightedObject.RemoveHighlight();
            prevHighlightedObject = null;
        }
    }

    List<GridPieceData> highlightedGrids = null;

    public void ShowPossibleMoves()
    {
        // 1. Take the selected object and based on its type, determine which grids to highlight
        // 2. Highlight those grids
        // 3. Add highlighted grid objects to a new array list, which we will later go through to dehighlight
        var piece = prevSelectedObject.gameObject.GetComponent<ChessPiece>();
        highlightedGrids = GetPiecePotentialMoves(piece);

        for (int i = 0; i < highlightedGrids.Count; i++)
        {
            highlightedGrids[i].isHighlighted = true;
            highlightedGrids[i].GridObject.GetComponent<GridPiece>().GridPrehighlight();
        }
    }

    public void HidePossibleMoves()
    {
        for (int i = 0; i < highlightedGrids.Count; i++)
        {
            highlightedGrids[i].isHighlighted = false;
            highlightedGrids[i].GridObject.GetComponent<GridPiece>().GridRemovePrehighlight();
        }
        highlightedGrids = null;

        prevSelectedObject.OnDeselected();
    }

    public void MoveSelectedPieceToPosition(Vector2 position)
    {
        // 0. Remove all highlights from grid
        HidePossibleMoves();

        // 1. If there was a piece there, take it off the grid. Move it into the pile of claimed pieces by current turn's player

        GridPieceData gridData = ChessBoardGrid.grid[(int)position.x, (int)position.y];
        if (gridData.ChessPiece != null)
        {
            AddPieceToDiscardPile(!isWhitePlayerTurn, gridData.ChessPiece);
            gridData.ChessPiece = null;
        }

        // 2. Move the selected piece to that spot (remove it from the old grid, add it to the new grid, 
        //      and update the position data within the piece object)
        ChessBoardGrid.grid[(int)prevSelectedObject.gridPosition.x, (int)prevSelectedObject.gridPosition.y].ChessPiece = null;
        gridData.ChessPiece = prevSelectedObject.gameObject;
        //gridData containing prev selection object needs to set chessPiece to null
        prevSelectedObject.gameObject.GetComponent<ChessPiece>().MakeMove(position);
        lastMovedPiece = prevSelectedObject.gameObject.GetComponent<ChessPiece>();
        prevSelectedObject = null;

        // 3. Calculate check/checkmate
        bool whiteCheck, whiteCheckmate, blackCheck, blackCheckmate;
        AnalyzeCheckmate(out whiteCheck, out whiteCheckmate, out blackCheck, out blackCheckmate);
        if (whiteCheckmate)
            print("white checkmate");
        else if (whiteCheck)
            print("white check");
        if (blackCheckmate)
            print("black checkmate");
        else if (blackCheck)
            print("black check");
        //if (whiteCH)
        //{
        //CheckMate();
        //}
        //else if (check)
        //{
        //    //Check();
        //}

        isWhitePlayerTurn = !isWhitePlayerTurn;
    }

    //white check = white is CHECKED, black is winning
    public void AnalyzeCheckmate(out bool whiteCheck, out bool whiteCheckmate, out bool blackCheck, out bool blackCheckmate)
    {
        List<GridPieceData> allWhitePotentialMoves = new List<GridPieceData>();
        List<GridPieceData> allBlackPotentialMoves = new List<GridPieceData>();
        List<GridPieceData> piecePotentialMoves = null;
        ChessPiece currentPiece = null;
        List<GridPieceData> whiteKingPotentialPositions = null;
        List<GridPieceData> blackKingPotentialPositions = null;

        // get list of all potential moves for each team
        foreach (var grid in ChessBoardGrid.grid)
        {
            if (grid.ChessPiece == null)
                continue;
            currentPiece = grid.ChessPiece.GetComponent<ChessPiece>();
            piecePotentialMoves = GetPiecePotentialMoves(currentPiece);
            foreach (GridPieceData potentialMove in piecePotentialMoves)
            {
                if (currentPiece.IsWhiteTeam)
                {
                    allWhitePotentialMoves.Add(potentialMove);
                }
                else
                {
                    allBlackPotentialMoves.Add(potentialMove);
                }
            }


            // get king's spaces if this is king piece
            if (currentPiece.pieceType == ChessPieceEnum.King)
            {
                if (currentPiece.IsWhiteTeam)
                {
                    whiteKingPotentialPositions = piecePotentialMoves;
                    AddPotentialMoveToList(whiteKingPotentialPositions, (int)currentPiece.gridPosition.x, (int)currentPiece.gridPosition.y);
                }
                else
                {
                    blackKingPotentialPositions = piecePotentialMoves;
                    AddPotentialMoveToList(blackKingPotentialPositions, (int)currentPiece.gridPosition.x, (int)currentPiece.gridPosition.y);
                }
            }
        }

        // after getting all the potential moves, determine if all of the king's positions are checked by the opponent team
        // if at least the last option is checked, but not all, it becomes check
        // if all are checked, then it's checkmato
        bool[] checkPositions = new bool[whiteKingPotentialPositions.Count];
        int counter = 0;
        whiteCheckmate = true;
        foreach (var kingGrid in whiteKingPotentialPositions)
        {
            checkPositions[counter] = false;

            foreach (var potentialMoveGrid in allBlackPotentialMoves)
            {
                // if we find any piece that can check that spot, break and move to the next king's spot
                if (potentialMoveGrid.gridPosition == kingGrid.gridPosition)
                {
                    checkPositions[counter] = true;
                    break;
                }
            }

            // if we find any position left available, it's not checkmate
            if (!checkPositions[counter])
            {
                whiteCheckmate = false;
            }
            counter++;
        }

        whiteCheck = checkPositions[counter - 1] == true;

        // now to check black side
        checkPositions = new bool[blackKingPotentialPositions.Count];
        counter = 0;
        blackCheckmate = true;
        foreach (var kingGrid in blackKingPotentialPositions)
        {
            checkPositions[counter] = false;

            foreach (var potentialMoveGrid in allWhitePotentialMoves)
            {
                // if we find any piece that can check that spot, break and move to the next king's spot
                if (potentialMoveGrid.gridPosition == kingGrid.gridPosition)
                {
                    checkPositions[counter] = true;
                    break;
                }
            }

            // if we find any position left available, it's not checkmate
            if (!checkPositions[counter])
            {
                blackCheckmate = false;
            }
            counter++;
        }

        blackCheck = checkPositions[counter - 1] == true;

        return;
    }

    int numBlackTaken;
    int numWhiteTaken;
    public void AddPieceToDiscardPile(bool isWhitePiece, GameObject piece)
    {
        // place the piece on the correct side of the player that took it, in a like a puddle (not a row
        var zOffset = !isWhitePiece ? -10 : 80;
        var xOffset = isWhitePiece ? numWhiteTaken * 5 : numBlackTaken * 5;
        piece.GetComponent<ChessPiece>().gridPosition = new Vector2(-1, -1);
        piece.GetComponent<ChessPiece>().RemoveHighlight();
        piece.transform.position = new Vector3(xOffset, 5.6f, zOffset); // (put it somewhere on the correct side)
        if (isWhitePiece)
        {
            numWhiteTaken++;
        }
        else
        {
            numBlackTaken++;
        }
    }

    public bool IsGridAtPositionHighlighted(Vector2 position)
    {
        return ChessBoardGrid.grid[(int)position.x, (int)position.y].isHighlighted;
    }


    #region Piece Logic

    private void AddPotentialMoveToList(List<GridPieceData> potentialMoves, int x, int y, bool conquer = false)
    {
        if (InGridRange(x, y))
        {
            potentialMoves.Add(ChessBoardGrid.grid[x, y]);
            //print("potential move: " + x + ", " + y + " conquer: " + conquer.ToString());
        }
    }

    // also need to have option to perform castling
    // gets 
    public List<GridPieceData> GetPiecePotentialMoves(ChessPiece piece)
    {
        List<GridPieceData> potentialMoves = new List<GridPieceData>();
        int x = (int)piece.gridPosition.x;
        int y = (int)piece.gridPosition.y;
        int d = piece.direction;
        ChessPiece enemyPiece = null;

        //print("piece is " + x + ", " + y + " in direction " + d);
        if (piece.pieceType == ChessPieceEnum.Pawn)
        {
            // move 1-2 spaces if it hasn't moved once yet
            if (!IsPieceInGrid(x, y + d))
            {
                AddPotentialMoveToList(potentialMoves, x, y + d);

                if (!piece.hasMoved && !IsPieceInGrid(x, y + 2 * d))
                {
                    AddPotentialMoveToList(potentialMoves, x, y + 2 * d);
                }
            }
            // conquering diagonal enemies
            if (IsEnemyPieceInGrid(x + 1, y + d, out enemyPiece))
            {
                AddPotentialMoveToList(potentialMoves, x + 1, y + d, true);
            }
            if (IsEnemyPieceInGrid(x - 1, y + d, out enemyPiece))
            {
                AddPotentialMoveToList(potentialMoves, x - 1, y + d, true);
            }
            // conquering an enemy that just jumped forward 2 spaces
            if (IsEnemyPieceInGrid(x + 1, y, out enemyPiece) && enemyPiece == lastMovedPiece &&
                enemyPiece.justFinishedFirstMove && enemyPiece.pieceType == ChessPieceEnum.Pawn)
            {
                AddPotentialMoveToList(potentialMoves, x + 1, y, true);
            }
            if (IsEnemyPieceInGrid(x - 1, y, out enemyPiece) && enemyPiece == lastMovedPiece &&
                enemyPiece.justFinishedFirstMove && enemyPiece.pieceType == ChessPieceEnum.Pawn)
            {
                AddPotentialMoveToList(potentialMoves, x - 1, y, true);
            }
        }
        if (piece.pieceType == ChessPieceEnum.Rook || piece.pieceType == ChessPieceEnum.Queen)
        {
            for (int i = x + 1; i < 8; i++)
            {
                if (IsPieceInGrid(i, y))
                {
                    if (IsEnemyPieceInGrid(i, y, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, i, y, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, i, y, true);
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (IsPieceInGrid(i, y))
                {
                    if (IsEnemyPieceInGrid(i, y, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, i, y, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, i, y, true);
            }
            for (int j = y + 1; j < 8; j++)
            {
                if (IsPieceInGrid(x, j))
                {
                    if (IsEnemyPieceInGrid(x, j, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, x, j, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, x, j, true);
            }
            for (int j = y - 1; j >= 0; j--)
            {
                if (IsPieceInGrid(x, j))
                {
                    if (IsEnemyPieceInGrid(x, j, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, x, j, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, x, j, true);
            }
        }
        if (piece.pieceType == ChessPieceEnum.Knight)
        {
            Vector2[] listPos = new Vector2[8] {
                    new Vector2(x + 2, y + 1),
                    new Vector2(x + 1, y + 2),
                    new Vector2(x - 2, y + 1),
                    new Vector2(x - 1, y + 2),
                    new Vector2(x + 2, y - 1),
                    new Vector2(x + 1, y - 2),
                    new Vector2(x - 2, y - 1),
                    new Vector2(x - 1, y - 2),
                };
            int posx, posy;
            for (int i = 0; i < 8; i++)
            {
                posx = (int)listPos[i].x;
                posy = (int)listPos[i].y;
                if ((!IsPieceInGrid(posx, posy) || IsEnemyPieceInGrid(posx, posy, out enemyPiece)))
                {
                    bool isConquer = enemyPiece != null;
                    AddPotentialMoveToList(potentialMoves, posx, posy, isConquer);
                }
            }
        }
        if (piece.pieceType == ChessPieceEnum.Bishop || piece.pieceType == ChessPieceEnum.Queen)
        {
            int j = y + 1;
            for (int i = x + 1; i < 8 && j < 8; i++)
            {
                if (IsPieceInGrid(i, j))
                {
                    if (IsEnemyPieceInGrid(i, j, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, i, j, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, i, j, true);
                j++;
            }

            j = y - 1;
            for (int i = x + 1; i < 8 && j >= 0; i++)
            {
                if (IsPieceInGrid(i, j))
                {
                    if (IsEnemyPieceInGrid(i, j, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, i, j, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, i, j, true);
                j--;
            }

            j = y - 1;
            for (int i = x - 1; i >= 0 && j >= 0; i--)
            {
                if (IsPieceInGrid(i, j))
                {
                    if (IsEnemyPieceInGrid(i, j, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, i, j, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, i, j, true);
                j--;
            }

            j = y + 1;
            for (int i = x - 1; i >= 0 && j < 8; i--)
            {
                if (IsPieceInGrid(i, j))
                {
                    if (IsEnemyPieceInGrid(i, j, out enemyPiece))
                    {
                        AddPotentialMoveToList(potentialMoves, i, j, true);
                    }
                    break;
                }
                AddPotentialMoveToList(potentialMoves, i, j, true);
                j++;

            }
        }
        if (piece.pieceType == ChessPieceEnum.King)
        {
            Vector2[] listPos = new Vector2[8] {
                    new Vector2(x + 1, y + 1),
                    new Vector2(x, y + 1),
                    new Vector2(x - 1, y + 1),
                    new Vector2(x + 1, y - 1),
                    new Vector2(x, y - 1),
                    new Vector2(x - 1, y - 1),
                    new Vector2(x + 1, y),
                    new Vector2(x - 1, y),
                };
            int posx, posy;
            for (int i = 0; i < 8; i++)
            {
                posx = (int)listPos[i].x;
                posy = (int)listPos[i].y;
                if ((!IsPieceInGrid(posx, posy) || IsEnemyPieceInGrid(posx, posy, out enemyPiece)))
                {
                    bool isConquer = enemyPiece != null;
                    AddPotentialMoveToList(potentialMoves, posx, posy, isConquer);
                }
            }
        }

        return potentialMoves;
    }

    public bool IsPieceInGrid(int x, int y)
    {
        bool result = false;
        if (InGridRange(x, y))
        {
            result = ChessBoardGrid.grid[x, y].ChessPiece != null;
        }

        //print("move check x and y are:" + x + ", " + y + ": " + result.ToString());
        return result;
    }

    public bool IsEnemyPieceInGrid(int x, int y, out ChessPiece enemy)
    {
        bool result = false;
        enemy = null;
        if (InGridRange(x, y))
        {
            if (ChessBoardGrid.grid[(int)x, (int)y].ChessPiece != null)
            {
                var enemyPiece = ChessBoardGrid.grid[x, y].ChessPiece.GetComponent<ChessPiece>();
                if ((enemyPiece.IsWhiteTeam && !isWhitePlayerTurn) || (!enemyPiece.IsWhiteTeam && isWhitePlayerTurn))
                {
                    enemy = enemyPiece;
                    result = true;
                }
            }
        }
        //print("enemy check x and y are:" + x + ", " + y + ": " + result.ToString());
        return result;
    }

    private bool InGridRange(int x, int y)
    {
        return (x < 8 && x >= 0 && y < 8 && y >= 0);
    }

    #endregion
}

public enum ChessPieceEnum
{
    Pawn = 0,
    Rook = 1,
    Bishop = 2,
    Knight = 3,
    Queen = 4,
    King = 5
}
