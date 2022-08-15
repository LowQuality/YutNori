using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
public class ExitTrigger : MonoBehaviour
{
    // Detected Horse in Exit
    private void OnTriggerEnter2D(Collider2D col)
    {
        var color = col.gameObject.GetComponent<Image>().color.ToHexString()[..6];
        var saveHorseCount = Convert.ToInt32(col.gameObject.transform.GetChild(0).name[1..]);
        switch (color)
        {
            case "FF0000":
                Destroy(col.gameObject);
                BoardGame.RedTokenCount -= saveHorseCount;
                break;
            case "00FF00":
                Destroy(col.gameObject);
                BoardGame.GreenTokenCount -= saveHorseCount;
                break;
            case "0000FF":
                Destroy(col.gameObject);
                BoardGame.BlueTokenCount -= saveHorseCount;
                break;
            case "FFFF00":
                Destroy(col.gameObject);
                BoardGame.YellowTokenCount -= saveHorseCount;
                break;
        }
        
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
        OverLap.Finished = false;
    }
}
}