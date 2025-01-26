using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalking;

    TextMeshProUGUI npcDialog;

    Button option1;
    TextMeshProUGUI option1Text;
    Button option2;
    TextMeshProUGUI option2Text;

    [Header("Mission Management")]
    public List<BaseMission> missions;
    public BaseMission currentMission = null;
    public int activeMissionIndex = 0;
    public bool firstInteraction = true;
    public int currentDialog = 0;

    // Start is called before the first frame update
    void Start()
    {
        npcDialog = DialogSystem.instance.questText;

        option1 = DialogSystem.instance.firstOption;
        option2 = DialogSystem.instance.secondOption;

        option1Text = DialogSystem.instance.firstOption.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        option2Text = DialogSystem.instance.secondOption.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void StartConversation()
    {
        isTalking = true;
        if (firstInteraction)
        {
            firstInteraction = false;
            currentMission = missions[activeMissionIndex];
            StartMissionFirstConvo();
            currentDialog = 0;
        }
        else
        {
            // If we return after declining the quest
            if (currentMission.declined)
            {

                DialogSystem.instance.OpenDialogUI();

                npcDialog.text = currentMission.info.comebackAfterDecline;

                SetAcceptAndDeclineOptions();
            }


            // If we return while the quest is still in progress
            if (currentMission.accepted && currentMission.isComplete == false)
            {
                if (AreQuestRequirmentsCompleted())
                {

                    SubmitRequiredItems();

                    DialogSystem.instance.OpenDialogUI();

                    npcDialog.text = currentMission.info.comebackCompleted;

                    option1Text.text = "[Take Reward]";
                    option1.onClick.RemoveAllListeners();
                    option1.onClick.AddListener(() => {
                        ReceiveRewardAndCompleteMission();
                    });
                }
                else
                {
                    DialogSystem.instance.OpenDialogUI();

                    npcDialog.text = currentMission.info.comebackInProgress;

                    option1Text.text = "[Close]";
                    option1.onClick.RemoveAllListeners();
                    option1.onClick.AddListener(() => {
                        DialogSystem.instance.CloseDialogUI();
                        isTalking = false;
                    });
                }
            }

            if (currentMission.isComplete == true)
            {
                DialogSystem.instance.OpenDialogUI();

                npcDialog.text = currentMission.info.finalWords;

                option1Text.text = "[Close]";
                option1.onClick.RemoveAllListeners();
                option1.onClick.AddListener(() => {
                    DialogSystem.instance.CloseDialogUI();
                    isTalking = false;
                });
            }

            // If there is another quest available
            if (currentMission.firstConvoComplete == false)
            {
                StartMissionFirstConvo();
            }

        }


    }

    private void SubmitRequiredItems()
    {
        string firstRequiredItem = currentMission.info.firstRequirmentItem;
        int firstRequiredAmount = currentMission.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }


        string secondRequiredItem = currentMission.info.secondRequirmentItem;
        int secondRequiredAmount = currentMission.info.secondRequirementAmount;

        if (secondRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondRequiredItem, secondRequiredAmount);
        }

    }

    private bool AreQuestRequirmentsCompleted()
    {
        print("Checking Requirments");

        // First Item Requirment

        string firstRequiredItem = currentMission.info.firstRequirmentItem;
        int firstRequiredAmount = currentMission.info.firstRequirementAmount;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == firstRequiredItem)
            {
                firstItemCounter++;
            }
        }

        // Second Item Requirment -- If we dont have a second item, just set it to 0

        string secondRequiredItem = currentMission.info.secondRequirmentItem;
        int secondRequiredAmount = currentMission.info.secondRequirementAmount;

        var secondItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == secondRequiredItem)
            {
                secondItemCounter++;
            }
        }

        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartMissionFirstConvo()
    {
        DialogSystem.instance.OpenDialogUI();

        npcDialog.text = currentMission.info.initialDialog[currentDialog];
        option1Text.text = "Next";
        option1.onClick.RemoveAllListeners();
        option1.onClick.AddListener(() =>
        {
            currentDialog++;
            CheckIfDialogIsDone();
        });

        option2.gameObject.SetActive(false);
    }

    private void SetAcceptAndDeclineOptions()
    {
        option1Text.text = currentMission.info.acceptOption;
        option1.onClick.RemoveAllListeners();
        option1.onClick.AddListener(() => {
            AcceptedMission();
        });

        option2.gameObject.SetActive(true);
        option2Text.text = currentMission.info.declineOption;
        option2.onClick.RemoveAllListeners();
        option2.onClick.AddListener(() => {
            DeclinedMission();
        });
    }

    private void DeclinedMision()
    {
        currentMission.declined = true;

        npcDialog.text = currentMission.info.declineAnswer;
        CloseDialogUI();
    }

    private void AcceptedMission()
    {
        currentMission.accepted = true;
        currentMission.declined = false;

        if (currentMission.hasNoRequirements)
        {
            npcDialog.text = currentMission.info.comebackCompleted;
            option1Text.text = "[Take Reward]";
            option1.onClick.RemoveAllListeners();
            option1.onClick.AddListener(() => {
                ReceiveRewardAndCompleteMission();
            });
            option2.gameObject.SetActive(false);
        }
        else
        {
            npcDialog.text = currentMission.info.acceptAnswer;
            CloseDialogUI();
        }

    }

    private void ReceiveRewardAndCompleteMission()
    {
        currentMission.isComplete = true;

        var coinsRecieved = currentMission.info.coinReward;
        print("You recieved " + coinsRecieved + " gold coins");

        if (currentMission.info.rewardItem1 != "")
        {
            InventorySystem.Instance.InsertIntoInv(currentMission.info.rewardItem1);
        }

        if (currentMission.info.rewardItem2 != "")
        {
            InventorySystem.Instance.InsertIntoInv(currentMission.info.rewardItem2);
        }

        activeMissionIndex++;

        // Start Next Quest 
        if (activeMissionIndex < missions.Count)
        {
            currentMission = missions[activeMissionIndex];
            currentDialog = 0;
            DialogSystem.instance.CloseDialogUI();
            isTalking = false;
        }
        else
        {
            DialogSystem.instance.CloseDialogUI();
            isTalking = false;
            print("No more quests");
        }

    }

    private void DeclinedMission()
    {
        currentMission.declined = true;

        npcDialog.text = currentMission.info.declineAnswer;
        CloseDialogUI();
    }

    private void CloseDialogUI()
    {
        option1Text.text = "[Close]";
        option1.onClick.RemoveAllListeners();
        option1.onClick.AddListener(() => {
            DialogSystem.instance.CloseDialogUI();
            isTalking = false;
        });
        option2.gameObject.SetActive(false);
    }

    private void CheckIfDialogIsDone()
    {
        if (currentDialog == currentMission.info.initialDialog.Count - 1) // If its the last dialog 
        {
            npcDialog.text = currentMission.info.initialDialog[currentDialog];

            currentMission.firstConvoComplete = true;

            SetAcceptAndDeclineOptions();
        }
        else  // If there are more dialogs
        {
            npcDialog.text = currentMission.info.initialDialog[currentDialog];

            option1Text.text = "Next";
            option1.onClick.RemoveAllListeners();
            option1.onClick.AddListener(() => {
                currentDialog++;
                CheckIfDialogIsDone();
            });
        }
    }

    //public void StartConversation()
    //{
    //    isTalking = true;

    //    DialogSystem.instance.OpenDialogUI();
    //    DialogSystem.instance.questText.text = "Hello there";
    //    DialogSystem.instance.accept.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Bye";
    //    DialogSystem.instance.accept.onClick.AddListener(() => {
    //        DialogSystem.instance.CloseDialogUI();
    //        isTalking = false;
    //    });


    //}
}
