﻿using Immersive.SuperHero;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Immersive.SuperHero.SuperHeroCreator;

namespace Immersive.SuperHero
{
    public class HorizontalScroll : MonoBehaviour
    {
        int partIndex = 0;
        int spriteIndex = -1;

        List<Transform> parts = new List<Transform>();
        List<SuperHeroParts> superHeroParts;
        SuperHeroParts selectedPart;

        float gapValue;
        float transitionTime;

        public GameObject buttonsParent;
        public SpriteRenderer default_Selected_Sprite;

        Action onScroll;

        private void Awake()
        {
            buttonsParent.SetActive(false);

            transitionTime = 1.0f;
            gapValue = 0.35f;
            partIndex = 0;
        }

        public void SetScroll(List<SuperHeroParts> superHeroParts, Action action)
        {
            onScroll = action;
            buttonsParent.SetActive(true);
            default_Selected_Sprite.gameObject.SetActive(false);

            spriteIndex = 0;
            this.superHeroParts = superHeroParts;

            for (int i = 0; i < 2; i++)
            {
                SpriteRenderer objPart = Utils.GetSpriteRenderer(this.transform);
                objPart.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                objPart.transform.localScale = Vector3.one * 0.8f;
                objPart.sprite = superHeroParts[i].creatorSprite;
                objPart.transform.localPosition = new Vector3(i * gapValue, 0, 0);

                parts.Add(objPart.transform);
            }
        }

        public void SetSelectedSprite(SuperHeroParts selectedSprite)
        {
            selectedPart = selectedSprite;

            if (selectedSprite != null)
            {
                spriteIndex = 0;
                default_Selected_Sprite.sprite = selectedSprite.creatorSprite;
            }
        }

        bool scrolling;

        public void MoveNext()
        {
            if (scrolling)
                return;

            partIndex++;
            spriteIndex++;

            Scroll(-1);
        }

        public void MovePrevious()
        {
            if (scrolling)
                return;

            partIndex--;
            spriteIndex--;

            Scroll(1);
        }

        void Scroll(int direction)
        {
            onScroll();

            scrolling = true;

            if (spriteIndex >= superHeroParts.Count)
                spriteIndex = 0;

            if (spriteIndex < 0)
                spriteIndex = superHeroParts.Count - 1;

            parts[1].GetComponent<SpriteRenderer>().sprite = superHeroParts[spriteIndex].creatorSprite;
            parts[1].localPosition = new Vector3(partIndex * gapValue, 0, 0);

            iTween.MoveBy(this.gameObject, iTween.Hash("x", direction * gapValue, "y", 0, "z", 0, "islocal", false, "time", transitionTime,
                "easetype", iTween.EaseType.easeOutQuad, "oncomplete", (System.Action<object>)(newValue =>
                {
                    Transform temp = parts[0];
                    parts.Remove(temp);
                    parts.Add(temp);

                    scrolling = false;
                })));
        }

        public SuperHeroParts GetSelectedPart()
        {
            return superHeroParts[spriteIndex];
        }
    }
}