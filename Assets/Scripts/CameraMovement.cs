using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    private Vector3 target;
    private Camera cam;

    private float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            target += new Vector3(0, speed * Time.deltaTime);
        }
        else if (Input.GetKey("s"))
        {
            target += new Vector3(0, -speed * Time.deltaTime);
        }

        if (Input.GetKey("a"))
        {
            target += new Vector3(-speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey("d"))
        {
            target += new Vector3(speed * Time.deltaTime, 0);
        }
        transform.position = Vector3.Lerp(transform.position, target, 0.25f);
        //transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, 0.5f);
    }
}
