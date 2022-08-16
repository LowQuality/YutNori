using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Game
{
public class YutSupport : MonoBehaviour
{
    public GameObject previous;
    public GameObject calculating;
    public GameObject calculated;
    public GameObject droppedYutCheck;
    
    // Calculate Yut (yut F/B/D, yut Marked)
    
    // !! Notice !! //
    // Front = Curved Side, Back = Straight Side, Drop = Dropped
    // !! Notice !! //
    
    public static Dictionary<int, Dictionary<int, bool>> yut = new ();

    public void LowThrow()
    {
        // Clean Up Previous Yut Data
        yut.Clear();
        
        // Threw
        BoardGame.ThrewYut = true;
        
        // Calc (Front = 39%, Back = 60.5%, Drop = 0.5%) x 4
        for (var i = 0; i < 4; i++)
        {
            var random = new Random();
            var randomNumber = random.Next(0, 10000);
            switch (randomNumber)
            {
                case < 3900:
                    yut.Add(i, new Dictionary<int, bool> { { 0, false } }); // Front
                    break;
                case < 9950:
                    yut.Add(i, new Dictionary<int, bool> { { 1, false } }); // Back
                    break;
                default:
                    yut.Add(i, new Dictionary<int, bool> { { 2, false } }); // Drop
                    break;
            }
        }
        CalculateYut();
    }
    public void HighThrow()
    {
        // Clean Up Previous Yut Data
        yut.Clear();
        
        // Threw
        BoardGame.ThrewYut = true;
        
        // Calc (Front = 60%, Back = 30%, Drop = 10%) x 4
        for (var i = 0; i < 4; i++)
        {
            var random = new Random();
            var randomNumber = random.Next(0, 100);
            switch (randomNumber)
            {
                case < 60:
                    yut.Add(i, new Dictionary<int, bool> {{1, false}}); // Front
                    break;
                case < 90:
                    yut.Add(i, new Dictionary<int, bool> {{2, false}}); // Back
                    break;
                default:
                    yut.Add(i, new Dictionary<int, bool> {{3, false}}); // Drop
                    break;
            }
        }
        CalculateYut();
    }

    public void DroppedYutCheck()
    {
        if (BoardGame.NowTurn != BoardGame.MaxTurn) BoardGame.NowTurn++;
        else BoardGame.NowTurn = 0;
        
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = false;
        BoardGame.ShowedValue = false;
        BoardGame.DroppedYut = false;
        BoardGame.MoveCount = 0;
    }

    private void Start()
    {
        previous.SetActive(true);
        calculating.SetActive(false);
        calculated.SetActive(false);
    }

    private void Update()
    {
        previous.SetActive(!BoardGame.ThrewYut);
        calculating.SetActive(BoardGame.ThrewYut && !BoardGame.ShowedValue);
        calculated.SetActive(BoardGame.ThrewYut && BoardGame.ShowedValue);
        droppedYutCheck.SetActive(BoardGame.ThrewYut && BoardGame.ShowedValue && BoardGame.DroppedYut);
    }

    private static void MoveCount(int a)
    {
        BoardGame.MoveCount = a;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
    }

    private static void CalculateYut()
    {
        // Mark Rendomly
        var random = new Random();
        var randomMark = random.Next(0, 3);
        yut.ElementAt(randomMark).Value[0] = true;
        
        // Drop Check
        var dropCheck = yut.Where(x => x.Value.ContainsKey(3)).ToList();
        if (dropCheck.Count > 0)
        {
            BoardGame.MoveCount = 0;
            BoardGame.DoubleChance = false;
            BoardGame.ShowedValue = true;
            BoardGame.DroppedYut = true;
        }
        else
        {
            var front = yut.Where(x => x.Value.ContainsKey(1)).ToList().Count;
            var frontMarked = yut.Where(x => x.Value.ContainsKey(1) && x.Value.ContainsValue(true)).ToList().Count;
            var back = yut.Where(x => x.Value.ContainsKey(2)).ToList().Count;
            
            // calculate yut
            switch (front)
            {
                // Mo
                case 0:
                    MoveCount(5);
                    BoardGame.DoubleChance = true;
                    break;
                // Do
                case 1:
                    if (frontMarked == 1) MoveCount(-1);
                    else MoveCount(1);
                    break;
                // Gae
                case 2:
                    MoveCount(2);
                    break;
                // Geol
                case 3:
                    MoveCount(3);
                    break;
                // Yut
                case 4:
                    MoveCount(4);
                    BoardGame.DoubleChance = true;
                    break;
            }
            
            if (BoardGame.MoveCount == -1 &&
                Movement.StoredHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])) ==
                Movement.AllHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])))
            {
                BoardGame.DroppedYut = true;
            }
        }
    }
}
}