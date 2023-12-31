using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionStrength;
    [SerializeField] LayerMask objects;

    [Header("Speeds")]
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float drag;

    [Header("Jumping")]
    [SerializeField] float jumpStrength;
    [SerializeField] float doubleJumpStrength;
    [SerializeField] float groundDrag;
    [SerializeField] int jumps;
    int localJumps;
    float aerialMovementLoss;

    [Header("Gravity")]
    [SerializeField] float fallGravity;
    [SerializeField] float floatGravity;
    [SerializeField] float slamGravity;
    [SerializeField] float gravity;
    bool slamming;

    [Header("GroundCheck")]
    [SerializeField] LayerMask ground;
    [SerializeField] float playerHeight;
    public bool grounded;

    [Header("Modifiers")]
    [SerializeField] float moveModifier;
    [SerializeField] float groundedModifier;

    [Header("Upgrade States")]
    [SerializeField] bool placeholderUpgrade;
    [SerializeField] bool moreJumps;
    [SerializeField] bool slowFall;
    [SerializeField] bool slam;

    private void Awake()
    {
        localJumps = jumps;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DoGravity();
        DoDrag();
        CheckIfGrounded();
        HandlePhysics();
        HandleInputs();

        if (grounded) localJumps = jumps;
        if (grounded) rb.mass = 1;
        if (grounded) moveModifier = groundedModifier;
        else
        {
            moveModifier -= moveModifier * (Time.smoothDeltaTime * aerialMovementLoss);
        }
        if (moveModifier < 0.2f) moveModifier = 0.2f;
    }
    void DoDrag()
    {
        float timedDrag = 1 - (drag * Time.smoothDeltaTime);
        var velocity = rb.velocity;
        velocity.x *= timedDrag;;
        velocity.z *= timedDrag;
        rb.velocity = velocity;
    }
    void DoGravity()
    {
        rb.AddForce(-transform.up * gravity * Time.smoothDeltaTime);
    }

    void HandleInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * moveSpeed * moveModifier * Time.smoothDeltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * moveSpeed * moveModifier * Time.smoothDeltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * moveSpeed * moveModifier * Time.smoothDeltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * moveSpeed * moveModifier * Time.smoothDeltaTime);
        }
        //Jumping
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
        }
        else if (moreJumps && localJumps > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            localJumps--;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.1f, rb.velocity.z);
            rb.AddForce(transform.up * doubleJumpStrength, ForceMode.Impulse);
            moveModifier = groundedModifier;
        }

        //Slamming
        if (slam && !grounded && Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!slamming) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            slamming = true;

            StartCoroutine(nameof(Slamming));
        }
        else if (slowFall && !grounded && Input.GetKey(KeyCode.Space))
        {
            gravity = floatGravity;
            aerialMovementLoss = 0.2f;
        }
        else
        {
            gravity = fallGravity;
            aerialMovementLoss = 0.5f;
        }
    }

    IEnumerator Slamming()
    {
        gravity = slamGravity;
        yield return null;
        if (!grounded) StartCoroutine(nameof(Slamming));
        else
        {
            slamming = false;
            Vector3 offsetPos = new(transform.position.x, transform.position.y - 1, transform.position.z);
            Collider[] exploded = Physics.OverlapSphere(transform.position, explosionRadius, objects);
            foreach (var go in exploded)
            {
                Rigidbody goRb = go.GetComponent<Rigidbody>();
                if (goRb)
                {
                    goRb.AddExplosionForce(explosionStrength, transform.position, explosionRadius);
                }
            }
        }
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
