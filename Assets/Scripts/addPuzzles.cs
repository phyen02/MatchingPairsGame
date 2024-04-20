using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addPuzzles : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
    // thiết lập một prefab GameObject
    [SerializeField]
    private GameObject Button;
    GameObject btn;

    // Awake() only call once during this project
    // Use to initialize object need when start game
    void Awake()
    {
        for (int i = 0; i < 15; i++)
        {
            btn = Instantiate(Button);
            btn.name = "" + (i+1);
            // false -> Cho phép giữ transform của riêng object đó
            // true -> Lấy transform của parent
            btn.transform.SetParent(panel, false);
        }
    }
}
