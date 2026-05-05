using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BanPosScript : MonoBehaviour
{
    // Start is called before the first frame 
    public List<Vector2> banpos = new List<Vector2>();
    public List<GameObject> banObjs = new List<GameObject>();
    private SystemScript systemScript;
    void Start()
    {
        systemScript = GetComponent<SystemScript>();
        for (int i = -6; i < 6; i++)
        {
            banpos.Add(new Vector2(i + 0.5f, -0.5f));
        }
        for (int i = -1; i < 25; i++)
        {
            banpos.Add(new Vector2(-6.5f, i + 0.5f));
            banpos.Add(new Vector2(6.5f, i + 0.5f));
        }
    }
    public void Check()
    {
        banpos.Clear();
        for (int i = -6; i < 6; i++)
        {
            banpos.Add(new Vector2(i + 0.5f, -0.5f));
        }
        for (int i = -1; i < 25; i++)
        {
            banpos.Add(new Vector2(-6.5f, i + 0.5f));
            banpos.Add(new Vector2(6.5f, i + 0.5f));
        }
        foreach (GameObject obj in banObjs)
        {
            banpos.Add(SnapToHalf((Vector2)obj.transform.position));
        }
    }
    private bool isDelete;
    private float deletey = -100;

    void CheckSameY()
    {
        Dictionary<float, int> yCount = new Dictionary<float, int>();

        for (int i = 0; i < banpos.Count; i++)
        {
            Vector2 pos = banpos[i];
            float y = pos.y;

            if (y > 20 && pos.x < 6.5f && pos.x > -6.5f)
            {
                systemScript.GameOver();
            }
            if (y != -0.5f && pos.x > -6.5f && pos.x < 6.5f)
            {
                if (yCount.ContainsKey(y))
                    yCount[y]++;
                else
                    yCount[y] = 1;

                // 盤面の横幅に合わせる
                if (yCount[y] >= 12)
                {
                    deletey = y;
                    Debug.Log(y);
                    isDelete = true;
                    break;
                }
            }
        }

        if (isDelete && deletey != -100)
        {
            isDelete = false;
            systemScript.GetPoint();
            MoveDownAboveY(banObjs, deletey);
            deletey = -100;
        }
        else
        {
            systemScript.Instance();
        }
    }
    Vector2 SnapToHalf(Vector2 v)
    {
        return new Vector2(
            Mathf.Round(v.x * 2f) / 2f,
            Mathf.Round(v.y * 2f) / 2f
        );
    }
    void MoveDownAboveY(List<GameObject> banObjs, float targetY)
    {
        for (int i = banObjs.Count - 1; i >= 0; i--)
        {
            GameObject obj = banObjs[i];
            if (obj != null)
            {
                Vector2 pos = SnapToHalf(obj.transform.position);
                if (Mathf.Abs(pos.y - targetY) < 0.01f)
                {
                    banObjs.RemoveAt(i);
                    Destroy(obj);
                }
                else if (pos.y > targetY)
                {
                    banObjs[i].transform.position = SnapToHalf(new Vector2(pos.x, pos.y - 1f));
                }
            }
        }

        Invoke("Check", 0.2f);
        Invoke("Drop", 0.5f);
    }
    // Update is called once per frame
    public void Drop()
    {
        CheckSameY();
    }
}