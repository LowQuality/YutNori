using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
public class YutPhysicsMode : MonoBehaviour
{
    public GameObject previous;
    public GameObject calculating;
    public GameObject calculated;
    public GameObject droppedYut;
    public GameObject yutCam;

    public List<GameObject> spawner;
    public GameObject yut;
    public GameObject yutMarked;

    private const string ThrowType = "물리";

    public static readonly Dictionary<int, Dictionary<int, bool>> Yut = new();
    public static readonly List<GameObject> YutGameObject = new();

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
        yutCam.SetActive(BoardGame.ThrewYut && !BoardGame.ShowedValue);
        
        calculated.SetActive(BoardGame.ThrewYut && BoardGame.ShowedValue);
        droppedYut.SetActive(BoardGame.ThrewYut && BoardGame.ShowedValue && BoardGame.DroppedYut);
    }
    
    private static void MoveCount(int a)
    {
        BoardGame.MoveCount = a;
        BoardGame.DoubleChance = false;
        BoardGame.ThrewYut = true;
        BoardGame.ShowedValue = true;
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

    public void YutThrow()
    {
        // Init
        Yut.Clear();
        BoardGame.ThrewYut = true;

        // Throw
        for (var i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 3:
                    YutGameObject.Add(Instantiate(yutMarked, spawner[i].transform.position, Quaternion.identity));
                    break;
                default:
                    YutGameObject.Add(Instantiate(yut, spawner[i].transform.position, Quaternion.identity));
                    break;
            }
        }

        StartCoroutine(CalculateYut());
    }

    private static IEnumerator CalculateYut()
    {
        yield return new WaitUntil(() => Yut.Count == 4);

        var front = Yut.Where(x => x.Value.ContainsKey(1)).ToList().Count;
        var frontMarked = Yut.Where(x => x.Value.ContainsKey(1) && x.Value.ContainsValue(true)).ToList().Count;
        var back = Yut.Where(x => x.Value.ContainsKey(2)).ToList().Count;
        Debug.Log($"front: {front} | frontMarked: {frontMarked} | back: {back}");

        // calculate yut
        switch (front)
        {
            // Do
            case 1:
                if (frontMarked == 1)
                {
                    MoveCount(-1);
                    GameLog.AddMoveLog(ThrowType, BoardGame.MoveCountToStr(-1));
                }
                else
                {
                    MoveCount(1);
                    GameLog.AddMoveLog(ThrowType, BoardGame.MoveCountToStr(1));
                }
                break;
            // Gae
            case 2:
                MoveCount(2);
                GameLog.AddMoveLog(ThrowType, BoardGame.MoveCountToStr(2));
                break;
            // Geol
            case 3:
                MoveCount(3);
                GameLog.AddMoveLog(ThrowType, BoardGame.MoveCountToStr(3));
                break;
            // Yut
            case 0:
                MoveCount(4);
                BoardGame.DoubleChance = true;
                GameLog.AddMoveLog(ThrowType, BoardGame.MoveCountToStr(4));
                break;
            // Mo
            case 4:
                MoveCount(5);
                BoardGame.DoubleChance = true;
                GameLog.AddMoveLog(ThrowType, BoardGame.MoveCountToStr(5));
                break;
        }

        if (BoardGame.MoveCount == -1 &&
            Movement.StoredHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])) ==
            Movement.AllHorseCount(Convert.ToInt32(CharacterSelector.UserInfo[BoardGame.NowTurn][2])))
        {
            BoardGame.DroppedYut = true;
        }

        foreach (var t in YutGameObject)
        {
            Destroy(t);
        }
        YutGameObject.Clear();
    }
}
}