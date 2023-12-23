using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Objects.Enemy
{

    public class Enemy : MonoBehaviour
    {
        protected virtual void NormalState() { }
        protected virtual void PlayerSpotted(Vector2 playerPosition, Vector2 direction) { }
    }
}
