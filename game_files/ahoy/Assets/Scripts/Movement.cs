using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Movement : MonoBehaviour
{
    public float wind;
    public float steering;
    public Rigidbody rb;
    public Transform trans;
    public GameObject vec;
    public float deadzone;
    public List<float> speed_levels;

    private float speed_tol = 0.01f;
    public float turn_control_tol = 0;

    private Vector3 velocity;
    private Vector3 acceleration;

    private int current_speed = 1;
    private float target_speed;

    private float steering_mod = 1;
    private float speed_mod = 1;
    private Text info;
    // Start is called before the first frame update
    void Start()
    {
      target_speed = speed_levels[current_speed];
      rb = GetComponent<Rigidbody>();
      rb.velocity = new Vector3(0,0,1);
      trans = GetComponent<Transform>();
      info = vec.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      //Update velocity vector

      //Add turning force
      float turn = Input.GetAxis("Horizontal");
      if (Mathf.Abs(turn) > turn_control_tol) {
        rb.AddTorque(transform.up * turn * steering);
      }
      rb.velocity = Vector3.ClampMagnitude(transform.forward,1);
      rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 0.5f);
      info.text = rb.velocity.ToString();
      UpdateModel();
    }

    void UpdateModel() {
      //trans.rotation = Quaternion.LookRotation(rb.velocity);
    }
}
