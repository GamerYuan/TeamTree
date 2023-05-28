using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PromoVidBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject tree1, updateButton;

    private Bonsai bonsai1;

    void Awake()
    {
        bonsai1 = tree1.GetComponent<Bonsai>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetButtonActive();
        }
    }

    public void UpdateTicks()
    {
        bonsai1.TreeUpdate();
    }

    private void SetButtonActive()
    {
        updateButton.SetActive(!updateButton.activeSelf);
    }

}
