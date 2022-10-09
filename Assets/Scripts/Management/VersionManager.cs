using TMPro;
using UnityEngine;

namespace Management
{
public class VersionManager : MonoBehaviour
{
    public TextMeshProUGUI a;

    public void Start()
    {
        a.text = $"Ver. {Application.version}\nBy - {Application.companyName}";
    }
}
}