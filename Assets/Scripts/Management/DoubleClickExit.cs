using UnityEngine;
using Utils.Android;

namespace Management
{
public class DoubleClickExit : MonoBehaviour
{
    private int _escapeCount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_escapeCount == 0) ToastMessage.Send("게임을 종료하려면 한번 더 누르세요.");
            _escapeCount++;
            if (!IsInvoking(nameof(DoubleClick)))
                Invoke(nameof(DoubleClick), 1.0f);

        }
        else if (_escapeCount == 2)
        {
            CancelInvoke(nameof(DoubleClick));
            Application.Quit();
        }
    }

    private void DoubleClick()
    {
        _escapeCount = 0;
    }
}
}