using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collisions 
{
    public const float SKIN_WIDTH = 0.06f;// 0.01f;
    public static void CalculateRaySpacing(Collider collider,ref RayCastOrigins rayCastOrigins)
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);
        rayCastOrigins.HorizontalRayCount = Mathf.Clamp(rayCastOrigins.HorizontalRayCount, 2, int.MaxValue);
        rayCastOrigins.VerticalRayCount = Mathf.Clamp(rayCastOrigins.VerticalRayCount, 2, int.MaxValue);
        rayCastOrigins.HorizontalRaySpacing = bounds.size.y / (rayCastOrigins.HorizontalRayCount - 1);
        rayCastOrigins.VerticalRaySpacing = bounds.size.x / (rayCastOrigins.VerticalRayCount - 1);
    }
    public static void UpdateRayCastOrigins(Collider collider, ref RayCastOrigins rayCastOrigins)
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);
        rayCastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }
}
