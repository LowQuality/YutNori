using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
public class BoardGame : MonoBehaviour
{
    // Debug Module
    public GameObject debugModule;
    
    // Now Turn Text (TMP)
    public TextMeshProUGUI turnText;
    
    // MoveCount Text (TMP)
    public TextMeshProUGUI moveCountText;
    
    // DoubleChance Text (TMP)
    public GameObject doubleChanceText;

    // TokenCount Text (TMP)
    public TextMeshProUGUI redToken;
    public TextMeshProUGUI greenToken;
    public TextMeshProUGUI blueToken;
    public TextMeshProUGUI yellowToken;
    
    // Game Variables
    public static int NowTurn;
    public static readonly int MaxTurn = PlayerCountSetting.PlayerCount - 1;
    public static int MoveCount;
    public static bool DoubleChance;
    public static bool ThrewYut;
    public static bool ShowedValue;
    public static bool DroppedYut;
    
    // Token Count (Normal = Not Exited, N = Not in the Board)
    public static int RedTokenCount = HorseCountSetting.HorseCount;
    public static int GreenTokenCount = HorseCountSetting.HorseCount;
    public static int BlueTokenCount = HorseCountSetting.HorseCount;
    public static int YellowTokenCount = HorseCountSetting.HorseCount;
    public static int NRedTokenCount = HorseCountSetting.HorseCount;
    public static int NGreenTokenCount = HorseCountSetting.HorseCount;
    public static int NBlueTokenCount = HorseCountSetting.HorseCount;
    public static int NYellowTokenCount = HorseCountSetting.HorseCount;

    private void Start()
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log(
                "\n" +
                      "\n" +
                      "------------------------------------------------------\n" +
                      "Debug Mode Enabled!\n" +
                      "------------------------------------------------------\n" +
                      "\n" +
                      "\n")
                ;
            debugModule.SetActive(true);
        }
        NowTurn = 0;
    }

    private void Update()
    {
        // Token Count Text
        redToken.text = CharacterSelector.UserInfo.Any(v => v.Value.Contains("빨간색")) ? $": {NRedTokenCount}/{RedTokenCount}" : ": X";
        greenToken.text = CharacterSelector.UserInfo.Any(v => v.Value.Contains("초록색")) ? $": {NGreenTokenCount}/{GreenTokenCount}" : ": X";
        blueToken.text = CharacterSelector.UserInfo.Any(v => v.Value.Contains("파란색")) ? $": {NBlueTokenCount}/{BlueTokenCount}" : ": X";
        yellowToken.text = CharacterSelector.UserInfo.Any(v => v.Value.Contains("노란색")) ? $": {NYellowTokenCount}/{YellowTokenCount}" : ": X";

        // Now Turn Text
        var userData = CharacterSelector.UserInfo[NowTurn];
        turnText.color = ColorUtility.TryParseHtmlString(userData[1], out var color) ? color : Color.white;
        turnText.text = userData[0];
        
        // NewToken
        // if (MoveCount != -1 && ThrewYut && TokenCount(NowTurn) > 0) newTokenButton.SetActive(true); else newTokenButton.SetActive(false);
        
        // MoveCount Text
        if (ThrewYut && ShowedValue) moveCountText.text = MoveCountToStr(MoveCount); else moveCountText.text = "";
        
        // DoubleChance Text
        doubleChanceText.SetActive(ThrewYut && ShowedValue && DoubleChance);
        
        // Check Winner
        if (RedTokenCount <= 0)
        {
            End(CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("빨간색")).Key);
        }
        else if (GreenTokenCount <= 0)
        {
            End(CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("초록색")).Key);
        }
        else if (BlueTokenCount <= 0)
        {
            End(CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("파란색")).Key);
        }
        else if (YellowTokenCount <= 0)
        {
            End(CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("노란색")).Key);
        }
    }

    private static void End(int a)
    {
        GameOver.WinnerData = CharacterSelector.UserInfo[a];
        SceneManager.LoadScene("GameOver");
    }
    
    // Change MoveCount to String
    private static string MoveCountToStr(int a)
    {
        return a switch
        {
            0 => "낙",
            1 => "도",
            2 => "개",
            3 => "걸",
            4 => "윷",
            5 => "모",
            -1 => "백도",
            _ => null
        };
    }
}
}