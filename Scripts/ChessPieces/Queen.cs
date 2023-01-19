using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        for (int i = currentY - 1; i >= 0; i--)
        {
            if (board[currentX, i] == null) r.Add(new Vector2Int(currentX, i));
            else
            {
                if (board[currentX, i].team != this.team) r.Add(new Vector2Int(currentX, i));
                break;
            }
        }
        for (int i = currentY + 1; i < 8; i++)
        {
            if (board[currentX, i] == null) r.Add(new Vector2Int(currentX, i));
            else
            {
                if (board[currentX, i].team != this.team) r.Add(new Vector2Int(currentX, i));
                break;
            }
        }
        //left & right
        for (int i = currentX - 1; i >= 0; i--)
        {
            if (board[i, currentY] == null) r.Add(new Vector2Int(i, currentY));
            else
            {
                if (board[i, currentY].team != this.team) r.Add(new Vector2Int(i, currentY));
                break;
            }
        }
        for (int i = currentX + 1; i < 8; i++)
        {
            if (board[i, currentY] == null) r.Add(new Vector2Int(i, currentY));
            else
            {
                if (board[i, currentY].team != this.team) r.Add(new Vector2Int(i, currentY));
                break;
            }
        }

        for (int i = currentX + 1, j = currentY + 1; i < 8 && j < 8; j++, i++)
        {
            if (board[i, j] != null && board[i, j].team == this.team) break;

            if (board[i, j] == null) r.Add(new Vector2Int(i, j));
            else if (board[i, j] != null && board[i, j].team != this.team)
            {
                r.Add(new Vector2Int(i, j));
                break;
            }

        }
        for (int i = currentX + 1, j = currentY - 1; i < 8 && j >= 0; j--, i++)
        {
            if (board[i, j] != null && board[i, j].team == this.team) break;

            if (board[i, j] == null) r.Add(new Vector2Int(i, j));
            else if (board[i, j].team != this.team)
            {
                r.Add(new Vector2Int(i, j));
                break;
            }
        }
        for (int i = currentX - 1, j = currentY + 1; i >= 0 && j < 8; j++, i--)
        {
            if (board[i, j] != null && board[i, j].team == this.team) break;

            if (board[i, j] == null) r.Add(new Vector2Int(i, j));
            else if (board[i, j].team != this.team)
            {
                r.Add(new Vector2Int(i, j));
                break;
            }
        }
        for (int i = currentX - 1, j = currentY - 1; i >= 0 && j >= 0; j--, i--)
        {
            if (board[i, j] != null && board[i, j].team == this.team) break;

            if (board[i, j] == null) r.Add(new Vector2Int(i, j));
            else if (board[i, j].team != this.team)
            {
                r.Add(new Vector2Int(i, j));
                break;
            }
        }
        return r;
    }
}