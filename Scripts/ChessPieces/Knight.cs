using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        int x = currentX + 1, y = currentY + 2;
        if (x < 8 && y < 8) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));

        x = currentX + 2;
        y = currentY + 1;
        if (x < 8 && y < 8) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));

        x = currentX - 1;
        y = currentY + 2;
        if (x >= 0 && y < 8) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));

        x = currentX - 2;
        y = currentY + 1;
        if (x >= 0 && y < 8) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));


        x = currentX + 1;
        y = currentY - 2;
        if (x < 8 && y >= 0) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));

        x = currentX + 2;
        y = currentY - 1;
        if (x < 8 && y >= 0) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));

        x = currentX - 1;
        y = currentY - 2;
        if (x >= 0 && y >= 0) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));

        x = currentX - 2;
        y = currentY - 1;
        if (x >= 0 && y >= 0) if (board[x, y] == null || board[x, y].team != this.team) r.Add(new Vector2Int(x, y));



        return r;

    }
}