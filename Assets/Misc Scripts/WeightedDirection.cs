using UnityEngine;
using System.Collections;

public class WeightedDirection {

    public enum Type { Blend, Exclusive, Fallback }

    public readonly Vector3 direction;
    public readonly float weight;
    public readonly Type type;

    public WeightedDirection(Vector3 dir, float wgt)
    {
        direction = dir.normalized;
        weight = wgt;
        type = Type.Blend;
    }

    public WeightedDirection(Vector3 dir, float wgt, Type tp)
    {
        direction = dir.normalized;
        weight = wgt;
        type = tp;
    }
}
