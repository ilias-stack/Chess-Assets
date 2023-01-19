
using System;
using System.Collections.Generic;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] float yOffset = 0.2f;
    [SerializeField] Vector3 boardCenter = Vector3.zero;
    [Header("Prefabs & Mats")]
    [SerializeField] GameObject[] prefabs;
    [SerializeField] Material[] teamMaterials;
    [SerializeField] float deathSize = .3f, deathSpacing = .3f;
    [SerializeField] PauseButtonScript pbs;
    [SerializeField] AudioSource startSound, moveSound;

    private ChessPiece[,] chessPieces;
    private GameObject[,] tiles;
    ChessPiece whiteKing, blackKing;
    private Camera currentcamera;
    private GameObject currentHover = null;
    List<ChessPiece> deadWhites = new List<ChessPiece>(), deadBlacks = new List<ChessPiece>();
    ChessPiece currentlyDragging;
    Vector2Int hitPosition;
    GameObject highlitedTile;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    LayerMask previousLayer, BeforeHover;
    Vector3 bounds;
    GameObject previousTile;


    public bool isWhiteTurn;
    bool whiteCheck = false, blackCheck = false;
    private void Awake()
    {

        GenerateNewGame();
    }
    public void GenerateNewGame()
    {

        startSound.Play();
        isWhiteTurn = true;
        GenerateTiles(1, 8, 8);
        SpawnAllPieces();
        positionAllPieces();
    }

    private void Update()
    {
        if (!pbs.pauseMenuState)
            try
            {
                if (!currentcamera)
                {
                    currentcamera = Camera.main;
                    return;
                }
                RaycastHit info;
                Ray ray = currentcamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out info, 1000))
                {
                    if (currentHover != null)
                    {
                        previousLayer = BeforeHover;
                        previousTile = currentHover;
                    }
                    hitPosition = hoverPostion();
                    if ((info.transform.gameObject.layer == LayerMask.NameToLayer("Tile") || info.transform.gameObject.layer == LayerMask.NameToLayer("Highlight") ||
                     info.transform.gameObject.layer == LayerMask.NameToLayer("CheckWarn"))
                     && currentHover != null)
                    {
                        // Debug.Log(string.Format("original of {0} = {1}", previousTile.name, (int)previousLayer));

                        BeforeHover = info.transform.gameObject.layer;
                        currentHover.layer = availableMoves.Contains(hitPosition) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                        info.transform.gameObject.layer = LayerMask.NameToLayer("Hover");
                        currentHover = info.transform.gameObject;

                        if (previousLayer == LayerMask.NameToLayer("CheckWarn"))
                            previousTile.layer = previousLayer;

                    }
                    if (currentHover == null)
                    {
                        currentHover = info.transform.gameObject;

                    }


                    if (Input.GetMouseButtonDown(0))
                    {
                        if (chessPieces[hitPosition.x, hitPosition.y] != null)
                        {
                            //our turn ?
                            if ((chessPieces[hitPosition.x, hitPosition.y].team == ChessTeam.White && isWhiteTurn) || (chessPieces[hitPosition.x, hitPosition.y].team == ChessTeam.Black && !isWhiteTurn))
                            {
                                currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];
                                availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, ref tiles);
                                HighlightTiles();
                            }
                        }
                    }

                    if (currentlyDragging != null && Input.GetMouseButtonUp(0))
                    {
                        Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);
                        bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
                        if (!validMove)
                        {
                            currentlyDragging.SetPosition(new Vector3(previousPosition.x, yOffset, previousPosition.y) - bounds + new Vector3(.5f, 0, .5f));
                        }
                        HighlightTiles(true);
                        currentlyDragging = null;
                    }
                }
                else
                {
                    if (currentHover != null)
                    {
                        currentHover.layer = availableMoves.Contains(hitPosition) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                    }
                    if (currentlyDragging && Input.GetMouseButtonUp(0))
                    {
                        currentlyDragging.SetPosition(new Vector3(currentlyDragging.currentX, yOffset, currentlyDragging.currentY) - bounds + new Vector3(.5f, 0, .5f));

                        HighlightTiles(true);
                        currentlyDragging = null;
                    }
                }

                if (currentlyDragging)
                {
                    Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
                    float distance = 0;
                    if (horizontalPlane.Raycast(ray, out distance)) currentlyDragging.SetPosition(ray.GetPoint(distance) + 1.5f * Vector3.up);
                }


            }
            catch (Exception)
            {
                // Debug.Log(e.ToString());
                return;
            }
    }

    private void HighlightTiles(bool c = false)
    {
        if (!c)
        {
            for (int i = 0; i < availableMoves.Count; i++)
                tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
        }
        else
        {
            for (int i = 0; i < availableMoves.Count; i++)
                tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");
            availableMoves.Clear();
        }
    }

    private bool MoveTo(ChessPiece cp, int x, int y)
    {
        Checks();
        if (availableMoves.Contains(new Vector2Int(x, y)))
        {
            Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);
            if (chessPieces[x, y] != null)
            {
                ChessPiece ocp = chessPieces[x, y];
                if (cp.team == ocp.team)
                    return false;

                if (ocp.team == ChessTeam.White)
                {
                    deadWhites.Add(ocp);
                    ocp.SetPosition(new Vector3(8, yOffset, -1) -
                    bounds + new Vector3(.5f, 0, 0.5f) +
                    Vector3.forward * deathSpacing * deadWhites.Count);
                }
                else
                {
                    deadBlacks.Add(ocp);
                    ocp.SetPosition(new Vector3(-1, yOffset, 8) -
                    bounds + new Vector3(.5f, 0, 0.5f) +
                    Vector3.back * deathSpacing * deadBlacks.Count);
                }
                ocp.SetScale(Vector3.one * deathSize);
            }
            chessPieces[x, y] = cp;
            chessPieces[previousPosition.x, previousPosition.y] = null;
            positionSinglePiece(x, y);
            isWhiteTurn = !isWhiteTurn;
            PromotionCheck();
            whiteCheck = whiteKing.GetComponent<King>().IsUnderCheck(ref chessPieces, ref tiles);
            blackCheck = blackKing.GetComponent<King>().IsUnderCheck(ref chessPieces, ref tiles);
            // Debug.Log("Executed NOW");
            Checks();
            moveSound.Play();
            return true;
        }
        return false;
    }

    private void GenerateTiles(float tileSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + boardCenter;
        tiles = new GameObject[tileCountX, tileCountY];
        for (int i = 0; i < tileCountX; i++)
            for (int j = 0; j < tileCountY; j++)
                tiles[i, j] = GenerateSingleTile(tileSize, i, j);
    }
    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;
        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;


        Vector3[] verticies = new Vector3[4];
        verticies[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        verticies[1] = new Vector3(x * tileSize, yOffset, (y + 1) * tileSize) - bounds;
        verticies[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;
        verticies[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };
        mesh.vertices = verticies;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        tileObject.layer = LayerMask.NameToLayer("Tile");

        tileObject.AddComponent<BoxCollider>();




        return tileObject;
    }

    void PromotionCheck()
    {
        for (int i = 0; i < 8; i++)
        {
            if (chessPieces[i, 7] != null)
                if (chessPieces[i, 7].type == ChessPieceType.Pawn && chessPieces[i, 7].team == ChessTeam.White)
                {
                    Destroy(chessPieces[i, 7].gameObject);
                    chessPieces[i, 7] = SpawnSinglePiece(ChessPieceType.Queen, ChessTeam.White);
                    positionSinglePiece(i, 7);
                }
        }
        for (int i = 0; i < 8; i++)
        {
            if (chessPieces[i, 0] != null)

                if (chessPieces[i, 0].type == ChessPieceType.Pawn && chessPieces[i, 0].team == ChessTeam.Black)
                {
                    Destroy(chessPieces[i, 0].gameObject);
                    chessPieces[i, 0] = SpawnSinglePiece(ChessPieceType.Queen, ChessTeam.Black);
                    positionSinglePiece(i, 0);
                }
        }
    }

    private void SpawnAllPieces()
    {
        chessPieces = new ChessPiece[8, 8];
        //whites
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Rook, ChessTeam.White);
        chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Knight, ChessTeam.White);
        chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, ChessTeam.White);
        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Queen, ChessTeam.White);
        whiteKing = chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.King, ChessTeam.White);
        chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Rook, ChessTeam.White);
        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Knight, ChessTeam.White);
        chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, ChessTeam.White);
        for (int i = 0; i < 8; i++)
            chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, ChessTeam.White);
        //niggas
        chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Rook, ChessTeam.Black);
        chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Knight, ChessTeam.Black);
        chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, ChessTeam.Black);
        chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Queen, ChessTeam.Black);
        blackKing = chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.King, ChessTeam.Black);
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Rook, ChessTeam.Black);
        chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Knight, ChessTeam.Black);
        chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Bishop, ChessTeam.Black);
        for (int i = 0; i < 8; i++)
            chessPieces[i, 6] = SpawnSinglePiece(ChessPieceType.Pawn, ChessTeam.Black);

    }

    void positionAllPieces()
    {
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if (chessPieces[x, y] != null)
                    positionSinglePiece(x, y, true);
            }
    }

    void Checks()
    {
        // Debug.Log(string.Format("w:{0}  b:{1}", whiteCheck, blackCheck));
        if (!whiteCheck)
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i != blackKing.currentX && j != blackKing.currentY) tiles[i, j].layer = LayerMask.NameToLayer("Tile");
                    else if (i == blackKing.currentX && j == blackKing.currentY) if (!blackCheck) tiles[i, j].layer = LayerMask.NameToLayer("Tile");
                }
            }
        if (!blackCheck)
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i != whiteKing.currentX && j != whiteKing.currentY) tiles[i, j].layer = LayerMask.NameToLayer("Tile");
                    else if (i == whiteKing.currentX && j == whiteKing.currentY) if (!whiteCheck) tiles[i, j].layer = LayerMask.NameToLayer("Tile");

                }
            }
        if (whiteCheck) tiles[whiteKing.currentX, whiteKing.currentY].layer = LayerMask.NameToLayer("CheckWarn");
        if (blackCheck) tiles[blackKing.currentX, blackKing.currentY].layer = LayerMask.NameToLayer("CheckWarn");

    }
    void positionSinglePiece(int x, int y, bool force = false)
    {
        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentY = y;
        if (chessPieces[x, y].team == ChessTeam.Black) chessPieces[x, y].transform.Rotate(0, 180, 0);
        chessPieces[x, y].SetPosition(new Vector3(x, yOffset, y) - bounds + new Vector3(.5f, 0, .5f), force);
    }

    ChessPiece SpawnSinglePiece(ChessPieceType type, ChessTeam team)
    {
        ChessPiece cp = Instantiate(prefabs[(int)type], transform).GetComponent<ChessPiece>();
        cp.type = type;
        cp.team = team;
        cp.GetComponent<MeshRenderer>().material = teamMaterials[(int)team];
        return cp;
    }

    private Vector2Int hoverPostion()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (tiles[i, j].layer == LayerMask.NameToLayer("Hover")) return new Vector2Int(i, j);

        return -Vector2Int.one;
    }

}
