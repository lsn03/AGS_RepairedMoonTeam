using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Vector2 jumpPadForce;
    //если 0 ( по умолчанию), то горизонтальная скорость персонажа заменяется скоростью джам пада, если 1, то берется скорость персонажа, если 2 то берется наибольшее значение
    //вариант 0 полезен когда мы хотим что бы игрок летел по заданной траэктории, вариант 1 - для вертикальных джам падов, когда мы не хотим что-бы скорость терялась, 2 - для остального
    [Range(0, 2), SerializeField] int isPreservePlayerSpeed;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 _finalForce = jumpPadForce;
            float _playerVelocityX = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            if (isPreservePlayerSpeed == 1)
            {
                _finalForce.x = _playerVelocityX;
            }

            if (isPreservePlayerSpeed == 2)
                if (_finalForce.x > 0)
                    _finalForce.x = Mathf.Max(_finalForce.x, _playerVelocityX);
                else if (_finalForce.x < 0)
                    _finalForce.x = Mathf.Min(_finalForce.x, _playerVelocityX);
                else
                    _finalForce.x = _playerVelocityX;

            collision.gameObject.GetComponent<Rigidbody2D>().velocity = _finalForce;
        }
    }
}
