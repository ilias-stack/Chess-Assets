
using System.Collections.Generic;
using UnityEngine;


public enum ChessPieceType
{
    Pawn, Rook, Knight, Bishop, Queen, King
}

public enum ChessTeam
{
    White, Black
}

public class ChessPiece : MonoBehaviour
{
    public ChessPieceType type;
    public ChessTeam team;
    public int currentX, currentY;
    private void Start()
    {
        desiredScale = transform.localScale;
    }


    private Vector3 desiredPosition, desiredScale;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);

    }

    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {

        return new List<Vector2Int>();
    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;
        if (force) transform.position = desiredPosition;
    }

    public virtual void SetScale(Vector3 scale, bool force = false)
    {
        desiredScale = scale;
        if (force) transform.localScale = desiredScale;
    }

}