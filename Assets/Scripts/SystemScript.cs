using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SystemScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject block;
    public List<GameObject> blocks;
    private RigidScript rigidScript;
    private float time;
    public TextMeshProUGUI textMeshProUGUI;
    private int point;
    private bool isReset;
    public TextMeshProUGUI scoreText;
    public GameObject GameOverPanel;
    private float x;
    void Start()
    {
        Instance();
        textMeshProUGUI.text = point.ToString() + "pt";
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        isReset = true;
        scoreText.text = "Your Point:" + point.ToString() + "pt";
        GameOverPanel.SetActive(true);
    }
    public void Reset()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void GetPoint()
    {
        //Q5 ポイントの加算
        //Q6　ポイントの更新
    }
    public void Instance()
    {
        //Q4 乱数の設定
        int r = 0;
        GameObject obj = Instantiate(blocks[r], new Vector3(0, 20, 0), Quaternion.identity);
        block = obj;
        rigidScript = obj.GetComponent<RigidScript>();
    }
    public void Rotate()
    {
        if (rigidScript.RotateCheck() == true)
        {
            //Q3 オブジェクトの回転
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }
        if (isReset == true) return;
        time += Time.deltaTime;
        //Q1 x方向の入力の取得

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Rotate();
        }
        if (time > 0.1 && rigidScript.MoveCheck(x))
        {
            time = 0;
            //Q2 blockをx方向に動かす

        }
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 5f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}

//ANSWER
//A1 : x = Input.GetAxisRaw("Horizontal");
//A2 : block.transform.position += new Vector3(x, 0, 0);
//A3 : block.transform.Rotate(0, 0, 90);
//A4 : int r = Random.Range(0, blocks.Count);
//A5 : point += 100;
//A6 : textMeshProUGUI.text = point.ToString() + "pt";