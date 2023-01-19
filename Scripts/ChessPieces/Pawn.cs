using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        int direction = team == ChessTeam.White ? 1 : -1;
        if (board[currentX, currentY + direction] == null)
        {
            r.Add(new Vector2Int(currentX, currentY + direction));
            if (team == ChessTeam.White && currentY == 1 && !board[currentX, currentY + direction * 2] || team == ChessTeam.Black && currentY == 6 && !board[currentX, currentY + direction * 2])
            {
                r.Add(new Vector2Int(currentX, currentY + direction * 2));
            }

        }
        try
        {
            if (board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != this.team)
                r.Add(new Vector2Int(currentX + 1, currentY + direction));
        }
        catch (IndexOutOfRangeException)
        {
        }
        try
        {
            if (board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != this.team)
                r.Add(new Vector2Int(currentX - 1, currentY + direction));
        }
        catch (IndexOutOfRangeException)
        {
        }



        return r;
    }

    public List<Vector2Int> PawnAttack()
    {
        List<Vector2Int> r = new List<Vector2Int>();
        int direction = team == ChessTeam.White ? 1 : -1;
        try
        {
            r.Add(new Vector2Int(currentX - 1, currentY + direction));
        }
        catch (IndexOutOfRangeException)
        {
        }
        try
        {
            r.Add(new Vector2Int(currentX + 1, currentY + direction));
        }
        catch (IndexOutOfRangeException)
        {
        }

        return r;
    }
}