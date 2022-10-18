using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistace = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;
    
   // int maxZpos;
    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    TMP_Text gameOverText;
    
    private void Start()
    {
        //setup gameover panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        for (int z = backDistace; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        for (int z = 1; z <= frontDistance; z++)
        {
            //dapetin block dengan probabilitas 50 %
            var prefab = GetNextTerrainPrefab(z);

            CreateTerrain(prefab, z);
        }
        player.SetUp(backDistace,extent);
    }

    private int playerLastMaxTravel;
        private void Update()
        {
            //cek player masih hidup atau tidak
            if (player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine (ShowGameOverPanel());

            //infinite terrain
            if(player.MaxTravel == playerLastMaxTravel)
                return;

            playerLastMaxTravel = player.MaxTravel;

            //bikin ke depan
            var randTbPrefab = GetNextTerrainPrefab(player.MaxTravel + frontDistance);
            CreateTerrain(randTbPrefab,player.MaxTravel + frontDistance);
            //hapus yang dibelakang
            var lastTB = map[player.MaxTravel-1 + backDistace];

            map.Remove(player.MaxTravel-1+backDistace);
            Destroy(lastTB.gameObject);

            player.SetUp(player.MaxTravel+backDistace,extent);

        }

        IEnumerator ShowGameOverPanel()
        {
            yield return new WaitForSeconds(2);
            //player.enabled = false;
            Debug.Log("GameOver");
            gameOverText.text = "YOUR SCORE : " + player.MaxTravel;
            gameOverPanel.SetActive(true);
        }

    private void CreateTerrain(GameObject prefab, int zPos )
    {
        var go = Instantiate(prefab, new Vector3(0,0,zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if(map[nextPos - distance].GetType() != tbRef.GetType())
            { 
                isUniform = false;
                break; 
            }
        }

        if(isUniform)
        {
            if(tbRef is Grass)
                return road;
            else
                return grass;
        }

        return Random.value > 0.5f ? road : grass;
    }

}
