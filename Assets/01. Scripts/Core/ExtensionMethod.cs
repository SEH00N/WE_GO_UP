using UnityEngine;

public static class ExtensionMethod
{
    public static Vector3 Round(this Vector3 vector3, int demicalPlace = 1)
    {
        float multiplier = 1;
        for (int i = 0; i < demicalPlace; i++)
            multiplier *= 10f;

        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

}
