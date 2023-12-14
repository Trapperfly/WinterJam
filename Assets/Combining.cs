using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combining : MonoBehaviour
{
    public bool combinedThisFrame;

    public void CombineSlimes(GameObject slime0, GameObject slime1)
    {
        StartCoroutine(nameof(CombineCooldown));
        Debug.Log("Combining slimes " +  slime0 + " and " + slime1);
        
        GameObject tempSlime = slime0;
        Vector3 slimeSize0 = slime0.transform.localScale;
        Vector3 slimeSize1 = slime1.transform.localScale;

        Vector3 slimePos0 = slime0.transform.position;
        Vector3 slimePos1 = slime1.transform.position; 

        Destroy(slime1);
        Destroy(slime0);

        GameObject newSlime = Instantiate(tempSlime, Vector3.Lerp(slimePos0, slimePos1, 0.5f), new Quaternion(), null);

        newSlime.transform.localScale = ((slimeSize0 + slimeSize1) / 2) + ((slimeSize0 + slimeSize1) / 3);

        if (newSlime.transform.localScale.x > 4 || Random.Range(0, 2) < 1) newSlime.tag = "Untagged";

        newSlime.GetComponent<SimpleAI>().enabled = true;
        newSlime.GetComponentInChildren<AdjustToSize>().enabled = true;
    }
    IEnumerator CombineCooldown()
    {
        combinedThisFrame = true;
        yield return new WaitForSeconds(1f);
        combinedThisFrame = false;
        yield return null;  
    }
}
