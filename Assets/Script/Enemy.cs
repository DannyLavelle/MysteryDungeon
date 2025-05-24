using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public IEnumerator TakeTurn()
    {
        // Your enemy logic here (move, attack…), then yield
        yield return new WaitForSeconds(0.1f);
    }
}
