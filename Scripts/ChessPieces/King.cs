using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //right
        if (currentX + 1 < 8)
        {
            if (board[currentX + 1, currentY] == null) r.Add(new Vector2Int(currentX + 1, currentY));
            else if (board[currentX + 1, currentY].team != this.team) r.Add(new Vector2Int(currentX + 1, currentY));

            //top
            if (currentY + 1 < 8)
                if (board[currentX + 1, currentY + 1] == null) r.Add(new Vector2Int(currentX + 1, currentY + 1));
                else if (board[currentX + 1, currentY + 1].team != this.team) r.Add(new Vector2Int(currentX + 1, currentY + 1));

            //bot
            if (currentY - 1 >= 0)
                if (board[currentX + 1, currentY - 1] == null) r.Add(new Vector2Int(currentX + 1, currentY - 1));
                else if (board[currentX + 1, currentY - 1].team != this.team) r.Add(new Vector2Int(currentX + 1, currentY - 1));

        }

        //left
        if (currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY] == null) r.Add(new Vector2Int(currentX - 1, currentY));
            else if (board[currentX - 1, currentY].team != this.team) r.Add(new Vector2Int(currentX - 1, currentY));

            //top
            if (currentY + 1 < 8)
                if (board[currentX - 1, currentY + 1] == null) r.Add(new Vector2Int(currentX - 1, currentY + 1));
                else if (board[currentX - 1, currentY + 1].team != this.team) r.Add(new Vector2Int(currentX - 1, currentY + 1));

            //bot
            if (currentY - 1 >= 0)
                if (board[currentX - 1, currentY - 1] == null) r.Add(new Vector2Int(currentX - 1, currentY - 1));
                else if (board[currentX - 1, currentY - 1].team != this.team) r.Add(new Vector2Int(currentX - 1, currentY - 1));

        }

        //top
        if (currentY + 1 < 8)
            if (board[currentX, currentY + 1] == null || board[currentX, currentY + 1].team != this.team) r.Add(new Vector2Int(currentX, currentY + 1));

        //bot
        if (currentY - 1 >= 0)
            if (board[currentX, currentY - 1] == null || board[currentX, currentY - 1].team != this.team) r.Add(new Vector2Int(currentX, currentY - 1));

        CheckMateCheck(ref board, ref tiles, new Vector2Int(currentX, currentY), ref r);
        return r;
    }

    private bool CheckMateCheck(ref ChessPiece[,] board, ref GameObject[,] tiles, Vector2Int target, ref List<Vector2Int> r)
    {
        bool output = false;
        if (this.team == ChessTeam.Black)
        {
            List<Vector2Int> whiteMoves = new List<Vector2Int>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].team == ChessTeam.White && board[i, j].type != ChessPieceType.Pawn && board[i, j].type != ChessPieceType.King)
                    {
                        List<Vector2Int> sub = board[i, j].GetAvailableMoves(ref board, ref tiles);
                        foreach (Vector2Int item in sub)
                            whiteMoves.Add(item);
                        sub.Clear();
                    }
                    else if (board[i, j] != null && board[i, j].team == ChessTeam.White && board[i, j].type == ChessPieceType.Pawn && board[i, j].type != ChessPieceType.King)
                    {
                        List<Vector2Int> sub = board[i, j].GetComponent<Pawn>().PawnAttack();
                        foreach (Vector2Int item in sub)
                            whiteMoves.Add(item);
                        sub.Clear();
                    }
                }
            foreach (var item in whiteMoves)
            {
                if (item.x == target.x && item.y == target.y)
                {
                    output = true;
                    Debug.Log(string.Format("Black King : {0}", item));
                }
                r.Remove(item);
            }
        }

        else if (this.team == ChessTeam.White)
        {
            List<Vector2Int> blackMoves = new List<Vector2Int>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].team == ChessTeam.Black && board[i, j].type != ChessPieceType.Pawn && board[i, j].type != ChessPieceType.King)
                    {
                        List<Vector2Int> sub = board[i, j].GetAvailableMoves(ref board, ref tiles);
                        foreach (Vector2Int item in sub)
                            blackMoves.Add(item);
                        sub.Clear();
                    }
                    else if (board[i, j] != null && board[i, j].team == ChessTeam.Black && board[i, j].type == ChessPieceType.Pawn && board[i, j].type != ChessPieceType.King)
                    {
                        List<Vector2Int> sub = board[i, j].GetComponent<Pawn>().PawnAttack();
                        foreach (Vector2Int item in sub)
                            blackMoves.Add(item);
                        sub.Clear();
                    }
                }
            foreach (var item in blackMoves)
            {
                if (item.x == target.x && item.y == target.y)
                {
                    output = true;
                    Debug.Log(string.Format("White King : {0}", item));
                }
                r.Remove(item);
            }
        }
        return output;
    }

    public bool IsUnderCheck(ref ChessPiece[,] board, ref GameObject[,] tiles)
    {
        if (this.team == ChessTeam.Black)
        {
            List<Vector2Int> whiteMoves = new List<Vector2Int>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].team == ChessTeam.White && board[i, j].type != ChessPieceType.King)
                    {
                        List<Vector2Int> sub = board[i, j].GetAvailableMoves(ref board, ref tiles);
                        foreach (Vector2Int item in sub)
                            whiteMoves.Add(item);
                        sub.Clear();
                    }
                }
            foreach (var item in whiteMoves)
            {
                if (item.x == currentX && item.y == currentY)
                    return true;
            }
        }

        else if (this.team == ChessTeam.White)
        {
            List<Vector2Int> blackMoves = new List<Vector2Int>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].team == ChessTeam.Black && board[i, j].type != ChessPieceType.King)
                    {
                        List<Vector2Int> sub = board[i, j].GetAvailableMoves(ref board, ref tiles);
                        foreach (Vector2Int item in sub)
                            blackMoves.Add(item);
                        sub.Clear();
                    }
                }
            foreach (var item in blackMoves)
            {
                if (item.x == currentX && item.y == currentY)
                    return true;
            }
        }
        return false;
    }
}