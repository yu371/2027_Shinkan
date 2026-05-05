using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RigidScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMove = true;
    private float time;
    public float speed = 2;
    private BanPosScript banPosScript;
    public List<Vector2> myBanPos;
    private SystemScript systemScript;
    void Start()
    {
        banPosScript = GameObject.FindWithTag("System").GetComponent<BanPosScript>();
        systemScript = GameObject.FindWithTag("System").GetComponent<SystemScript>();
        foreach (Transform child in transform)
        {
            myBanPos.Add(child.localPosition);
        }
    }
    public bool MoveCheck(float x)
    {
        foreach (Vector2 pos in myBanPos)
        {
            Vector2 center = transform.position;
            Vector2 p = SnapToHalf(pos + new Vector2(x, 0) + center);
            if (banPosScript.banpos.Contains(p))
            {
                return false;
            }
        }
        return true;
    }
    public bool RotateCheck()
    {
        Vector2 center = transform.position;
        float angle = 90f;

        List<Vector2> rotatedPositions = new List<Vector2>();

        foreach (Vector2 pos in myBanPos)
        {
            Vector2 rotated = RotatePoint(pos + center, center, angle);
            rotatedPositions.Add(rotated);
        }
        foreach (Vector2 pos in rotatedPositions)
        {
            if (banPosScript.banpos.Contains(pos))
            {
                return false;
            }
        }
        myBanPos = rotatedPositions
     .Select(x => SnapToHalf(x - (Vector2)transform.position))
     .ToList();

        return true;
    }

    Vector2 SnapToHalf(Vector2 v)
    {
        return new Vector2(
            Mathf.Round(v.x * 2f) / 2f,
            Mathf.Round(v.y * 2f) / 2f
        );
    }

    Vector2 RotatePoint(Vector2 point, Vector2 center, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector2 relative = point - center;

        float x = relative.x * cos - relative.y * sin;
        float y = relative.x * sin + relative.y * cos;

        return new Vector2(x, y) + center;
    }
    void FixedUpdate()
    {
        if (isMove == false) return;
        time += Time.deltaTime * speed;
        if (time >= 1)
        {
            time = 0;
            Vector3 targetPos = transform.position - new Vector3(0, 1, 0);
            targetPos = new Vector3(
    Mathf.Round(targetPos.x),
    Mathf.Round(targetPos.y),
    0
    );
            foreach (Vector2 pos in myBanPos)
            {
                if (banPosScript.banpos.Contains((Vector2)targetPos + pos))
                {

                    isMove = false;

                    banPosScript.banpos.AddRange(
                        myBanPos.Select(x => x + new Vector2(
                            Mathf.Round(transform.position.x),
                            Mathf.Round(transform.position.y)
                        ))
                    );
                    for (int i = transform.childCount - 1; i >= 0; i--)
                    {

                        banPosScript.banObjs.Add(transform.GetChild(i).gameObject);
                        transform.GetChild(i).SetParent(null);
                    }
                    banPosScript.Drop();
                    break;
                }
            }
            if (isMove)
                transform.position = targetPos;


        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        isMove = false;
    }

}
