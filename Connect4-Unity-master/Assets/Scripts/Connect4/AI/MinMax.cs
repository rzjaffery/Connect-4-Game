using System;
using UnityEngine;

public class MinMax : AI
{
    private int depth = 4;

    public MinMax(int depth)
    {
        this.depth = depth;
    }

    public int GetBestMove(Player currentPlayer, Board board)
    {
        int bestMove = -1;
        double bestScore = double.NegativeInfinity;

        foreach (int move in board.GetValidMoves())
        {
            Board simulatedBoard = new Board(board.Table);
            simulatedBoard.SetPiece(move, currentPlayer.Alliance);

            double score = Minimax(simulatedBoard, depth - 1, false, currentPlayer.Alliance);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private double Minimax(Board board, int depth, bool isMaximizing, PlayerAlliance aiAlliance)
    {
        if (depth == 0 || Connect4Utils.Finished(board))
        {
            return BoardUtils.EvaluateBoard(board, aiAlliance);
        }

        double bestScore;
        PlayerAlliance currentTurnAlliance = isMaximizing ? aiAlliance : GetOpponentAlliance(aiAlliance);

        if (isMaximizing)
        {
            bestScore = double.NegativeInfinity;
            foreach (int move in board.GetValidMoves())
            {
                Board newBoard = new Board(board.Table);
                newBoard.SetPiece(move, currentTurnAlliance);
                double score = Minimax(newBoard, depth - 1, false, aiAlliance);
                bestScore = Math.Max(bestScore, score);
            }
        }
        else
        {
            bestScore = double.PositiveInfinity;
            foreach (int move in board.GetValidMoves())
            {
                Board newBoard = new Board(board.Table);
                newBoard.SetPiece(move, currentTurnAlliance);
                double score = Minimax(newBoard, depth - 1, true, aiAlliance);
                bestScore = Math.Min(bestScore, score);
            }
        }

        return bestScore;
    }

    private PlayerAlliance GetOpponentAlliance(PlayerAlliance alliance)
    {
        return alliance == PlayerAlliance.RED ? PlayerAlliance.BLACK : PlayerAlliance.RED;
    }
}
