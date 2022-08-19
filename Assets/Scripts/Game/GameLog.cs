using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game
{
public class GameLog : MonoBehaviour
{
    public static readonly List<string> Logs = new();
    public TextMeshProUGUI logText;

    private void Start()
    {
        foreach (var t in Logs)
        {
            logText.text += t + "\n";
        }
    }

    public static void AddMoveLog(string uses, string yutString)
    {
        var nowUser = CharacterSelector.UserInfo[BoardGame.NowTurn];
        Logs.Add($"<color={nowUser[1]}><sprite={nowUser[2]}> {nowUser[0]}</color> 님이 '{uses}' 을(를) 이용하여 '{yutString}' 이(가) 나왔습니다!");
        if (BoardGame.DoubleChance)
        {
            Logs.Add($"<color={nowUser[1]}><sprite={nowUser[2]}> {nowUser[0]}</color> 한번 더!");
        }
    }

    public static void AddExitLog(int horseCount)
    {
        var nowUser = CharacterSelector.UserInfo[BoardGame.NowTurn];
        Logs.Add($"<color={nowUser[1]}><sprite={nowUser[2]}> {nowUser[0]}</color> 님이 탈출 하였습니다! 남은 말 개수 : {horseCount}");
    }
    
    public static void AddOverLapLog(List<string> user1, List<string> user2, int overLappedCount = 0)
    {
        var user1Key = Convert.ToInt32(CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[0] == user1[0]).Value[2]);
        var user2Key = Convert.ToInt32(CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[0] == user2[0]).Value[2]);
        if (user1[0] != user2[0])
        {
            Logs.Add($"<color={user2[1]}><sprite={user2Key}> {user2[0]}</color> 님이 <color={user1[1]}><sprite={user1Key}> {user1[0]}</color> 님을 잡았습니다!");
            Logs.Add($"<color={user2[1]}><sprite={user2Key}> {user2[0]}</color> 한번 더!");
        }
        else
        {
            Logs.Add($"<color={user2[1]}><sprite={user2Key}> {user2[0]}</color> 님이 말을 업었습니다! 업은 수 : {overLappedCount}");
        }
    }
    
    public static void AddWinLog(List<string> user)
    {
        var nowUserKey = Convert.ToInt32(CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[0] == user[0]).Value[2]);
        Logs.Add($"<color={user[1]}><sprite={nowUserKey}> {user[0]}</color> 님이 승리 하였습니다!");
    }
}
}