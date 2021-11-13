using UnityEngine;
using UnityEngine.UI;

public class LevelSetup : MonoBehaviour
{
    public string playSceneName = "playground";

    public Sprite selectedIcon, notSelectedIcon;
    public Image onePlayerModeImage, twoPlayerModeImage;
    public Image player1XImage, player1OImage;

    public Slider sizeSlider;
    public Text sizeText;

    public Slider minSlider;
    public Text minText;

    public Slider intelligenceSlider;
    public Text intelligenceText;

    public InputField player1Name, player2Name;

    int selectedMode;
    int player1Side;

    int Size { get { return (int)sizeSlider.value; } set { sizeSlider.value = value; SizeChanged(); } }
    int MinSize { get { return (int)minSlider.value; } set { minSlider.value = value; MinSizeChanged(); } }
    int Intelligence { get { return (int)intelligenceSlider.value; } set { intelligenceSlider.value = value; IntelligenceChanged(); } }

    void OnEnable()
    {
        SelectPlayerMode(1);
        SelectPlayer1Side(0);

        Size = 3;
        MinSize = 3;
    }

    public void SelectPlayerMode(int mode)
    {
        selectedMode = mode;

        onePlayerModeImage.sprite = mode == 1 ? selectedIcon : notSelectedIcon;
        twoPlayerModeImage.sprite = mode == 2 ? selectedIcon : notSelectedIcon;
    }

    public void SelectPlayer1Side(int side)
    {
        player1Side = side;
        
        player1XImage.sprite = side == 0 ? selectedIcon : notSelectedIcon;
        player1OImage.sprite = side == 1 ? selectedIcon : notSelectedIcon;
    }

    public void SizeChanged()
    {
        sizeText.text = "Size: " + Size;

        if (MinSize > Size) MinSize = Size;
        minSlider.maxValue = Size;
    }

    public void MinSizeChanged()
    {
        minText.text = "Wins : " + MinSize;
    }

    public void IntelligenceChanged()
    {
        intelligenceText.text = "Intelect : " + Intelligence;
    }
    
    public void Play()
    {
        PlayerPrefs.SetInt("Size", Size);
        PlayerPrefs.SetInt("Required Minimum", MinSize);
        PlayerPrefs.SetInt("Intelligence", Intelligence);

        PlayerPrefs.SetInt("Mode", selectedMode);
        PlayerPrefs.SetInt("Player One Side", player1Side);

        PlayerPrefs.SetString("Player 1 Name", player1Name.text);
        PlayerPrefs.SetString("Player 2 Name", player2Name.text);

        LevelMethods.instance.Play(playSceneName);
    }
}
