using MykroFramework.Runtime.Objects.Player2D;
using System.Collections;
using UnityEngine;

namespace Assets.Objects.Enemy
{
    public class Shroom : Enemy
    {
        [SerializeField] private float _desiredLengthTillPlayer;
        [SerializeField] private Vector3 _groundCheckOffset;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _wallCheckLength;
        [SerializeField] private float _groundCheckLength;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight;
        private PlayerCharacter2D _character2D;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.right * _wallCheckLength);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position + _groundCheckOffset, Vector3.down * _groundCheckLength);
        }

        private void Awake()
        {
            _character2D = GetComponent<PlayerCharacter2D>();
        }

        private void Update()
        {
            _character2D.Move(_moveSpeed, 0, transform.right);
            var down = Physics2D.Raycast(transform.position + _groundCheckOffset, Vector2.down, _groundCheckLength, _whatIsGround);
            var right = Physics2D.Raycast(transform.position, transform.right, _wallCheckLength, _whatIsGround);
            if (!down || right)
            {
                if(_character2D.Grounded)
                    _character2D.Jump(new Vector2(transform.right.x, _jumpHeight), true);
            }
        }
    }
}
