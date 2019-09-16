using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFall : MonoBehaviour
{
    private int layermask;
    // Start is called before the first frame update
    void Start()
    {
        layermask = 1 << LayerMask.NameToLayer("SlipperyGround");
    }

    // Update is called once per frame
    void Update()
    {
        CheckStandOn();
    }

    private void CheckStandOn()
    {
        var CharacterMoveInfo = GetComponent<CharacterMoveInfo>();
        Vector2 OriPoint = (Vector2)transform.position + CharacterMoveInfo.PivotPoint;
        float ColliderWidth = CharacterMoveInfo.ColliderWidth;
        float ColliderHeight = CharacterMoveInfo.ColliderHeight;

        RaycastHit2D hit = Physics2D.Raycast(OriPoint, Vector2.down, ColliderHeight / 2, layermask);
        if (hit)
        {
            GetComponent<CharacterStateManager>().State = CharacterState.Fall;
            Destroy(hit.collider.gameObject);
        }
    }
}
