using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    public void CameraShake()
    {
        StartCoroutine(CameraShakeRoutine());
    }
    private IEnumerator CameraShakeRoutine()
    {
        bool shakeOn = true;
        float start = Time.time;
        float stop = start + 0.5f;
        float magnitude = 0.1f;
        float frequency = 100f;
        float distanceX, distanceY;
        while (shakeOn == true)
        {
            distanceX = magnitude * Mathf.Sin(0.8f * frequency * (Time.time - start)); 
            distanceY = magnitude * Mathf.Sin(frequency * (Time.time - start));
            transform.position = new Vector3(distanceX, distanceY, -10f);
            if (Time.time > stop)
            {
                transform.position = new Vector3(0, 0, -10f);
                shakeOn = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
