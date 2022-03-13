using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    public float acceleration;
    public float decceleration;

    public Vector3 rawInput;
    public Vector3 calculatedInput;

    public float speed;

    public bool canRotate;
    public float turnSpeed;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleInputs();
        anim.SetFloat("Vertical", calculatedInput.z);
        anim.SetFloat("Horizontal", calculatedInput.x);
        Vector3 moveVector = transform.TransformDirection(calculatedInput * speed) * Time.deltaTime;
        moveVector.y = 0;
        rb.velocity = moveVector;

        HandleRotation();

        if(Input.GetMouseButtonDown(0))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.root.GetComponent<NPC>())
                {
                    NPC npc = hitCollider.transform.root.GetComponent<NPC>();
                    npc.StartCoroutine(npc.trip());
                }
            }
        }
    }

    //CALCULATES AND SETS RAW INPUT AND CALCULATED INPUT
    //CHANGES ANIMATOR TO PLAY CORRECT ANIMATION
    void HandleInputs()
    {
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (rawInput.magnitude != 0)
            calculatedInput = new Vector3(Mathf.MoveTowards(calculatedInput.x, rawInput.x, acceleration * Time.deltaTime), 0, Mathf.MoveTowards(calculatedInput.z, rawInput.z, acceleration * Time.deltaTime));
        else
            calculatedInput = new Vector3(Mathf.MoveTowards(calculatedInput.x, 0, acceleration * Time.deltaTime), 0, Mathf.MoveTowards(calculatedInput.z, 0, decceleration * Time.deltaTime));

        calculatedInput = Vector3.ClampMagnitude(calculatedInput, 1.5f);
    }

    void HandleRotation()
    {
        if (!canRotate)
            return;
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;

        if (rawInput.magnitude != 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}