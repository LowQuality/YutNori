using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
public class HorseMovement : MonoBehaviour
{
    public static bool MoveEnabled;
    private static GameObject _waitingHorse;
    private static readonly List<GameObject> Highlights = new ();
    private static GameObject _movedHorse;

    public static bool IsHorseMove;

    private void Update()
    {
        if (MoveEnabled) return;
        try
        {
            foreach (var t in Highlights)
            {
                Destroy(t);
            }
            Highlights.Clear();
        }
        catch (Exception)
        {
            // ignored
        }
        _waitingHorse = null;
    }

    public void Move()
    {
        // Check is clicked horse is now turn
        if (CharacterSelector.UserInfo[BoardGame.NowTurn][1][1..] !=
            gameObject.GetComponent<Image>().color.ToHexString()[..6]) return;

        if (!MoveEnabled && BoardGame.MoveCount != 0)
        {
            var horseMovedCount = Movement.Horses[Convert.ToInt32(gameObject.transform.GetChild(1).name)][0].Item1;

            // BackDo Support
            if (horseMovedCount == 0)
                Movement.UpdateHorsesMoveCount(Convert.ToInt32(gameObject.transform.GetChild(1).name), 20);

            MoveEnabled = true;

            horseMovedCount = horseMovedCount switch
            {
                50 => 5,
                70 => 10,
                90 => 53,
                110 => 73,
                _ => horseMovedCount
            };

            GameObject highlight;
            GameObject newMoveGameObject;
            GameObject storeHorseMoveCount;
            switch (horseMovedCount)
            {
                // rMo
                case 5 when BoardGame.MoveCount != -1:
                    storeHorseMoveCount = new GameObject
                    {
                        name = (50 + BoardGame.MoveCount).ToString()
                    };
                    newMoveGameObject = Movement.Instance.MoveNewPlace(Convert.ToInt32(storeHorseMoveCount.name));
                    highlight = Instantiate(Movement.Instance.moveTo, newMoveGameObject.transform.position,
                        Quaternion.identity, GameObject.Find("Container").transform);
                    storeHorseMoveCount.transform.SetParent(highlight.transform);
                    Highlights.Add(highlight);
                    break;
                // bMo
                case 10 when BoardGame.MoveCount != -1:
                    storeHorseMoveCount = new GameObject
                    {
                        name = (70 + BoardGame.MoveCount).ToString()
                    };
                    newMoveGameObject = Movement.Instance.MoveNewPlace(Convert.ToInt32(storeHorseMoveCount.name));
                    highlight = Instantiate(Movement.Instance.moveTo, newMoveGameObject.transform.position,
                        Quaternion.identity, GameObject.Find("Container").transform);
                    storeHorseMoveCount.transform.SetParent(highlight.transform);
                    Highlights.Add(highlight);
                    break;
            }
            
            var storeHorseMoveCount2 = new GameObject
            {
                name = horseMovedCount.ToString()
            };

            var moveCount = Convert.ToInt32(storeHorseMoveCount2.name);
            var willMoveCount = moveCount + BoardGame.MoveCount;

            if (
                (Enumerable.Range(0, 21).Contains(horseMovedCount) && Enumerable.Range(22, 27).Contains(willMoveCount))
                ||
                (Enumerable.Range(50, 12).Contains(horseMovedCount) && Enumerable.Range(62, 7).Contains(willMoveCount))
                ||
                (Enumerable.Range(70, 7).Contains(horseMovedCount) && Enumerable.Range(77, 11).Contains(willMoveCount))
                ||
                (Enumerable.Range(90, 9).Contains(horseMovedCount) && Enumerable.Range(99, 9).Contains(willMoveCount))
                ||
                (Enumerable.Range(110, 4).Contains(horseMovedCount) && Enumerable.Range(114, 5).Contains(willMoveCount))
                )
            {
                storeHorseMoveCount2.name = 21.ToString();
            }
            else
            {
                storeHorseMoveCount2.name = (horseMovedCount + BoardGame.MoveCount).ToString();
            }
            
            if (horseMovedCount is 53 or 73 && BoardGame.MoveCount != -1)
            {
                storeHorseMoveCount = new GameObject
                {
                    name = (90 + BoardGame.MoveCount).ToString()
                };
                newMoveGameObject = Movement.Instance.MoveNewPlace(Convert.ToInt32(storeHorseMoveCount.name));
                highlight = Instantiate(Movement.Instance.moveTo, newMoveGameObject.transform.position,
                    Quaternion.identity, GameObject.Find("Container").transform);
                storeHorseMoveCount.transform.SetParent(highlight.transform);
                Highlights.Add(highlight);

                storeHorseMoveCount2.name = 110 + BoardGame.MoveCount > 114 ? 114.ToString() : (110 + BoardGame.MoveCount).ToString();
                newMoveGameObject = Movement.Instance.MoveNewPlace(Convert.ToInt32(storeHorseMoveCount2.name));
                highlight = Instantiate(Movement.Instance.moveTo, newMoveGameObject.transform.position,
                    Quaternion.identity, GameObject.Find("Container").transform);
                storeHorseMoveCount2.transform.SetParent(highlight.transform);
                Highlights.Add(highlight);
                _waitingHorse = gameObject;
            }
            else
            {
                newMoveGameObject = Movement.Instance.MoveNewPlace(Convert.ToInt32(storeHorseMoveCount2.name));

                highlight = Instantiate(Movement.Instance.moveTo, newMoveGameObject.transform.position,
                    Quaternion.identity, GameObject.Find("Container").transform);
                storeHorseMoveCount2.transform.SetParent(highlight.transform);
                Highlights.Add(highlight);
                _waitingHorse = gameObject;
            }
        }
        else
        {
            MoveEnabled = false;
            _waitingHorse = null;
        }
    }

