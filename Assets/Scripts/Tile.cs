using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Image valueImage;
    public Button button;

    public int x;
    public int y;

    public enum TileValue { Empty, Cross, Circle }
    TileValue value;
    public TileValue Value { get { return value; } set { this.value = value; OnValueChanged(); } }

    public bool Opened { get { return value != TileValue.Empty; } }
    
    GameMaster master;

    void OnValueChanged()
    {
        if (GameMaster.AITesting) return;

        valueImage.enabled = true;

        switch (value)
        {
            case TileValue.Empty:
                valueImage.enabled = false;
                break;
            case TileValue.Circle:
                valueImage.sprite = master.tileCircle;
                break;
            case TileValue.Cross:
                valueImage.sprite = master.tileCross;
                break;
        }
    }

    void Start()
    {
        master = GameMaster.instance;

        Value = TileValue.Empty;
    }

    public void Click()
    {
        Value = GameMaster.side == Side.Circle ? TileValue.Circle : TileValue.Cross;
        
        button.interactable = false;

        master.MoveMade(this);
    }
}
