using UnityEngine;
using System.Collections;
using UnityEditor;

public class SearchByTag : MonoBehaviour
{

    private static string SelectedTag = "Player";

    [MenuItem("Helpers/Select By Tag")]
    public static void SelectObjectsWithTag()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
        Selection.objects = objects;
    }
}