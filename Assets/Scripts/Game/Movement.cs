using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
public class Movement : MonoBehaviour
{
    // Start //
    public GameObject startPlace; // **

    // Right
    public GameObject rDo;
    public GameObject rGae;
    public GameObject rGeol;
    public GameObject rYut;
    public GameObject rMo; // *

    // Back
    public GameObject bDo;
    public GameObject bGae;
    public GameObject bGeol;
    public GameObject bYut;
    public GameObject bMo; // *

    // Left
    public GameObject lDo;
    public GameObject lGae;
    public GameObject lGeol;
    public GameObject lYut;
    public GameObject lMo; // *

    // Front
    public GameObject nDo;
    public GameObject nGae;
    public GameObject nGeol;
    public GameObject nYut;
    
    // Exit
    public GameObject exit; // **

    // Middle
    public GameObject bang;

    public GameObject fMoDo;
    public GameObject fMoGae;
    public GameObject sYut;
    public GameObject sMo;

    public GameObject bMoDo;
    public GameObject bMoGae;
    public GameObject salyeo;

    public GameObject anjjyeo;
    // End //

    // x1
    public GameObject redToken;
    public GameObject greenToken;
    public GameObject blueToken;
    public GameObject yellowToken;

    // x2
    public GameObject redTokenX2;
    public GameObject greenTokenX2;
    public GameObject blueTokenX2;
    public GameObject yellowTokenX2;

    // x3
    public GameObject redTokenX3;
    public GameObject greenTokenX3;
    public GameObject blueTokenX3;
    public GameObject yellowTokenX3;

    // x4
    public GameObject redTokenX4;
    public GameObject greenTokenX4;
    public GameObject blueTokenX4;
    public GameObject yellowTokenX4;
    
    // Highlight
    public GameObject moveTo;
    
    // New Horse Button
    public GameObject newHorseButton;
    
    // Store Horses
    public GameObject horsesGameObject;

    // int : createdCount, int : MoveCount, GameObject : Horse
    public static readonly Dictionary<int, List<Tuple<int, GameObject>>> Horses = new ();
    public static int CreatedHorseCount;

    public static void UpdateHorsesMoveCount(int a, int b)
    {
        Horses[a][0] = new Tuple<int, GameObject>(b, Horses[a][0].Item2);
    }

    public static Movement Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Enable New Horse Button
        newHorseButton.SetActive(
            BoardGame.MoveCount != -1
            &&
            BoardGame.MoveCount != 0
            &&
            BoardGame.ThrewYut
            &&
            StoredHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])) > 0
            );
    }

    public void NewHorse()
    {
        GameObject resolvedHorse;
        var createdHorseCounter = new GameObject();
        switch (Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2]))
        {
            case 0:
                resolvedHorse = Instantiate(redToken, startPlace.transform.position, Quaternion.identity);
                resolvedHorse.transform.SetParent(horsesGameObject.transform);
                Horses.Add(CreatedHorseCount, new List<Tuple<int, GameObject>> {new (BoardGame.MoveCount, resolvedHorse)});

                // AddCounter
                createdHorseCounter.name = CreatedHorseCount.ToString();
                createdHorseCounter.transform.SetParent(resolvedHorse.transform);
                
                CreatedHorseCount++;
                break;
            case 1:
                resolvedHorse = Instantiate(greenToken, startPlace.transform.position, Quaternion.identity);
                resolvedHorse.transform.SetParent(horsesGameObject.transform);
                Horses.Add(CreatedHorseCount, new List<Tuple<int, GameObject>> {new (BoardGame.MoveCount, resolvedHorse)});
                
                // AddCounter
                createdHorseCounter.name = CreatedHorseCount.ToString();
                createdHorseCounter.transform.SetParent(resolvedHorse.transform);
                
                CreatedHorseCount++;
                break;
            case 2:
                resolvedHorse = Instantiate(blueToken, startPlace.transform.position, Quaternion.identity);
                resolvedHorse.transform.SetParent(horsesGameObject.transform);
                Horses.Add(CreatedHorseCount, new List<Tuple<int, GameObject>> {new (BoardGame.MoveCount, resolvedHorse)});
                
                // AddCounter
                createdHorseCounter.name = CreatedHorseCount.ToString();
                createdHorseCounter.transform.SetParent(resolvedHorse.transform);
                
                CreatedHorseCount++;
                break;
            case 3:
                resolvedHorse = Instantiate(yellowToken, startPlace.transform.position, Quaternion.identity);
                resolvedHorse.transform.SetParent(horsesGameObject.transform);
                Horses.Add(CreatedHorseCount, new List<Tuple<int, GameObject>> {new (BoardGame.MoveCount, resolvedHorse)});
                
                // AddCounter
                createdHorseCounter.name = CreatedHorseCount.ToString();
                createdHorseCounter.transform.SetParent(resolvedHorse.transform);
                
                CreatedHorseCount++;
                break;
        }
        
        // Move Horse
        Horses[CreatedHorseCount - 1][0].Item2.transform.position = MoveNewPlace(BoardGame.MoveCount).transform.position;
        // UpdateHorsesMoveCount(CreatedHorseCount - 1, Horses[CreatedHorseCount - 1][0].Item1 + BoardGame.MoveCount);
        UpdateStoredHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2]), -1);
        
        if (!BoardGame.DoubleChance)
        {
            if (BoardGame.NowTurn != BoardGame.MaxTurn) BoardGame.NowTurn++;
            else BoardGame.NowTurn = 0;
        }
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = false;
        BoardGame.ShowedValue = false;
        BoardGame.DroppedYut = false;
        BoardGame.MoveCount = 0;
    }
    
    // Functions //
    public static int StoredHorseCount(int a)
    {
        return a switch
        {
            0 => BoardGame.NRedTokenCount,
            1 => BoardGame.NGreenTokenCount,
            2 => BoardGame.NBlueTokenCount,
            3 => BoardGame.NYellowTokenCount,
            _ => 0
        };
    }
    
    public static void UpdateStoredHorseCount(int a, int b)
    {
        switch (a)
        {
            case 0:
                BoardGame.NRedTokenCount += b;
                break;
            case 1:
                BoardGame.NGreenTokenCount += b;
                break;
            case 2:
                BoardGame.NBlueTokenCount += b;
                break;
            case 3:
                BoardGame.NYellowTokenCount += b;
                break;
        }
    }
    
    public GameObject MoveNewPlace(int a)
    {
        return a switch
        {
            0 => startPlace,
            1 => rDo,
            2 => rGae,
            3 => rGeol,
            4 => rYut,
            5 => rMo // *
            ,
            6 => bDo,
            7 => bGae,
            8 => bGeol,
            9 => bYut,
            10 => bMo // *
            ,
            11 => lDo,
            12 => lGae,
            13 => lGeol,
            14 => lYut,
            15 => lMo // *
            ,
            16 => nDo,
            17 => nGae,
            18 => nGeol,
            19 => nYut,
            20 => startPlace,
            21 => exit // **
            ,
            50 => bang, // *
            51 => fMoDo,
            52 => fMoGae,
            53 => sYut,
            54 => sMo,
            55 => bMoDo,
            56 => bMoGae,
            57 => salyeo,
            58 => anjjyeo,
            _ => null
        };
    }
}
}