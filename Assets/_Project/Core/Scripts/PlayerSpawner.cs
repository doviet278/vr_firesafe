using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject vrPlayerPrefab;
    public GameObject pcPlayerPrefab;
    public Transform spawnPoint;

    [Header("UI Instruction Panels")]
    public GameObject vrInstructionUI;
    public GameObject pcInstructionUI;

    private void Start()
    {
        if (IsVRHeadsetPresent())
        {
            Debug.Log("Khoi tao VR Rig va UI VR.");
            // Sinh ra nhan vat VR
            Instantiate(vrPlayerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Bat bang huong dan VR, tat bang PC
            if (vrInstructionUI != null) vrInstructionUI.SetActive(true);
            if (pcInstructionUI != null) pcInstructionUI.SetActive(false);
        }
        else
        {
            Debug.Log("Khoi tao PC Rig va UI PC.");
            // Sinh ra nhan vat PC
            Instantiate(pcPlayerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Bat bang huong dan PC, tat bang VR
            if (pcInstructionUI != null) pcInstructionUI.SetActive(true);
            if (vrInstructionUI != null) vrInstructionUI.SetActive(false);
        }
    }

    private bool IsVRHeadsetPresent()
    {
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetSubsystems(displaySubsystems);
        
        foreach (var subsystem in displaySubsystems)
        {
            if (subsystem.running)
            {
                return true;
            }
        }
        return false;
    }
}