using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GroupSorter
{
    public static List<KeyValuePair<T, int>> SortDictionary<T>(Dictionary<T, int> inputData, List<Transform> groupTargets) where T : Component
    {
        if (inputData == null) throw new ArgumentNullException(nameof(inputData));
        if (groupTargets == null) throw new ArgumentNullException(nameof(groupTargets));

        List<KeyValuePair<T, int>> list = new List<KeyValuePair<T, int>>(inputData);

        if (list.Count > 1)
        {
            QuickSort(list, 0, list.Count - 1, groupTargets);
        }

        return list;
    }

    private static void QuickSort<T>(List<KeyValuePair<T, int>> list, int left, int right, List<Transform> groupTargets) where T : Component
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right, groupTargets);
            QuickSort(list, left, pivotIndex - 1, groupTargets);
            QuickSort(list, pivotIndex + 1, right, groupTargets);
        }
    }

    private static int Partition<T>(List<KeyValuePair<T, int>> list, int left, int right, List<Transform> groupTargets) where T : Component
    {
        KeyValuePair<T, int> pivot = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (CompareElements(list[j], pivot, groupTargets) < 0)
            {
                i++;
                Swap(list, i, j);
            }
        }

        Swap(list, i + 1, right);
        return i + 1;
    }

    private static int CompareElements<T>(KeyValuePair<T, int> a, KeyValuePair<T, int> b, List<Transform> groupTargets) where T : Component
    {
        if (a.Value != b.Value)
        {
            return a.Value.CompareTo(b.Value);
        }

        int groupIndex = a.Value;

        if (groupIndex < 0 || groupIndex >= groupTargets.Count)
        {
            return 0;
        }

        Transform target = groupTargets[groupIndex];
        if (target == null || a.Key == null || b.Key == null)
        {
            return 0;
        }

        Vector3 posA = a.Key.transform.position;
        Vector3 posB = b.Key.transform.position;
        Vector3 targetPos = target.position;

        float distA = (posA - targetPos).sqrMagnitude;
        float distB = (posB - targetPos).sqrMagnitude;

        return distA.CompareTo(distB);
    }

    private static void Swap<T>(List<KeyValuePair<T, int>> list, int indexA, int indexB)
    {
        KeyValuePair<T, int> temp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = temp;
    }
}

