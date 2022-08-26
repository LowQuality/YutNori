﻿using System.Linq;
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
    public static int MaxTurn;
    public static int MoveCount;
    public static bool DoubleChance;
    public static bool ThrewYut;
    public static bool ShowedValue;
    public static bool DroppedYut;
    
    // Token Count (Normal = Not Exited, N = Not in the Board)
    public static int RedTokenCount;
    public static int GreenTokenCount;
    public static int BlueTokenCount;
    public static int YellowTokenCount;
    public static int NRedTokenCount;
    public static int NGreenTokenCount;
    public static int NBlueTokenCount;
    public static int NYellowTokenCount;

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
            var data = CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("빨간색"));
            GameLog.AddWinLog(data.Value);
            End(data.Key);
        }
        else if (GreenTokenCount <= 0)
        {
            var data = CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("초록색"));
            GameLog.AddWinLog(data.Value);
            End(data.Key);
        }
        else if (BlueTokenCount <= 0)
        {
            var data = CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("파란색"));
            GameLog.AddWinLog(data.Value);
            End(data.Key);
        }
        else if (YellowTokenCount <= 0)
        {
            var data = CharacterSelector.UserInfo.FirstOrDefault(r => r.Value.Contains("노란색"));
            GameLog.AddWinLog(data.Value);
            End(data.Key);
        }
    }

    private static void End(int a)
    {
        GameOver.WinnerData = CharacterSelector.UserInfo[a];
        SceneManager.LoadScene("GameOver");
    }
    
    // Change MoveCount to String
    public static string MoveCountToStr(int a)
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