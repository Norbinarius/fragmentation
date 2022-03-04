using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameProgresser : MonoBehaviour
{
    private static int gameState = 0;
    public static bool isArenaSpawnActive;
    private static bool isArenasPostionsRandomized;
    [SerializeField] private TextMeshPro sectors;
    [SerializeField] private TextMeshPro coordinatesSectors;

    [Header("Arenas in order of progress")]
    [SerializeField] private ArenaActivator[] arenas;

    [Space(15)]
    private Queue<ArenaActivator> arenasQueue;
    private Vector3[] arenasPostions;
    [SerializeField] private FinalArena finalArena;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject shipSpawner;
    public static GameProgresser current;

    private void Awake()
    {
        current = this;
        arenasQueue = new Queue<ArenaActivator>(arenas);
    }

    private void Start()
    {
        ApplyArenasPositions();
        ApplyGameProgress();
        ApplySpawnPostion();
    }

    private void ApplyArenasPositions()
    {
        if(isArenasPostionsRandomized)
            return;

        arenasPostions = new Vector3[arenas.Length];

        for(int i = 0; i < arenas.Length; i++)
        {
            arenasPostions[i] = new Vector3(
                Random.Range(arenas[i].PosX.x, arenas[i].PosX.y), 
                Random.Range(arenas[i].PosY.x, arenas[i].PosY.y), 0);
        }

        isArenasPostionsRandomized = true;

        for(int i = 0; i < arenas.Length; i++)
        {
            arenas[i].transform.position = arenasPostions[i];
        }
    }

    public void ApplyGameProgress()
    {
        SetArenaActive();
        sectors.text = $"Секторов восстановлено:\n{gameState}/{arenas.Length}";

        if(arenasQueue.Count == 0)
            return;

        coordinatesSectors.text = "Восстановите все битые\nсектора. координаты:\n";

        for(int j = 0; j < gameState; j++)
        {
            coordinatesSectors.text += $"{j + 1}.   ВОССТАНОВЛЕНО\n";
        }

        coordinatesSectors.text += $"{gameState + 1}.   {GetCoordinates()}\n";

        for(int j = gameState + 1; j < arenas.Length; j++)
        {
            coordinatesSectors.text += $"{j + 1}.   ??????????\n";
        }
    }

    public void ApplySpawnPostion()
    {
        UpdateShipPostion(isArenaSpawnActive ? 
            new Vector3((int)(arenasQueue.Peek().transform.position.x), (int)(arenasQueue.Peek().transform.position.y), 0) : 
            Vector3.zero);
    }

    private string GetCoordinates()
    {
        return "X: " + ((int)(arenas[gameState].transform.position.x)).ToString() + "  " + "Y: " + ((int)(arenas[gameState].transform.position.y)).ToString();
    }

    private void UpdateShipPostion(Vector3 newPos)
    {
        cameraFollow.transform.position = new Vector3(newPos.x, newPos.y, CameraFollow.Z_OFFSET);
        shipSpawner.transform.position = newPos;
    }

    public void SetArenaActive()
    {
        foreach(var area in arenas)
        {
            area.SetEnabled(false);
        }

        for(int i = 0; i < gameState; i++)
        {
            arenasQueue.Dequeue();
        }

        if(arenasQueue.Count == 0)
        {
            finalArena.gameObject.SetActive(true);
            coordinatesSectors.text = "Все поврежденные сектора\nуспешно восстановлены.\n Х: 92  Y: 78";
            return;
        }

        Debug.Log(arenasQueue.Peek());
        arenasQueue.Peek().SetEnabled(true);
    }

    public void SaveProgress()
    {
        gameState++;
        isArenaSpawnActive = false;
        ApplyGameProgress();
    }

    public void ResetProgress()
    {
        gameState = 0;
        isArenaSpawnActive = false;
        isArenasPostionsRandomized = false;
    }

}
