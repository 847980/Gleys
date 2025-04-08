//using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AimToMite
{

    public class BaseNextPrev : MonoBehaviour
    {
        //[ReadOnly]
        public int curIndex, maxIndex;
        public UnityEvent<int> actionAfterChangeIndex;

        //[Button]
        public void SetMaxVal(int maxIndex)
        {
            this.maxIndex = maxIndex;
        }

        //[Button]
        public void OnChangeIndexRandom()
        {
            curIndex = Random.Range(0, maxIndex);
            actionAfterChangeIndex?.Invoke(curIndex);
        }

        //[Button]
        public void OnChangeIndex(int index)
        {
            curIndex = Random.Range(0, index);
            actionAfterChangeIndex?.Invoke(curIndex);
        }

        //[Button]
        public void OnChangeIndex(bool next)
        {
            if (next)
            {
                if (curIndex >= maxIndex) curIndex = 0;
                else curIndex++;
            }
            else
            {
                if (curIndex <= 0) curIndex = maxIndex;
                else curIndex--;
            }

            actionAfterChangeIndex?.Invoke(curIndex);
        }

    }
}
