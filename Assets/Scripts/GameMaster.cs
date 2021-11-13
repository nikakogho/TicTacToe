using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Side { Cross, Circle }

public class GameMaster : MonoBehaviour
{
    public Sprite tileCross, tileCircle;

    public GameObject endUI;
    public Text endText;

    #region Statics

    public static bool AITesting = false;

    public static GameMaster instance;
    public Tile[,] tiles;

    public enum GameMode { OnePlayer, TwoPlayers }
    public static GameMode gameMode;
    
    public static Side side;
    public static Side player1Side;

    public static int size;
    public static int requiredMinimum;

    void Awake()
    {
        instance = this;

        gameMode = (GameMode)(PlayerPrefs.GetInt("Mode", 1) - 1);
        player1Side = (Side)PlayerPrefs.GetInt("Player One Side", 0);
        side = Side.Cross;
        size = PlayerPrefs.GetInt("Size", 3);
        requiredMinimum = PlayerPrefs.GetInt("Required Minimum", 3);

        AI.intelligence = PlayerPrefs.GetInt("Intelligence", 1);
        AI.chosenTile = null;
    }

    void Start()
    {
        GameOverCheck.Init(tiles, size, requiredMinimum);

        if (gameMode == GameMode.OnePlayer && side != player1Side)
        {
            AIMove();
        }
    }

    #endregion
    
    #region Game Over

    void Draw()
    {
        AlterText("DRAW!", Color.grey);
    }

    void Win()
    {
        AlterText("YOU Won!", Color.green);
    }

    void Lose()
    {
        AlterText("You LOSE!", Color.red);
    }

    void PlayerWin(int index)
    {
        AlterText(PlayerPrefs.GetString("Player " + index + " Name", "Player" + index) + " Won!", Color.green);
    }

    void AlterText(string text, Color color)
    {
        endText.text = text;
        endText.color = color;
    }

    #endregion

    public bool gameOver = false;

    public void MoveMade(Tile tile)
    {
        if (GameOverCheck.FullCheck(tile))
        {
            endUI.SetActive(true);

            if(side == player1Side)
            {
                if (gameMode == GameMode.OnePlayer) Win();
                else PlayerWin(1);
            }
            else
            {
                if (gameMode == GameMode.OnePlayer) Lose();
                else PlayerWin(2);
            }

            return;
        }

        gameOver = GameOverCheck.DrawCheck();

        if (gameOver)
        {
            endUI.SetActive(true);
            Draw();

            return;
        }

        side = side == Side.Circle ? Side.Cross : Side.Circle;

        if(gameMode == GameMode.OnePlayer && side != player1Side)
        {
            AIMove();
        }
    }

    IEnumerator ExitGame()
    {
        if (AITesting) AI.thread.Abort();

        yield return new WaitForSeconds(0.5f);

        GetComponent<LevelMethods>().Play("main");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitGame());
            return;
        }

        if(AI.chosenTile != null)
        {
            AI.chosenTile.Click();
            AI.chosenTile = null;

            foreach (Tile t in tiles) t.button.interactable = t.Value == Tile.TileValue.Empty;
        }
    }

    IEnumerator AIMoveRoutine()
    {
        foreach (Tile tile in tiles) tile.button.interactable = false;

        yield return new WaitForSeconds(0.4f);

        AI.MakeMove(tiles, side);
    }

    void AIMove()
    {
        StartCoroutine(AIMoveRoutine());
    }
}
