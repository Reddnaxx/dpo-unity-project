using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00_Scripts.Input
{
    public class EnemyFollow : MonoBehaviour
    {
        [SerializeField] public Transform player; 
        [SerializeField] public float speed = 2f;

        private void Update()
        {
            if (player == null) return;

            Vector3 direction = (player.position - transform.position).normalized;

            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
