using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3 (10f,10f,10f);
    [SerializeField] float period;
    Vector3 offset;

    //#TODO remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; //0 if not moved, 1 for fully moved
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        period = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; //Crece de forma continua desde cero
        const float tau = Mathf.PI * 2; //2 pi, 6.48
        float rawSinWave = Mathf.Sin(cycles * tau); //Multiplico el ángulo, y la funcion siempre me va a dar un valor entre -1 y 1
        movementFactor = rawSinWave / 2f + 0.5f;//Hago que oscile entre -0.5 y + 0.5 y a eso le sumo 0.5 para que oscile entre -1 y 1 (lo smoothea un toque)
        offset = movementVector * movementFactor;
        transform.position = startingPos + offset;


    }
}
