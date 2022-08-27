using TMPro;
using UnityEngine;

namespace Game
{
public class PlayerCountSetting : MonoBehaviour
{
    
    // Player Counter Text (TMP)
    public TextMeshProUGUI playerCountText;
    
    // Player Count
    public static int PlayerCount = 1;
    
    // Player Max/Min Count
    private const int PlayerCountMin = 1;
    private const int PlayerCountMax = 4;

    // Player Add Button (->)
    public void AddPlayer()
    {
        if (PlayerCount >= PlayerCountMax) return;
        PlayerCount++;
        playerCountText.text = PlayerCount.ToString();
    }
    
    // Player Remove Button (<-)
    public void RemovePlayer()
    {
        if (PlayerCount <= PlayerCountMin) return;
        PlayerCount--;
        playerCountText.text = PlayerCount.ToString();
    }
    
    // Set Default Player Count
    private void Start()
    {
        // PlayerCount = PlayerCountMin;
        playerCountText.text = PlayerCount.ToString();
    }
}
}