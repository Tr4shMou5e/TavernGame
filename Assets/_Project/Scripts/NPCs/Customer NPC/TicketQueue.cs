using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;
using Cysharp.Text;
using TMPro;
using UnityEngine.UI;

public class TicketQueue : SerializedMonoBehaviour
{
    [SerializeField] private List<GameObject> ticketOrders;
    [SerializeField] private GameObject CustomerOrderPanel;
    [SerializeField] private GameObject currentOrderGameObject;
    private Dictionary<Type, RequiredProcess> processTranslation;
    private FoodItemInfoManager orders;
    
    void Awake()
    {
        orders = FoodItemInfoManager.Instance;
        SetupDictionary();
    }

    private void SetupDictionary()
    {
        processTranslation = new Dictionary<Type, RequiredProcess>();
        
        processTranslation.Add(typeof(CuttingBoardInteractableMiniGame), RequiredProcess.CuttingBoard);
        processTranslation.Add(typeof(OvenInteractableMiniGame), RequiredProcess.Oven);
        
    }

    void Update()
    {
        ToggleCustomerOrderCanvas();
    }

    private void ToggleCustomerOrderCanvas()
    {
        if(Keyboard.current.tabKey.wasPressedThisFrame)
            CustomerOrderPanel.SetActive(!CustomerOrderPanel.activeSelf);
    }

    private void RefreshQueue()
    {
        UpdateCurrentOrderDisplay();
        UpdateAllCustomerOrderDisplay();
        UpdateCheckMark();
    }

    private void UpdateAllCustomerOrderDisplay()
    {
        Debug.Log("Updating all customer orders");
        var orderList = orders.GetCustomerOrderKeys();
        Debug.Log(orderList.Count);
        for (int i = 0; i < ticketOrders.Count; i++)
        {
            int orderIndex = i + 1;
            bool hasQueuedOrder = orderIndex < orderList.Count;

            ticketOrders[i].SetActive(hasQueuedOrder);

            if (!hasQueuedOrder)
                continue;
            Debug.Log("Populating ticket " + i);
            Debug.Log(ticketOrders.Count);
            PopulateTicket(ticketOrders[i], orderList[orderIndex]);
        }
    }

    private void PopulateTicket(GameObject ticketObject, CustomerOrderKey orderKey)
    {
        var textFields = ticketObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        var dishImage = ticketObject.GetComponentInChildren<Image>(true);
        
        Debug.Log("textFields.Length: " + textFields.Length);
        foreach (var textField in textFields)
        {
            switch (textField.gameObject.name)
            {
                case "Customer Name":
                    textField.SetTextFormat("{0}'s Order", orderKey.Customer.name);
                    break;
                case "Dish Name":
                    textField.SetText(orderKey.FoodItem.dishName);
                    break;
                case "Price":
                    textField.SetTextFormat("${0} Tokens", orderKey.FoodItem.price.ToString());
                    break;
            }
        }

        if (dishImage != null)
        {
            dishImage.sprite = orderKey.FoodItem.dishImage;
        }
    }

    private void UpdateCurrentOrderDisplay()
    {
        var orderList = orders.GetCustomerOrderKeys();

        if (orderList.Count == 0)
        {
            currentOrderGameObject.SetActive(false);
            return;
        }

        currentOrderGameObject.SetActive(true);

        var processes = currentOrderGameObject.GetComponentsInChildren<Toggle>(true);
        var orderImage = currentOrderGameObject.GetComponentInChildren<Image>(true);
        var customerName = currentOrderGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
        var firstOrder = orderList[0].FoodItem.processes;

        for (int i = 0; i < processes.Length; i++)
        {
            processes[i].gameObject.SetActive(true);
            processes[i].isOn = false;
        }

        int count = Mathf.Min(firstOrder.Count, processes.Length);

        for (int i = 0; i < count; i++)
        {
            var interactable = firstOrder[i].GetComponent<InteractableObject>();
            if (interactable == null) continue;

            if (processTranslation.TryGetValue(interactable.GetType(), out var requiredProcess))
            {
                var label = processes[i].GetComponentInChildren<Text>(true);
                if (label != null)
                    label.text = requiredProcess.ToString();
            }
        }

        for (int i = count; i < processes.Length; i++)
        {
            processes[i].gameObject.SetActive(false);
        }

        if (orderImage != null)
            orderImage.sprite = orderList[0].FoodItem.dishImage;

        if (customerName != null)
            customerName.SetTextFormat("{0}'s Order", orderList[0].Customer.name);
    }

    private void UpdateCheckMark()
    {
        var orderList = orders.GetCustomerOrderKeys();
        if (orderList == null || orderList.Count == 0)
            return;

        var currentOrder = orderList[0];

        if (!orders.customersOrderDictionary.TryGetValue(currentOrder, out var processComplete))
            return;

        var processes = currentOrderGameObject.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < processes.Length; i++)
            processes[i].isOn = false;

        int count = Mathf.Min(processComplete.Count, processes.Length);

        for (int i = 0; i < count; i++)
            processes[i].isOn = processComplete[i].Item2;
    }

    private enum RequiredProcess
    {
        CuttingBoard,
        Oven
    }

    void OnEnable()
    {
        NpcOrderState.OnOrderTaken += RefreshQueue;
        InteractableObject.OnMiniGameEnd += RefreshQueue;
    }
    void OnDisable()
    {
        NpcOrderState.OnOrderTaken -= RefreshQueue;
        InteractableObject.OnMiniGameEnd -= RefreshQueue;
    }
}