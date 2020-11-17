using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMotion : MonoBehaviour
{
    private float init_pos;
    private float target_pos;

    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position.y;
        target_pos = init_pos + 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Lerp(transform.position.y, target_pos, 3 * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, y);
        if (transform.position.y >= target_pos - 0.1 && transform.position.y <= target_pos + 0.1)
        {
            if (target_pos == init_pos)
            {
                target_pos = init_pos + 0.3f;
            }
            else
            {
                target_pos = init_pos;
            }
        }
    }
}
