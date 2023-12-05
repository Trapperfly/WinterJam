using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveModifier;
    [SerializeField] float groundedModifier;
    [SerializeField] float aerialModifier;

    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;

    [SerializeField] float jumpStrength;
    [SerializeField] float doubleJumpStrength;
    [SerializeField] float groundDrag;
    Rigidbody rb;
    [SerializeField] int jumps;
    int localJumps;

    [SerializeField] float fallGravity;
    [SerializeField] float floatGravity;
    [SerializeField] float slamGravity;
    [SerializeField] float gravity;

    [SerializeField] LayerMask ground;
    [SerializeField] float playerHeight;
    public bool grounded;

    [SerializeField] float aerialMovementLoss;

    private void Awake()
    {
        localJumps = jumps;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DoGravity();
        CheckIfGrounded();
        HandlePhysics();
        HandleInputs();

        if (grounded) localJumps = jumps;
        if (grounded) moveModifier = groundedModifier;
        else moveModifier -= moveModifier * (Time.deltaTime * aerialMovementLoss);
    }

    void DoGravity()
    {
        rb.AddForce(-transform.up * gravity * Time.deltaTime);
    }

    void HandleInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * moveSpeed * moveModifier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * moveSpeed * moveModifier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * moveSpeed * moveModifier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * moveSpeed * moveModifier * Time.deltaTime);
        }
        //Jumping
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
        }
        else if (localJumps > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            localJumps--;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
            rb.AddForce(transform.up * doubleJumpStrength, ForceMode.Impulse);
            moveModifier = groundedModifier;
        }

        //Slamming
        if (!grounded && Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(nameof(Slamming));
        }
        else if (!grounded && Input.GetKey(KeyCode.Space))
        {
            gravity = floatGravity;
            aerialMovementLoss = 0.5f;
        }
        else
        {
            gravity = fallGravity;
            aerialMovementLoss = 1;
        }
    }

    IEnumerator Slamming()
    {
        gravity = slamGravity;
        yield return null;
        if (!grounded) StartCoroutine(nameof(Slamming));
    }

    void CheckIfGrounded()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
    }

    void HandlePhysics()
    {
        rb.drag = grounded ? groundDrag : 0;

    }
}
