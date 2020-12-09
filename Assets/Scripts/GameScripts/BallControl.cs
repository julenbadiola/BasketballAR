using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallControl : MonoBehaviour
{
    [SerializeField]
	private GameObject ballPrefab;
    [SerializeField]
    private Transform cam;
    private GameObject ball;

    public void initialize()
    {
        ball = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void Start()
    {
        initialize();
    }

    public IEnumerator resetBallAfterThrow()
    {
        //Wait 3 seconds (1.5 bc timeScale = 2) after throw and create new ball
        yield return new WaitForSeconds(3);
        Destroy(ball);
        initialize();
    }

}
