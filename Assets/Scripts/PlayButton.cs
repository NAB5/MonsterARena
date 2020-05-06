using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public float bounce = 10f;
    public float speed = 1f;

    RectTransform transform;
    float currentY;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<RectTransform>();
        currentY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float val = bounce * Mathf.Sin(Time.time * speed);

        transform.position = new Vector3(transform.position.x, currentY + val, transform.position.z);
    }

    public void playScene()
    {

    }
}
