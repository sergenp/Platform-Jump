using UnityEngine;

public class Pickable : MonoBehaviour, IBuff
{

    private bool goToPlayer;
    protected void GotoPlayer()
    {
        goToPlayer = true;
    }

    public virtual void Buff(){}

    private void Update()
    {
        if (goToPlayer)
        {
            Transform playerTransform = GameManager.instance.GetPlayerTransform();
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 20f * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
            {
                Buff();
                Destroy(gameObject);
            }
        }
    }

}