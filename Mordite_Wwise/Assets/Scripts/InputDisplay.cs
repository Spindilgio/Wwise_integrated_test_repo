using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INPUT_ANIM
{
    SHAKE,
    WAVE,
    NONE
}

public class InputDisplay : MonoBehaviour
{
    private Vector2 init_pos;
    private INPUT_ANIM current_anim;
    private float target_pos;
    private int anim_part;

    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position;
        current_anim = INPUT_ANIM.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_anim == INPUT_ANIM.NONE) return;

        switch (current_anim)
        {
            case INPUT_ANIM.SHAKE:
                float x = Mathf.Lerp(transform.position.x, target_pos, 25 * Time.deltaTime);
                transform.position = new Vector3(x, transform.position.y);
                if (transform.position.x >= target_pos - 0.1f && transform.position.x <= target_pos + 0.1f)
                {
                    anim_part++;
                    switch (anim_part)
                    {
                        case 1:
                            target_pos = init_pos.x + 0.5f;
                            break;
                        case 2:
                            target_pos = init_pos.x;
                            break;
                        case 3:
                            current_anim = INPUT_ANIM.NONE;
                            break;
                    }
                }
                break;
            case INPUT_ANIM.WAVE:
                float y = Mathf.Lerp(transform.position.y, target_pos, 6 * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, y);
                if (transform.position.y >= target_pos - 0.1f && transform.position.y <= target_pos + 0.1f)
                {
                    anim_part++;
                    switch (anim_part)
                    {
                        case 1:
                            target_pos = init_pos.y;
                            break;
                        case 2:
                            current_anim = INPUT_ANIM.NONE;
                            break;
                    }
                }
                break;
        }
    }

    public void StartShake()
    {
        current_anim = INPUT_ANIM.SHAKE;
        anim_part = 0;
        target_pos = init_pos.x - 0.5f;
        transform.position = init_pos;
    }

    public void StartWave()
    {
        current_anim = INPUT_ANIM.WAVE;
        anim_part = 0;
        target_pos = init_pos.y + 0.5f;
        transform.position = init_pos;
    }
}
