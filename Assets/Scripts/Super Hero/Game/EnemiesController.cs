﻿using Com.Immersive.Cameras;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Immersive.SuperHero
{
    public class EnemiesController : SuperHeroGame
    {
        [Header("Enemy Objects")]
        public Transform enemyParent;
        public Enemies enemies;
        public Enemy prefabEnemy;
        

        float enemyRange;
        int enemyIndex;
        int totalEnemy;

        new void Start()
        {
            base.Start();

            SetPosition();
        }

        public int GetTotalEnemies()
        {
            return enemies.EnemyList.Count;
        }

        void SetPosition()
        {
            if (wallType == WallType.Left)
            {
                this.transform.localPosition = AbstractImmersiveCamera.CurrentImmersiveCamera.cameras[0].transform.localPosition;
            }
            else if (wallType == WallType.Right)
            {
                this.transform.localPosition = AbstractImmersiveCamera.CurrentImmersiveCamera.cameras[2].transform.localPosition;
            }
        }

        public void SetWall()
        {
            enemyIndex = 0;
            //totalEnemy = enemies.EnemyList.Count;
            enemyRange = AbstractImmersiveCamera.CurrentImmersiveCamera.cameras[0].aspect / 3.0f;

            CreateEnemy();
            Invoke("CreateEnemy", 2.0f);
        }

        void CreateEnemy()
        {
            if (enemyIndex >= enemies.EnemyList.Count)
            {
               return;
            }

            Vector3 startPosition = new Vector3(Random.Range(-enemyRange, enemyRange), 1.0f, 0);
            Vector3 endPosition = new Vector3(Random.Range(-enemyRange, enemyRange), Random.Range(0.2f, 0.4f), 0);

            Enemy objEnemy = Instantiate(prefabEnemy, enemyParent, false);
            objEnemy.transform.localPosition = startPosition;
            objEnemy.transform.localScale = Vector3.one * 0.3f;

            objEnemy.Init(enemies.EnemyList[enemyIndex], endPosition);

            enemyIndex++;        
        }

        public void OnEnemyDestory()
        {
            if (enemyIndex < enemies.EnemyList.Count)
            {
                CreateEnemy();
            }            
        }

        protected override void UpdateScore()
        {
            totalEnemy++;

            textEnemy.text = "Enemy: " + totalEnemy;
            SuperHeroGameManager.Instance.OnAllEnemiesDestroyedOfWall();

            if (totalEnemy <= 0)
            {
                
            }
        }
    }
}