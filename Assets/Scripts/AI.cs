using System.Collections.Generic;
using System;
using System.Threading;

public static class AI
{
    public static int intelligence = 1;
    public static Thread thread;

    static void ResetValues(Tile[,] tiles, Tile.TileValue[,] values)
    {
        int size = GameMaster.size;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                tiles[x, y].Value = values[x, y];
            }
        }
    }

    static Tile.TileValue[,] GetValues(Tile[,] tiles)
    {
        int size = GameMaster.size;

        Tile.TileValue[,] values = new Tile.TileValue[size, size];
        
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                values[x, y] = tiles[x, y].Value;
            }
        }

        return values;
    }

    struct Pair
    {
        public Tile tile;
        public int moves;
        public bool ourOwn;

        public Pair(Tile tile, int moves, bool ourOwn)
        {
            this.tile = tile;
            this.moves = moves;
            this.ourOwn = ourOwn;
        }
    }

    static List<Pair> SimulateMoveForChosen(Tile[,] tiles, Side side, int index = 0)
    {
        List<Pair> best = new List<Pair>();
        Tile.TileValue ourValue = side == Side.Circle ? Tile.TileValue.Circle : Tile.TileValue.Cross;
        Tile.TileValue enemyValue = ourValue == Tile.TileValue.Circle ? Tile.TileValue.Cross : Tile.TileValue.Circle;
        
        var values = GetValues(tiles);

        foreach (Tile tile in tiles)
        {
            if (tile.Opened) continue;

            tile.Value = ourValue;

            if (GameOverCheck.FullCheck(tile))
            {
                if (best.Count > 0 && !best[0].ourOwn) best.Clear();

                best.Add(new Pair(tile, index + 1, true));
                tile.Value = Tile.TileValue.Empty;
                continue;
            }

            tile.Value = enemyValue;

            if (GameOverCheck.FullCheck(tile) && (best.Count == 0 || !best[0].ourOwn))
            {
                best.Add(new Pair(tile, index + 1, false));
                tile.Value = Tile.TileValue.Empty;
                continue;
            }

            tile.Value = Tile.TileValue.Empty;
        }

        if(best.Count == 0 && index < intelligence)
        {
            foreach(Tile tile in tiles)
            {
                if (tile.Opened) continue;
                
                tile.Value = ourValue;

                foreach(Tile t in tiles)
                {
                    if (t.Opened) continue;
                    
                    var oldValues = GetValues(tiles);

                    t.Value = enemyValue;

                    var hardBest = SimulateMoveForChosen(tiles, side, index + 1);

                    if(hardBest.Count > 0)
                    {
                        if(best.Count > 0 && (best[0].moves > hardBest[0].moves && (!best[0].ourOwn && hardBest[0].ourOwn)))
                        {
                            best.Clear();
                        }

                        if(best.Count == 0 || (hardBest[0].moves == best[0].moves && (hardBest[0].ourOwn || !best[0].ourOwn)))
                        best.AddRange(hardBest);
                    }
                    
                    ResetValues(tiles, oldValues);
                }

                tile.Value = Tile.TileValue.Empty;
            }
        }

        ResetValues(tiles, values);

        return best;
    }

    public static Tile chosenTile = null;

    public static void MakeMove(Tile[,] tiles, Side side)
    {
        var start = new ParameterizedThreadStart(MakeMoveObject);
        thread = new Thread(start);

        chosenTile = null;

        thread.Start(new object[] { tiles, side });
    }

    static void MakeMoveObject(object o)
    {
        object[] arr = (object[])o;

        var tiles = (Tile[,])arr[0];
        var side = (Side)arr[1];

        MakeRealMove(tiles, side);
    }

    static void MakeRealMove(Tile[,] tiles, Side side)
    { 
        GameMaster.AITesting = true;

        var possibles = SimulateMoveForChosen(tiles, side);
        int size = possibles.Count;

        Random rand = new Random(DateTime.Now.Millisecond);

        Tile tile = null;
        if(size > 0)
        tile = possibles[rand.Next(possibles.Count)].tile;
        else
        {
            List<Tile> canUse = new List<Tile>();

            foreach (Tile t in tiles) if (!t.Opened) canUse.Add(t);

            tile = canUse[rand.Next(canUse.Count)];
        }

        GameMaster.AITesting = false;

        chosenTile = tile;
    }
}
