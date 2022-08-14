using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
public class GameOver : MonoBehaviour
{
    // Winner Text (TMP)
    public TextMeshProUGUI winnerText;
    // Winner Data
    public static List<string> WinnerData = new ();
    
    private void Start()
    {
        winnerText.color = ColorUtility.TryParseHtmlString(WinnerData[1], out var color) ? color : Color.white;
        winnerText.text = WinnerData[0];
    }
}
}