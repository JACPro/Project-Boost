using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    //todo remvove from inspector later

    float movementFactor; //0 for not moved, 1 for fully moved

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        //todo protect against period = 0
        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; //roughly 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); //goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startPos + offset;
    }
}
