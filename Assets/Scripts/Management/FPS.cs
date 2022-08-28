using UnityEngine;

namespace Management
{
public class FPS : MonoBehaviour
{
    public int maxFPS;
    private void Start()
    {
        Application.targetFrameRate = maxFPS;
    }
}
}