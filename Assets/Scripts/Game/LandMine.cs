using UnityEngine;
using System.Collections;

public class LandMine : MonoBehaviour {

    private bool armed;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(-transform.forward * 5, ForceMode.VelocityChange);
        StartCoroutine(ArmMine());
    }

    IEnumerator ArmMine()
    {
        yield return new WaitForSeconds(1);
        armed = true;
    }

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && armed)
        {
            Debug.Log("Boom");
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddExplosionForce(25, transform.position, 10, 0, ForceMode.VelocityChange);
            Destroy(gameObject);
        }
    }
}
