using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager instance { get; set; }

    public GameObject interaction_Info_UI;
    public GameObject selectedObject;
    TextMeshProUGUI interaction_text;
    public bool onTarget;
    Transform selectionTransform;
    RaycastHit hit;


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit))
        {
            onTarget = true;
            var interactable = hit.transform.GetComponent<InteractableObject>();
            selectionTransform = hit.transform;
            // selectedObject = interactable.gameObject;
            NPCInteract();

            Debug.Log("Raycast hit object: " + hit.transform.name);


            if (interactable != null)
            {
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                return;
            }
        }
        onTarget = false;
        interaction_Info_UI.SetActive(false);
    }

    void NPCInteract()
    {
        NPC npc = selectionTransform.GetComponent<NPC>();


        if (npc == null) { return; }

        if (npc && npc.playerInRange)
        {
            interaction_text.text = "Talk";
            interaction_Info_UI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return) && !npc.isTalking)
            {
                npc.StartConversation();
            }

            if (DialogSystem.instance.isOpen)
            {
                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {
            interaction_text.text = "";
            interaction_Info_UI.SetActive(false);
        }
    }
}