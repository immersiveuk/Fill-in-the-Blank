﻿using Com.Immersive.Cameras;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Immersive.FillInTgeBlank
{
    public class FillInTheBlanksManager : MonoBehaviour
    {
        public delegate void SpellingSelected(FillInTheBlanksSpelling fillInTheBlanksData);
        public static event SpellingSelected OnSpellingSelected;

        public SpellingPanel spellingPanel;
        public MissingLettersPanel missingLettersPanel;

        public List<FillInTheBlanksData> fillInTheBlanksData;

        [Header("Sounds")]
        public AudioClip positiveClip;
        public AudioClip negativeClip;

        List<FillInTheBlanksSpelling> spellings;

        private int questionNo = 0;

        void Start()
        {
            spellings = new List<FillInTheBlanksSpelling>();
            SetLayout();
        }

        /// <summary>
        /// Set Layout of Spelling and Missing Layout based on <FillInTheBlanksData>
        /// </summary>
        void SetLayout()
        {
            missingLettersPanel.SetPanel(fillInTheBlanksData, OnResultAction);
            spellings = spellingPanel.SetPanel(fillInTheBlanksData).ToList();
            

            SelectNextSpelling();
        }

        /// <summary>
        /// Callback after click on Missing Letter with result
        /// </summary>
        /// <param name="result"></param>
        void OnResultAction(bool result)
        {
            if (result)
            {
                SelectNextSpelling();
                AbstractImmersiveCamera.PlayAudio(positiveClip);
            }
            else
                AbstractImmersiveCamera.PlayAudio(negativeClip);
        }

        /// <summary>
        /// Select next Spelling after correct answer
        /// </summary>
        public void SelectNextSpelling()
        {
            foreach (var obj in spellings)
            {
                obj.OnDeselect();
            }

            if (questionNo >= spellings.Count)
                return;

            spellings[questionNo].OnSelect();
            OnSpellingSelected(spellings[questionNo]);

            questionNo++;
        }
    }

    [System.Serializable]
    public class FillInTheBlanksData
    {
        public string spelling;
        public int startIndex;
        public int endIndex;

        [NonSerialized]
        public string missingLetters;
    }

    public static class IListExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}