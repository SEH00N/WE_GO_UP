using System;
using System.Collections;
using UnityEngine;

public class CubeRolling : MonoBehaviour
{
    [SerializeField] float rollingDuration = 0.3f;

    private Transform cube;
    private Rigidbody rb = null;

    private bool onRolling = false;

    private void Awake()
    {
        cube = transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.zero;
            cube.position = new Vector3(0, -0.5f, -0.5f);
        }
    }

    public void DoRolling(Vector3 dir)
    {
        if(onRolling)
            return;

        cube.SetParent(null);
        Vector3 axisPos = cube.position;

        transform.rotation = Quaternion.LookRotation(dir);

        if(Physics.Raycast(cube.position, dir, out RaycastHit hit, 1, 1 << 6)) //옆에 큐브가 있는 상태 -> 올라가야 되는 상태
        { 
            //큐브와 플레이어가 만난 쪽 위 모서리를 알아야됨
            axisPos.y += cube.localScale.y * 0.5f;
            axisPos += dir * cube.localScale.y * 0.5f;

            transform.position = axisPos;
            cube.SetParent(transform);

            StartCoroutine(RollingCoroutine(dir, true));
        }
        else //보는 방향에 큐브가 없는 상태
        {
            //큐브와 플레이어가 만난 쪽 아레 모서리를 알아야됨
            axisPos.y -= cube.localScale.y * 0.5f;
            axisPos += dir * cube.localScale.y * 0.5f;

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

        cube.localPosition = cube.localPosition.Round(1);
        transform.position = transform.position.Round(1);

        while(timer < rollingDuration)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timer / rollingDuration);

            yield return new WaitForEndOfFrame();
        }

        cube.localScale = Vector3.one;
        transform.position = transform.position.Round(1);

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
