using UnityEngine;
using System.Collections;

public class AnimateCounter : MonoBehaviour
{

    public float speed;
    int framekount = 0;
    int direction = 1;
    // Use this for initialization
    void Start()
    {
        speed = 3.0F;
    }

    // Update is called once per frame
    void Update()
    {

        framekount++;
        if (framekount == 250)
        {
            framekount = 0;
            direction = direction * 1;
        }
        Vector3 tv1, tv2;
        tv1.x = 0F;
        tv1.y = 1.5F;
        tv1.z = 0F;
        transform.Translate(tv1 * (Time.deltaTime * speed) * direction * -1.0F);

        tv2.x = -1F;
        tv2.y = 0F;
        tv2.z = 0F;
        transform.Rotate(tv2, .33F, Space.Self);
    }
}
