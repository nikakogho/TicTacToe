public static class GameOverCheck 
{
    static Tile[,] tiles;
    static int size;
    static int requiredMinimum;

    public static void Init(Tile[,] tiles, int size, int requiredMinimum)
    {
        GameOverCheck.tiles = tiles;
        GameOverCheck.size = size;
        GameOverCheck.requiredMinimum = requiredMinimum;
    }

    #region Game Over Checks

    static bool HorizontalCheck(Tile tile)
    {
        for (int x = tile.x - requiredMinimum; x <= tile.x; x++)
        {
            if (x < 0 || x + requiredMinimum > size) continue;

            bool working = true;

            for (int X = x; X < x + requiredMinimum; X++)
            {
                if (tiles[X, tile.y].Value != tile.Value)
                {
                    working = false;
                    break;
                }
            }

            if (working) return true;
        }

        return false;
    }

    static bool VerticalCheck(Tile tile)
    {
        for (int y = tile.y - requiredMinimum; y <= tile.y; y++)
        {
            if (y < 0 || y + requiredMinimum > size) continue;

            bool working = true;

            for (int Y = y; Y < y + requiredMinimum; Y++)
            {
                if (tiles[tile.x, Y].Value != tile.Value)
                {
                    working = false;
                    break;
                }
            }

            if (working) return true;
        }

        return false;
    }

    static void IncreaseXandY(ref int x, ref int y, int changeYBy = 1)
    {
        x++;
        y += changeYBy;
    }

    #region Diagonal Check

    static bool Diagonal1Check(Tile tile)
    {
        for (int x = tile.x - requiredMinimum, y = tile.y - requiredMinimum; x <= tile.x; IncreaseXandY(ref x, ref y))
        {
            if (y < 0 || x < 0 || y + requiredMinimum > size || x + requiredMinimum > size) continue;

            bool working = true;

            for (int X = x, Y = y; X < x + requiredMinimum; IncreaseXandY(ref X, ref Y))
            {
                if (tiles[X, Y].Value != tile.Value)
                {
                    working = false;
                    break;
                }
            }

            if (working) return true;
        }

        return false;
    }

    static bool Diagonal2Check(Tile tile)
    {
        for (int x = tile.x - requiredMinimum, y = tile.y + requiredMinimum; x <= tile.x; IncreaseXandY(ref x, ref y, -1))
        {
            if (x < 0 || y < requiredMinimum - 1 || y >= size || x + requiredMinimum > size) continue;

            bool working = true;

            for (int X = x, Y = y; X < x + requiredMinimum; IncreaseXandY(ref X, ref Y, -1))
            {
                if (tiles[X, Y].Value != tile.Value)
                {
                    working = false;
                    break;
                }
            }

            if (working) return true;
        }

        return false;
    }

    static bool DiagonalCheck(Tile tile)
    {
        return Diagonal1Check(tile) || Diagonal2Check(tile);
    }

    #endregion

    #endregion

    public static bool FullCheck(Tile tile)
    {
        return tile.Opened && (HorizontalCheck(tile) || VerticalCheck(tile) || DiagonalCheck(tile));
    }

    public static bool DrawCheck()
    {
        foreach (Tile tile in tiles) if (!tile.Opened) return false;

        return true;
    }
}
