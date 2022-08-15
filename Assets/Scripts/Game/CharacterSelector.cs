using System.Collections.Generic;
using Management;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
public class CharacterSelector : MonoBehaviour
{
    // Selected User Counter Text (TMP)
    public TextMeshProUGUI selectedUserCounterText;
    
    // Interactable Color Button
    public Button red;
    public Button green;
    public Button blue;
    public Button yellow;

    // Selected User Counter
    private static int _selectedUserCounter;
    private readonly int _selectedUserCounterMax = PlayerCountSetting.PlayerCount;
    
    // UserInfo <SelectionOrder(int), List<string>(new []{ColorName, ColorHex, Code)>
    public static Dictionary<int, List<string>> UserInfo = new ();

    private void Start()
    {
        _selectedUserCounter = 0;
        UserInfo = new Dictionary<int, List<string>>();
        selectedUserCounterText.text = $"{_selectedUserCounter + 1}/{_selectedUserCounterMax}";
    }
    
    // Check Final Selection
    private void CheckFinalSelection()
    {
        if (_selectedUserCounter != _selectedUserCounterMax)
        {
            selectedUserCounterText.text = $"{_selectedUserCounter + 1}/{_selectedUserCounterMax}";
        }
        else
        {
            // Init //
            BoardGame.RedTokenCount = HorseCountSetting.HorseCount;
            BoardGame.GreenTokenCount = HorseCountSetting.HorseCount;
            BoardGame.BlueTokenCount = HorseCountSetting.HorseCount;
            BoardGame.YellowTokenCount = HorseCountSetting.HorseCount;
            BoardGame.NRedTokenCount = HorseCountSetting.HorseCount;
            BoardGame.NGreenTokenCount = HorseCountSetting.HorseCount;
            BoardGame.NBlueTokenCount = HorseCountSetting.HorseCount;
            BoardGame.NYellowTokenCount = HorseCountSetting.HorseCount;
            
            BoardGame.DoubleChance = false;
            BoardGame.ThrewYut = false;
            BoardGame.ShowedValue = false;
            BoardGame.DroppedYut = false;
            BoardGame.MoveCount = 0;
            OverLap.Finished = false;
            HorseMovement.MoveEnabled = false;
            // Init //
            
            SceneAsyncLoadManager.SetSceneName("BoardGame");
            SceneManager.LoadScene("Loading");
        }
    }

    // Select Colors //
    public void SelectRed()
    {
        UserInfo.Add(_selectedUserCounter, new List<string>(new []{"빨간색", "#FF0000", "0"}));
        _selectedUserCounter++;
        red.interactable = false;
        CheckFinalSelection();
    }
    public void SelectGreen()
    {
        UserInfo.Add(_selectedUserCounter, new List<string>(new []{"초록색", "#00FF00", "1"}));
        _selectedUserCounter++;
        green.interactable = false;
        CheckFinalSelection();
    }
    public void SelectBlue()
    {
        UserInfo.Add(_selectedUserCounter, new List<string>(new []{"파란색", "#0000FF", "2"}));
        _selectedUserCounter++;
        blue.interactable = false;
        CheckFinalSelection();
    }
    public void SelectYellow()
    {
        UserInfo.Add(_selectedUserCounter, new List<string>(new []{"노란색", "#FFFF00", "3"}));
        _selectedUserCounter++;
        yellow.interactable = false;
        CheckFinalSelection();
    }
    // Select Colors //
}
}