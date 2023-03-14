using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector3 startPos = Vector3.zero;
    private CubeRolling cr = null;

    private void Awake()
    {
        cr = GetComponent<CubeRolling>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;
        else if(Input.GetMouseButtonUp(0))
            cr.DoRolling(DirectionDecision(startPos, Input.mousePosition));
    }

    // 입력이 x 또는 y 축에 존재할 때 사분면을 통일시키 위해 크거나 작다 또는 작거나 같다로 구분했다
    private Vector2 DirectionDecision(Vector3 startPos, Vector3 endPos)
    {
        Vector3 modifiedValue = endPos - startPos;
        Vector2 dir = Vector2.zero;

        if(modifiedValue.x >= 0 && modifiedValue.y >= 0)
            dir = new Vector2(1, 1); // 1사분면
        else if(modifiedValue.x <= 0 && modifiedValue.y >= 0)
            dir = new Vector2(-1, 1); //2사분면
        // else if(modifiedValue.x <= 0 && modifiedValue.y <= 0)
        //     dir = new Vector2(-1, -1); //3사분면
        // else if(modifiedValue.x >= 0 && modifiedValue.y <= 0)
        //     dir = new Vector2(1, -1); //4사분면

        return dir;
    }
}
