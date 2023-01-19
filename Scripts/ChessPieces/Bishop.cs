using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bishop : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {
        List<Vector2Int> r = new List<Vector2Int>();

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