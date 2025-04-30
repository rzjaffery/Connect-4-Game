
/// <summary>
/// Class for board utilitaries
/// </summary>
public class BoardUtils
{
    //board dimensions
    public static readonly int NUM_ROWS = 6;
    public static readonly int NUM_COLS = 7;

    //matrix for evaluatiing a board position
    private static int[,] evaluationBoard = new int[,]{
        {3, 4, 5, 7, 5, 4, 3},
        {4, 6, 8, 10, 8, 6, 4},
        {5, 8, 11, 13, 11, 8, 5},
        {5, 8, 11, 13, 11, 8, 5},
        {4, 6, 8, 10, 8, 6, 4},
        {3, 4, 5, 7, 5, 4, 3}
    };

    /// <summary>
    /// Function for getting the deault  empty board
    /// </summary>
    /// <returns>the default empty board for Connect4</returns>
    public static Tile[,] GetDefaultBoard()
    {
        Tile[,] board = new Tile[NUM_ROWS, NUM_COLS];
        for (int i = 0; i < NUM_ROWS; i++)
        {
            for (int j = 0; j < NUM_COLS; j++)
            {
                board[i, j] = Tile.EMPTY;
            }
        }

        return board;
    }
    private static double EvaluateWindow(Board board, int startRow, int startCol, int deltaRow, int deltaCol, PlayerAlliance alliance)
    {
        int myTiles = 0;
        int oppTiles = 0;
        int empty = 0;

        Tile myTile = GetTileFromAlliance(alliance);
        Tile oppTile = GetTileFromAlliance(GetOpponentAlliance(alliance));

        for (int i = 0; i < 4; i++)
        {
            int r = startRow + i * deltaRow;
            int c = startCol + i * deltaCol;
            Tile tile = board.Table[r, c];

            if (tile == myTile) myTiles++;
            else if (tile == oppTile) oppTiles++;
            else empty++;
        }

        // Scoring logic
        if (myTiles == 4) return 100;
        if (myTiles == 3 && empty == 1) return 10;
        if (myTiles == 2 && empty == 2) return 5;

        if (oppTiles == 3 && empty == 1) return -80;
        if (oppTiles == 2 && empty == 2) return -4;

        return 0;
    }

    private static Tile GetTileFromAlliance(PlayerAlliance alliance)
    {
        return alliance == PlayerAlliance.RED ? Tile.RED : Tile.BLACK;
    }

    private static PlayerAlliance GetOpponentAlliance(PlayerAlliance alliance)
    {
        return alliance == PlayerAlliance.RED ? PlayerAlliance.BLACK : PlayerAlliance.RED;
    }


    /// <summary>
    /// Function to evaluate a board from the given player perspective
    /// </summary>
    /// <param name="board"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public static double EvaluateBoard(Board board, PlayerAlliance alliance)
    {
        //if (Connect4Utils.HasWinner(board, alliance))
        //    return 100000.0;
        //if (Connect4Utils.HasWinner(board, GetOpponentAlliance(alliance)))
        //    return -100000.0;

        double score = 0;

        // Favor center column
        int centerCol = NUM_COLS / 2;
        for (int i = 0; i < NUM_ROWS; i++)
        {
            if (board.Table[i, centerCol] == GetTileFromAlliance(alliance))
                score += 6;
        }

        // Evaluate windows (4-tile patterns)
        for (int row = 0; row < NUM_ROWS; row++)
        {
            for (int col = 0; col < NUM_COLS - 3; col++)
            {
                score += EvaluateWindow(board, row, col, 0, 1, alliance); // Horizontal
            }
        }

        for (int row = 0; row < NUM_ROWS - 3; row++)
        {
            for (int col = 0; col < NUM_COLS; col++)
            {
                score += EvaluateWindow(board, row, col, 1, 0, alliance); // Vertical
            }
        }

        for (int row = 0; row < NUM_ROWS - 3; row++)
        {
            for (int col = 0; col < NUM_COLS - 3; col++)
            {
                score += EvaluateWindow(board, row, col, 1, 1, alliance); // Diagonal right
            }
        }

        for (int row = 3; row < NUM_ROWS; row++)
        {
            for (int col = 0; col < NUM_COLS - 3; col++)
            {
                score += EvaluateWindow(board, row, col, -1, 1, alliance); // Diagonal left
            }
        }

        return score;
    }

}