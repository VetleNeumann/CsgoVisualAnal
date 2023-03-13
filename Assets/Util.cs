using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static Vector3 DemoInfoVecToVec3(DemoInfo.Vector vec)
    {
        return new Vector3(vec.X, vec.Z, vec.Y);
    }

    public static Vector3 DemoInfoVecToVec2(DemoInfo.Vector vec)
    {
        return new Vector2(vec.X, vec.Y);
    }
}
