using UnityEngine;
using System.Collections;

public class DelayerSwitchScene : MonoBehaviour
{
    public float SecondsOfDelay;

    void Update()
    {
        SecondsOfDelay -= Time.deltaTime;
        if (SecondsOfDelay < 0.0f)
            Application.LoadLevel(1);
    }
}
