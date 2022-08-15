using System;
using UnityEngine;

namespace Game
{
public class DebugModule : MonoBehaviour
{
    public void TurnChanger()
    {
        if (BoardGame.NowTurn != BoardGame.MaxTurn) BoardGame.NowTurn++;
        else BoardGame.NowTurn = 0;
        
        gameObject.GetComponent<DebugModule>().Re();
    }

    public void Re()
    {
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = false;
        BoardGame.ShowedValue = false;
        BoardGame.DroppedYut = false;
        BoardGame.MoveCount = 0;
        OverLap.Finished = false;
    }
    
    public void Dropped()
    {
        BoardGame.MoveCount = 0;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        BoardGame.DroppedYut = true;
    }

    public void Do()
    {
        BoardGame.MoveCount = 1;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        BoardGame.DroppedYut = false;
    }

    public void Gae()
    {
        BoardGame.MoveCount = 2;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        BoardGame.DroppedYut = false;
    }

    public void Geol()
    {
        BoardGame.MoveCount = 3;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        BoardGame.DroppedYut = false;
    }

    public void Yut()
    {
        BoardGame.MoveCount = 4;
        BoardGame.DoubleChance = true;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        BoardGame.DroppedYut = false;
    }

    public void Mo()
    {
        BoardGame.MoveCount = 5;
        BoardGame.DoubleChance = true;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        BoardGame.DroppedYut = false;
    }

    public void BackDo()
    {
        BoardGame.MoveCount = -1;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
        OverLap.Finished = false;
        
        if (BoardGame.MoveCount == -1 &&
            Movement.StoredHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])) ==
            Movement.AllHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])))
        {
            BoardGame.DroppedYut = true;
        }
    }
}
}