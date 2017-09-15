using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalInventorySettings {
	public const float INVENTORY_TILE_SIZE = 50f;

    public static float GetTileSizeForCanvas(float multiplier)
    {
        return INVENTORY_TILE_SIZE * multiplier;
    }

    public static float ScaleOffset(float offset, Transform t)
    {
        return offset * t.localScale.x;
    }
    
}
