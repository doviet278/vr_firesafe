using UnityEngine;

public class SceneFinalManager : MonoBehaviour
{
	public static SceneFinalManager Instance { get; private set; }

	[SerializeField] private GameObject areaSound;
	private int totalFireItems = 7;
	private int extinguishedCount;
	public bool isCompleted;

	private void Awake()
	{
		Instance = this;
	}

	public void ReportFireExtinguished()
	{
		if (isCompleted) return;

		extinguishedCount++;

		if (extinguishedCount >= totalFireItems)
		{
			GameTaskManager.Instance?.MarkTaskCompleted("4");
			areaSound.SetActive(false);
			isCompleted = true;
		}
	}

}
