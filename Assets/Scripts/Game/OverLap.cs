﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
public class OverLap : MonoBehaviour
{
    // Horse Pair
    public readonly List<GameObject> Horses = new ();
    
    public static bool Finished;

    // Detect Horses
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Add Horse to List
        Horses.Add(col.gameObject);
        Debug.Log($"Now {Horses.Count} Horses in {gameObject.name} - Added");
        
        // Check if there are 2 Horses in the List
        if (Horses.Count == 2)
        {

            // Get horses Color
            var oldColorHex = Horses[0].GetComponent<Image>().color.ToHexString()[..6];
            var newColorHex = Horses[1].GetComponent<Image>().color.ToHexString()[..6];
            
            // Logs
            var user1 = new List<string>();
            var user2 = new List<string>();
            switch (oldColorHex)
            {
                case "FF0000":
                    user1.Add("빨간색");
                    break;
                case "00FF00":
                    user1.Add("초록색");
                    break;
                case "0000FF":
                    user1.Add("파란색");
                    break;
                case "FFFF00":
                    user1.Add("노란색");
                    break;
            }
            user1.Add($"#{oldColorHex}");
                
            switch (newColorHex)
            {
                case "FF0000":
                    user2.Add("빨간색");
                    break;
                case "00FF00":
                    user2.Add("초록색");
                    break;
                case "0000FF":
                    user2.Add("파란색");
                    break;
                case "FFFF00":
                    user2.Add("노란색");
                    break;
            }
            user2.Add($"#{newColorHex}");

            if (oldColorHex != newColorHex)
                // if this horse is different another horse
            {
                // Double Chance Enabled
                BoardGame.DoubleChance = true;
                // Destroy First Horse
                Movement.UpdateStoredHorseCount(
                    Convert.ToInt32(CharacterSelector.UserInfo[HexColorToUserInfoKey(oldColorHex)][2]),
                    Convert.ToInt32(Horses[0].transform.GetChild(0).name[1..]));
                Movement.Horses.Remove(Convert.ToInt32(Horses[0].transform.GetChild(1).name));
                Destroy(Horses[0]);

                GameLog.AddOverLapLog(user1, user2);
            }
            else
                // if this horse is the same another horse
            {
                var saveLocation = Horses[0].transform.position;
                var oldHorse = Convert.ToInt32(Horses[0].transform.GetChild(1).name);
                var newHorse = Convert.ToInt32(Horses[1].transform.GetChild(1).name);
                var overLapOldHorse = Horses[0].transform.GetChild(0).name;
                var overLapNewHorse = Horses[1].transform.GetChild(0).name;

                var movedCount = Movement.Horses[oldHorse][0].Item1;

                // Remove Old Horses //
                Movement.Horses.Remove(oldHorse);
                Movement.Horses.Remove(newHorse);

                Destroy(Horses[0]); // Destroy First Horse
                Destroy(Horses[0]); // Destroy Second Horse
                // Remove Old Horses //

                GameObject resolvedHorse = null;

                switch (overLapOldHorse)
                {
                    // x1, x1 to x2
                    case "x1" when overLapNewHorse == "x1":
                        resolvedHorse = newColorHex switch
                        {
                            "FF0000" => Instantiate(Movement.Instance.redTokenX2, saveLocation, Quaternion.identity),
                            "00FF00" => Instantiate(Movement.Instance.greenTokenX2, saveLocation, Quaternion.identity),
                            "0000FF" => Instantiate(Movement.Instance.blueTokenX2, saveLocation, Quaternion.identity),
                            "FFFF00" => Instantiate(Movement.Instance.yellowTokenX2, saveLocation, Quaternion.identity),
                            _ => null
                        };
                        break;
                    // (x1, x2) or (x2, x1) to x3
                    case "x1" when overLapNewHorse == "x2":
                    case "x2" when overLapNewHorse == "x1":
                        resolvedHorse = newColorHex switch
                        {
                            "FF0000" => Instantiate(Movement.Instance.redTokenX3, saveLocation, Quaternion.identity),
                            "00FF00" => Instantiate(Movement.Instance.greenTokenX3, saveLocation, Quaternion.identity),
                            "0000FF" => Instantiate(Movement.Instance.blueTokenX3, saveLocation, Quaternion.identity),
                            "FFFF00" => Instantiate(Movement.Instance.yellowTokenX3, saveLocation, Quaternion.identity),
                            _ => null
                        };
                        break;
                    // (x1, x3) or (x3, x1) to x4
                    case "x1" when overLapNewHorse == "x3":
                    case "x3" when overLapNewHorse == "x1":
                        resolvedHorse = newColorHex switch
                        {
                            "FF0000" => Instantiate(Movement.Instance.redTokenX4, saveLocation, Quaternion.identity),
                            "00FF00" => Instantiate(Movement.Instance.greenTokenX4, saveLocation, Quaternion.identity),
                            "0000FF" => Instantiate(Movement.Instance.blueTokenX4, saveLocation, Quaternion.identity),
                            "FFFF00" => Instantiate(Movement.Instance.yellowTokenX4, saveLocation, Quaternion.identity),
                            _ => null
                        };
                        break;
                    // (x2, x2) to x4
                    case "x2" when overLapNewHorse == "x2":
                        resolvedHorse = newColorHex switch
                        {
                            "FF0000" => Instantiate(Movement.Instance.redTokenX4, saveLocation, Quaternion.identity),
                            "00FF00" => Instantiate(Movement.Instance.greenTokenX4, saveLocation, Quaternion.identity),
                            "0000FF" => Instantiate(Movement.Instance.blueTokenX4, saveLocation, Quaternion.identity),
                            "FFFF00" => Instantiate(Movement.Instance.yellowTokenX4, saveLocation, Quaternion.identity),
                            _ => null
                        };
                        break;
                }

                if (resolvedHorse == null) return;
                resolvedHorse.transform.SetParent(Movement.Instance.horsesGameObject.transform);
                Movement.Horses.Add(Movement.CreatedHorseCount,
                    new List<Tuple<int, GameObject>> { new(movedCount, resolvedHorse) });

                // AddCounter
                var createdHorseCounter = new GameObject
                {
                    name = Movement.CreatedHorseCount.ToString()
                };
                createdHorseCounter.transform.SetParent(resolvedHorse.transform);

                Movement.CreatedHorseCount++;
                
                GameLog.AddOverLapLog(user1, user2, Convert.ToInt32(resolvedHorse.transform.GetChild(0).name[1..]));
            }
        }

        Finished = true;

        if (!HorseMovement.IsHorseMove) return; // Global if
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
        HorseMovement.IsHorseMove = false;
        HorseMovement.MoveEnabled = false;
        Finished = false;
    }
    
    // Remove Horses when Exit
    private void OnTriggerExit2D(Collider2D col)
    {
        Horses.Remove(col.gameObject);
        Debug.Log($"Now {Horses.Count} Horses in {gameObject.name} - Removed");
    }
    
    // Functions //
    private static int HexColorToUserInfoKey(string a)
    {
        return a switch
        {
            "FF0000" => CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[2] == "0").Key,
            "00FF00" => CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[2] == "1").Key,
            "0000FF" => CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[2] == "2").Key,
            "FFFF00" => CharacterSelector.UserInfo.FirstOrDefault(x => x.Value[2] == "3").Key,
            _ => -1
        };
    }
}
}