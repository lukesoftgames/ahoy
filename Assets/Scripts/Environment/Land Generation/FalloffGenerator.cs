using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator {
    private static float[,] baseFalloff;

    public static float[,] generateFalloffMap(float[,] noise, int size, float curveFactor, float shiftFactor, float frequency) {
        float[,] map = new float[size, size];
        float thresholdValue = frequency == 0 ? 0 : 1 / 1 + Mathf.Exp(2 * frequency - 5);
        List<Vector2> thresholdPoints = new List<Vector2>();

        //Generate noisemap if necessary
        if(baseFalloff == null) {
            baseFalloff = new float[size, size];

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    float x = i / (float)size * 2 - 1;
                    float y = j / (float)size * 2 - 1;

                    float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                    baseFalloff[i, j] = evaluate(value, curveFactor, shiftFactor);
                }
            }
        }

        //Find thresholds for island generation
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                if(noise[i, j] > thresholdValue) {
                    thresholdPoints.Add(new Vector2(i, j));
                }
            }
        }

        //No thresholds found
        if(thresholdPoints.Count == 0) {
            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    map[i, j] = 1.0f;
                }
            }

            return map;
        }

        //Overlap the various noisemaps
        float[,] overlapped = new float[size, size];

        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                overlapped[i, j] = 0.0f;

                foreach(Vector2 threshold in thresholdPoints) {
                    if(i + threshold.x < size && j + threshold.y < size) {
                        overlapped[i, j] += (1.0f - baseFalloff[i + (int)threshold.x, j + (int)threshold.y]);
                    }
                }

                overlapped[i, j] = Mathf.Clamp(overlapped[i, j], 0.0f, 1.0f);
                overlapped[i, j] = 1.0f - overlapped[i, j];
            }
        }

        return overlapped;
    }

    private static float evaluate(float value, float a, float b) {
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}