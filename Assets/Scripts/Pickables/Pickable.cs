using UnityEngine;

public abstract class Pickable : MonoBehaviour, IBuff
{

    protected Transform playerTransform;

    void Start()
    {
        playerTransform = GameManager.instance.GetPlayerTransform();
    }

    private bool goToPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            goToPlayer = true;
        }
    }

    public abstract void Buff();

    private void Update()
    {
        if (goToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 100f * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
            {
                Buff();
                Destroy(gameObject);
            }
        }
    }

}