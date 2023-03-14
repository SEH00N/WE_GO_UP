using UnityEngine;

public class CubeRolling : MonoBehaviour
{
    [SerializeField] Transform cube;

    private Vector3[] pivots = { new (-0.5f, -0.5f, 0), new (0, -0.5f, -0.5f) };

    public void DoRolling(Vector2 dir)
    {
        Debug.Log(dir);
        cube.SetParent(null);

        if(dir != new Vector2(1, 1) && dir != new Vector2(-1, 1))
            return;

        cube.position = pivots[dir == new Vector2(1, 1) ? 0 : 1];
        cube.SetParent(transform);
    }
}
