using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TreeChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject brocolliTree, tiltedTree, normalTree;
    [SerializeField] private Transform flowerPot;
    [SerializeField] private GameObject customTreeTemplate;
    private int currentTree;
    private GameObject customTree;
    [SerializeField] private Material treeMat;
    [SerializeField] private Mesh treeMesh;
    [SerializeField] private Mesh2d crossSectionMesh;
    [SerializeField] private TMP_InputField rule1, rule2, rule3, rule4, axiomString;
    [SerializeField] private Slider segmentLength, rotation;

    private LSystem customlsystem;
    private LSystemConstants customConst;
    private RuleSet customRules;

    private void ChangeTree(int treeType)
    {
        GameObject treeObject = GameObject.FindGameObjectWithTag("Tree");
        Destroy(treeObject);
        currentTree = treeType;
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
            case 3:
                ConstructCustomTree();
                break;
        }
    }

    public void ChangeTree(TMP_Dropdown dropDown)
    {
        int treeType = dropDown.value;
        if (treeType == 3)
        {
            customTreeTemplate.SetActive(true);
            CallCustomTreeUI();
        } 
        else
        {
            ChangeTree(treeType);
        }
    }

    public void TreeUpdate()
    {
        GameObject currentTree = GameObject.FindGameObjectWithTag("Tree");
        Bonsai bonsai = currentTree.GetComponent<Bonsai>();
        bonsai.TreeUpdate();
    }

    public void TreeReset()
    {
        ChangeTree(currentTree);
    }

    public void CallCustomTreeUI()
    {
        StopTime();
        customTreeTemplate.SetActive(true);
    }

    public void SetTreeParams()
    {
        StartTime();
        List<string> rules = new List<string>
        {
            rule1.text, rule2.text, rule3.text, rule4.text
        };
        rules.RemoveAll(x => x == "");

        customRules = ScriptableObject.CreateInstance<RuleSet>();
        customRules.ruleStrings = rules;

        customlsystem = ScriptableObject.CreateInstance<LSystem>();
        customlsystem.axiomString = axiomString.text;
        customlsystem.ruleSet = customRules;

        customConst = ScriptableObject.CreateInstance<LSystemConstants>();
        customConst.SegmentLength = segmentLength.value;
        customConst.Rotation = rotation.value;

        ChangeTree(3);
    }

    private void ConstructCustomTree()
    {
        customTree = new GameObject();
        customTree.name = "CustomTree";
        customTree.AddComponent<MeshRenderer>();
        customTree.GetComponent<MeshRenderer>().material = treeMat;
        customTree.AddComponent<MeshFilter>();
        customTree.GetComponent<MeshFilter>().mesh = treeMesh;
        customTree.tag = "Tree";
        customTree.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

        customTree.AddComponent<Bonsai>();
        Bonsai customBonsai = customTree.GetComponent<Bonsai>();
        customBonsai.lsystem = customlsystem;
        customBonsai.period = 12;
        customBonsai.crossSection = crossSectionMesh;
        customBonsai.constants = customConst;

        customTree.transform.SetParent(flowerPot);
        customTree.transform.position = flowerPot.position;
    }

    public void CancelTreeConstruct(TMP_Dropdown dropdown)
    {
        dropdown.value = currentTree;
        StartTime();
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
    }

    private void StartTime()
    {
        Time.timeScale = 1f;
    }

}
