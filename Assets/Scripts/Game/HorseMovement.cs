using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
public class HorseMovement : MonoBehaviour
{
    public static bool MoveEnabled;
    private static GameObject _waitingHorse;
    private static readonly List<GameObject> _highlights = new ();
    private static GameObject _movedHorse;

    public void Move()
    {
        // Check is clicked horse is now turn
        if (CharacterSelector.UserInfo[BoardGame.NowTurn][1][1..] !=
            gameObject.GetComponent<Image>().color.ToHexString()[..6]) return;

        if (!MoveEnabled && BoardGame.MoveCount != 0 && BoardGame.MoveCount != -1)
        {
            MoveEnabled = true;
            // Get Required Information
            var movedCount = Movement.Horses[Convert.ToInt32(gameObject.transform.GetChild(1).name)][0].Item1;
            Movement.UpdateHorsesMoveCount(Convert.ToInt32(gameObject.transform.GetChild(1).name), movedCount);
            
            int newMoveCount;
            if (movedCount + BoardGame.MoveCount > 21) newMoveCount = 21;
            else newMoveCount = movedCount + BoardGame.MoveCount;
            
            var newMoveGameObject = Movement.Instance.MoveNewPlace(newMoveCount);
            
            var highlight = Instantiate(Movement.Instance.moveTo, newMoveGameObject.transform.position, Quaternion.identity);
            highlight.transform.SetParent(GameObject.Find("Container").transform);
            _highlights.Add(highlight);
            _waitingHorse = gameObject;
        }
        else
        {
            MoveEnabled = false;
            try
            {
                foreach (var t in _highlights)
                {
                    Destroy(t);
                }
                _highlights.Clear();
            }
            catch (Exception e)
            {
                // ignored
            }
            _waitingHorse = null;
        }
    }
    public void Apply()
    {
        var savePosition = gameObject.transform.position;
        var saveHorseCount = _waitingHorse.transform.GetChild(0).name;
        var movedCount = Movement.Horses[Convert.ToInt32(_waitingHorse.transform.GetChild(1).name)][0].Item1 + BoardGame.MoveCount;
        
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
            new List<Tuple<int, GameObject>> { new(movedCount, _movedHorse) });

        // AddCounter
        var createdHorseCounter = new GameObject
        {
            name = Movement.CreatedHorseCount.ToString()
        };
        createdHorseCounter.transform.SetParent(_movedHorse.transform);
                
        Movement.CreatedHorseCount++;
        
        MoveEnabled = false;
        try
        {
            foreach (var t in _highlights)
            {
                Destroy(t);
            }
            _highlights.Clear();
        }
        catch (Exception e)
        {
            // ignored
        }
        _waitingHorse = null;

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
}
}