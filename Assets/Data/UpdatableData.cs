using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableData : ScriptableObject
{
    public event System.Action OnValuesUpdated;
    public bool autoUpdate;

    #if UNITY_EDITOR
    protected virtual void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }

    protected virtual void _OnValidate(){
        if(autoUpdate){
            //NotifyOfUpdatedValues();
            UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
        }
    }

    public void NotifyOfUpdatedValues(){
        UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
        if(OnValuesUpdated != null) OnValuesUpdated();
    }

    #endif
}
