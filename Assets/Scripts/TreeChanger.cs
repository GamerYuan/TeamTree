using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TreeChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject brocolliTree, tiltedTree, normalTree;
    [SerializeField] private Transform flowerPot;
    public void ChangeTree(TMP_Dropdown dropDown)
    {
        int treeType = dropDown.value;
        GameObject treeObject = GameObject.FindGameObjectWithTag("Tree");
        Destroy(treeObject);
        switch (treeType)
        {
            case 0:
                Instantiate(brocolliTree, flowerPot);
                break;
            case 1:
                Instantiate(tiltedTree, flowerPot);
                break;
            case 2:
                Instantiate(normalTree, flowerPot);
                break;
        }
    }
    
}
