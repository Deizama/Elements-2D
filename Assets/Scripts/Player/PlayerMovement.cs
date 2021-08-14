using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;

    Rigidbody2D rb;

    PlayerAnimation pa;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pa = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float ralentissement = Mathf.Abs(Input.GetAxis("Vertical")) * Mathf.Abs(Input.GetAxis("Horizontal")) * 0.22f;

        float depHorizontal = speed * Time.fixedDeltaTime * (Input.GetAxis("Horizontal") - (Input.GetAxis("Horizontal")  * ralentissement));
        float depVertical = speed * Time.fixedDeltaTime * (Input.GetAxis("Vertical") - (Input.GetAxis("Vertical") * ralentissement));

        Vector3 deplacement = new Vector3(transform.position.x + depHorizontal, transform.position.y + depVertical, 0);
        rb.MovePosition(deplacement);

        pa.Animate(depHorizontal, depVertical);
    }


}
