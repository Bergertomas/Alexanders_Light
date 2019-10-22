using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static float MapRange(float currentValueA, float maxValueA, float minValueB, float maxValueB)
    {
        return ((currentValueA / maxValueA) * (maxValueB - minValueB)) + minValueB;  
    }
}
