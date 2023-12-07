using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public float detractSpeed;
    public float retractSpeed;
    bool attacking;
    public float attackRange;
    public float moveSpeed;
    public Transform target;
    Material mat;
    [SerializeField] ParticleSystem ps;
    public float spikey;
    public float frequency;

    private void Start()
    {

        Random.InitState(GetInstanceID());
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (!attacking) transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < attackRange * transform.localScale.x)
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
}
