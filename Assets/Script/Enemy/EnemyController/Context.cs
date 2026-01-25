using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context
{
    private float[] interest;
    public float[] Interest => interest;

    private float[] danger;
    public float[] Danger => danger;

    private int directions = 8;
    public int Directions => directions;
    Vector2[] dirs;
    public Vector2[] Dirs => dirs;

    public Context()
    {
        interest = new float[directions];
        danger = new float[directions];
        dirs = new Vector2[directions];

        for (int i = 0; i < directions; i++)
        {
            float angle = i * 360f / directions * Mathf.Deg2Rad;
            dirs[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
    public void SetInterestElement(int index, float val)
    {
        interest[index] = val;
    }
    public void SetDangerElement(int index, float val)
    {
        danger[index] = val;
    }

    public Vector2 GetDirection()
    {
        float bestChoice = float.MinValue;
        Vector2 bestDir = Vector2.zero;;
        for(int i = 0;  i < directions; i++)
        {
            float value = Mathf.Clamp01(interest[i] - danger[i]);
            if(value > bestChoice)
            {
                bestChoice = value;
                bestDir = dirs[i];
            }
        }
        return bestDir;
    }


}
