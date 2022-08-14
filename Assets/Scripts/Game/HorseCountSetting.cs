using TMPro;
using UnityEngine;

namespace Game
{
public class HorseCountSetting : MonoBehaviour
{
    // Horse Counter Text (TMP)
    public TextMeshProUGUI horseCounterText;
    
    // Horse Count
    public static int HorseCount;
    
    // Horse Max/Min Count
    private const int HorseCountMin = 1;
    private const int HorseCountMax = 4;
    
    // Horse Add Button (->)
    public void AddHorse()
    {
        if (HorseCount >= HorseCountMax) return;
        HorseCount++;
        horseCounterText.text = HorseCount.ToString();
    }
    
    // Horse Remove Button (<-)
    public void RemoveHorse()
    {
        if (HorseCount <= HorseCountMin) return;
        HorseCount--;
        horseCounterText.text = HorseCount.ToString();
    }
    
    // Set Default Horse Count
    private void Start()
    {
        HorseCount = HorseCountMin;
        horseCounterText.text = HorseCount.ToString();
    }
}
}