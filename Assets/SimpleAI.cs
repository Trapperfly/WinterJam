using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleAI : MonoBehaviour
{
    public float startHealth;
    public float currentHealth;
    float healthPercent;
    public float detractSpeed;
    public float retractSpeed;
    bool attacking;
    bool isDead;
    float deadMove;
    public float attackRange;
    public float moveSpeed;
    public Transform target;
    Material mat;
    [SerializeField] ParticleSystem ps;
    public float spikey;
    public float frequency;

    [SerializeField] Transform tramplePs;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        startHealth *= Mathf.Pow(transform.localScale.x, 2);
        currentHealth = startHealth;
        healthPercent = currentHealth / startHealth;
        mat.SetFloat("_Health", 1 - healthPercent);
        target = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>().player;
        Random.InitState(GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (isDead) { transform.position += -transform.up * deadMove * Time.deltaTime; deadMove += 1 * Time.deltaTime; }
        if (attacking) { }
        else transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (!isDead && Vector3.Distance(transform.position, target.position) < attackRange * transform.localScale.x)
        {
            if (!attacking) StartCoroutine(nameof(Attack));
        }
    }

    IEnumerator Attack()
    {
        mat.SetFloat("_SpikeFrequency", Random.Range(10, 40));
        spikey = 0;
        attacking = true;
        while (spikey < 0.5f)
        {
            spikey += detractSpeed * Time.deltaTime;
            mat.SetFloat("_SpikeLength", spikey);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.2f);
        ps.Play();
        spikey = 5f;
        mat.SetFloat("_SpikeLength", spikey);
        yield return new WaitForSeconds(0.5f);
        while (spikey > 0)
        {
            spikey -= retractSpeed * Time.deltaTime;
            mat.SetFloat("_SpikeLength", spikey);
            yield return new WaitForEndOfFrame();
        }
        spikey = 0;
        mat.SetFloat("_SpikeLength", spikey);
        attacking = false;
        ps.Stop();
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        else if (collision.collider.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            currentHealth--;
            healthPercent = currentHealth / startHealth;
            mat.SetFloat("_Health", 1 - healthPercent);
            if (currentHealth < 0) { SimpleDie(); }
        }
    }

    void SimpleDie()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        tramplePs.parent = null;
        tramplePs.transform.position += new Vector3(0, -3,0);
        Destroy(tramplePs.gameObject, 10);
        isDead = true;
        Destroy(gameObject, 10);
    }
}
