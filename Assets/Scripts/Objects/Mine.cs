using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float explosionForce;
    public ParticleSystem explosionParticles;


    private GameObject player;

    private void Start()
    {
        player = GameManager.instance.GetPlayerTransform().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, other.transform.position, explosionForce, explosionForce, ForceMode.Impulse);
            player.GetComponent<CubeJump>().ResetJumpCount();
            StartCoroutine(BlowUp());
        }
    }
 
    IEnumerator BlowUp()
    {
        explosionParticles.Play();
        AudioManager.instance.PlayAudioOneShot("Mine");
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
