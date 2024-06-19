using UnityEngine;

public static class TagHelper
{
    public static bool TagExists(string tag)
    {
        try
        {
            GameObject.FindWithTag(tag);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