    public void Apply()
    {
        var savePosition = gameObject.transform.position;
        var saveHorseCount = _waitingHorse.transform.GetChild(0).name;
        
        Movement.Horses.Remove(Convert.ToInt32(_waitingHorse.transform.GetChild(1).name));
        
        switch (_waitingHorse.GetComponent<Image>().color.ToHexString()[..6])
        {
            case "FF0000":
                Destroy(_waitingHorse);
                // Create new horse from file
                _movedHorse = saveHorseCount switch
                {
                    "x1" => Instantiate(Movement.Instance.redToken, savePosition, Quaternion.identity),
                    "x2" => Instantiate(Movement.Instance.redTokenX2, savePosition, Quaternion.identity),
                    "x3" => Instantiate(Movement.Instance.redTokenX3, savePosition, Quaternion.identity),
                    "x4" => Instantiate(Movement.Instance.redTokenX4, savePosition, Quaternion.identity),
                    _ => _movedHorse
                };
                break;
            case "00FF00":
                Destroy(_waitingHorse);
                _movedHorse = saveHorseCount switch
                {
                    "x1" => Instantiate(Movement.Instance.greenToken, savePosition, Quaternion.identity),
                    "x2" => Instantiate(Movement.Instance.greenTokenX2, savePosition, Quaternion.identity),
                    "x3" => Instantiate(Movement.Instance.greenTokenX3, savePosition, Quaternion.identity),
                    "x4" => Instantiate(Movement.Instance.greenTokenX4, savePosition, Quaternion.identity),
                    _ => _movedHorse
                };
                break;
            case "0000FF":
                Destroy(_waitingHorse);
                _movedHorse = saveHorseCount switch
                {
                    "x1" => Instantiate(Movement.Instance.blueToken, savePosition, Quaternion.identity),
                    "x2" => Instantiate(Movement.Instance.blueTokenX2, savePosition, Quaternion.identity),
                    "x3" => Instantiate(Movement.Instance.blueTokenX3, savePosition, Quaternion.identity),
                    "x4" => Instantiate(Movement.Instance.blueTokenX4, savePosition, Quaternion.identity),
                    _ => _movedHorse
                };
                break;
            case "FFFF00":
                Destroy(_waitingHorse);
                _movedHorse = saveHorseCount switch
                {
                    "x1" => Instantiate(Movement.Instance.yellowToken, savePosition, Quaternion.identity),
                    "x2" => Instantiate(Movement.Instance.yellowTokenX2, savePosition, Quaternion.identity),
                    "x3" => Instantiate(Movement.Instance.yellowTokenX3, savePosition, Quaternion.identity),
                    "x4" => Instantiate(Movement.Instance.yellowTokenX4, savePosition, Quaternion.identity),
                    _ => _movedHorse
                };
                break;
        }
        _movedHorse.transform.position = savePosition;
        _movedHorse.transform.SetParent(Movement.Instance.horsesGameObject.transform);
        Movement.Horses.Add(Movement.CreatedHorseCount,
            new List<Tuple<int, GameObject>> { new(Convert.ToInt32(gameObject.transform.GetChild(0).name), _movedHorse) });

        // AddCounter
        var createdHorseCounter = new GameObject
        {
            name = Movement.CreatedHorseCount.ToString()
        };
        createdHorseCounter.transform.SetParent(_movedHorse.transform);
                
        Movement.CreatedHorseCount++;
        
        MoveEnabled = false;
        IsHorseMove = true;
    }
}
}