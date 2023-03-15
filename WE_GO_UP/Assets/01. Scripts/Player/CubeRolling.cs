using System.Collections;
using UnityEngine;

public class CubeRolling : MonoBehaviour
{
    [SerializeField] Transform cube;
    [SerializeField] bool onRolling = false;

    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void DoRolling(Vector3 dir)
    {
        if(onRolling)
            return;

        if(Physics.Raycast(cube.position, dir, out RaycastHit hit, 1, 1 << 6)) //옆에 큐브가 있는 상태 -> 올라가야 되는 상태
        { 
            Debug.Log(dir);
            cube.SetParent(null);
            //큐브와 플레이어가 만난 쪽 위 모서리를 알아야됨
            Vector3 axisPos = cube.position;
            axisPos.y += cube.localScale.y * 0.5f;
            axisPos += dir * cube.localScale.y * 0.5f;

            transform.rotation = Quaternion.LookRotation(dir);
            transform.position = axisPos;
            cube.SetParent(transform);

            StartCoroutine(RollingCoroutine(dir, true));
        }
        else //보는 방향에 큐브가 없는 상태
        {
            cube.SetParent(null);
            //큐브와 플레이어가 만난 쪽 아레 모서리를 알아야됨
            Vector3 axisPos = cube.position;
            axisPos.y -= cube.localScale.y * 0.5f;
            axisPos += dir * cube.localScale.y * 0.5f;

            transform.rotation = Quaternion.LookRotation(dir);
            transform.position = axisPos;
            cube.SetParent(transform);

            //축 설정하는 거 해야됨
            StartCoroutine(RollingCoroutine(dir));
        }

        rb.useGravity = false;
    }

    private IEnumerator RollingCoroutine(Vector3 dir, bool looping = false)
    {
        onRolling = true;
        rb.useGravity = false;

        float timer = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(90f, Vector3.right);

        while(timer < 0.2f)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timer / 0.2f);

            yield return new WaitForEndOfFrame();
        }

        if(looping)
            StartCoroutine(RollingCoroutine(dir));
        else
        {
            onRolling = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 0.5f, ForceMode.Impulse);
        }
    }
}
