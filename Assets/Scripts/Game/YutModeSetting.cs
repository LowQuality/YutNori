using TMPro;
using UnityEngine;

namespace Game
{
public class YutModeSetting : MonoBehaviour
{
    public GameObject blocker;
    public TextMeshProUGUI playBackTxt;
    public TextMeshProUGUI modeTxt;
    public GameObject warningTxt;

    public static int PlayBackRate = 1;
    public static int Mode;
    
    private void Start()
    {
        // Reset the play back rate to 1
        // PlayBackRate = 1;
        playBackTxt.text = $"x{PlayBackRate}";

        // Reset Mode to normal
        // Mode = 0;
        modeTxt.text = Mode switch
        {
            0 => "일반",
            1 => "물리 (Beta)",
            _ => modeTxt.text
        };
        blocker.SetActive(Mode != 1);
    }
    
    public void IncreasePlayBackRate()
    {
        if (PlayBackRate >= 4) return;
        PlayBackRate++;
        playBackTxt.text = $"x{PlayBackRate}";
    }
    
    public void DecreasePlayBackRate()
    {
        if (PlayBackRate <= 1) return;
        PlayBackRate--;
        playBackTxt.text = $"x{PlayBackRate}";
    }
    
    public void ChangeMode()
    {
        Mode++;
        if (Mode > 1) Mode = 0;
        blocker.SetActive(Mode != 1);
        modeTxt.text = Mode == 0 ? "일반" : "물리 (Beta)";
        warningTxt.SetActive(Mode == 1);
    }
}
}