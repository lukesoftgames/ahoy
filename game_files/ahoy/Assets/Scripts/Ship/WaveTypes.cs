using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTypes
{
    public static float SinXWave(
        Vector3 position,
        float speed,
        float scale,
        float waveDistance,
        float noiseStrength,
        float noiseWalk,
        float timeSinceStart    
    ) {
        float x = position.x;
        float y = 0f;
        float z = position.z;

        float waveType = 0f;
        waveType = x + y + z;
        //waveType = x * z;


        y += Mathf.Sin((timeSinceStart * speed + waveType) / waveDistance) * scale;

        y += Mathf.PerlinNoise(x + noiseWalk, y + Mathf.Sin(timeSinceStart * 0.1f)) * noiseStrength;
        
        return y;
    }
}
