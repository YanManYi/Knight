using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static Transform DG_FindChild(this Transform transform, string childName, Transform parent)
    {
        if (parent.name == childName)
        {
            return parent;
        }

        if (parent.childCount == 0)
        {
            return null;
        }

        Transform obj = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            obj = transform.DG_FindChild(childName, parent.GetChild(i));
            if (obj) break;
        }

        return obj;

    }

}
