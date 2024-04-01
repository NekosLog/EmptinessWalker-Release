/* 制作日 2023/11/30～
*　製作者 ニシガキ
*　最終更新日 2023/12/01
*/

using UnityEngine;
 
public class InputEvents:MonoBehaviour
{
    // 入力時間の管理配列　要素数は入力の種類
    private float[] _inputTimer = new float[13];

    // PlayerMoveのインターフェース
    private IFPlayerMove _playerMove = default;

    private void Awake()
    {
        _playerMove = GameObject.FindObjectOfType<PlayerMove>();
    }

    /// <summary>
    /// 入力に応じた押している時の処理を実行するクラス
    /// </summary>
    /// <param name="inputType">入力の種類</param>
    public void Execution(E_InputType inputType)
    {
        switch (inputType)
        {
            case E_InputType.Right:
                _playerMove.PlayerWalking(inputType);

                break;

            case E_InputType.Left:
                _playerMove.PlayerWalking(inputType);

                break;

            case E_InputType.Up:

                break;

            case E_InputType.Down:

                break;

            case E_InputType.Action:

                break;

            case E_InputType.Jump:
                if(_inputTimer[(int)inputType] == 0)
                {
                    _playerMove.PlayerJump();
                }

                break;

            case E_InputType.Dash:
                if (_inputTimer[(int)inputType] == 0)
                {
                    _playerMove.PlayerDash();
                }
                break;

            case E_InputType.Attack:
                if (_inputTimer[(int)inputType] == 0)
                {
                    _playerMove.PlayerAttack();
                }
                break;

            case E_InputType.Heal:
                if (_inputTimer[(int)inputType] == 0)
                {
                    _playerMove.PlayerHealStart();
                }
                _playerMove.PlayerHeal();
                break;

            case E_InputType.Skill1:
                if (_inputTimer[(int)inputType] == 0)
                {
                    _playerMove.PlayerSkill(inputType);
                }
                break;

            case E_InputType.Skill2:

                break;

            case E_InputType.Menu:
                
                break;

            case E_InputType.Map:

                break;
        }
        _inputTimer[(int)inputType] += Time.deltaTime;
    }

    /// <summary>
    /// 入力のに応じた離した時の処理を実行するクラス
    /// </summary>
    /// <param name="inputType">入力の種類</param>
    public void Exit(E_InputType inputType)
    {
        switch (inputType)
        {
            case E_InputType.Right:
                _playerMove.PlayerStoping();
                break;

            case E_InputType.Left:
                _playerMove.PlayerStoping();
                break;

            case E_InputType.Up:

                break;

            case E_InputType.Down:

                break;

            case E_InputType.Action:

                break;

            case E_InputType.Jump:

                break;

            case E_InputType.Dash:

                break;

            case E_InputType.Attack:

                break;

            case E_InputType.Heal:
                _playerMove.PlayerHealExit();
                break;

            case E_InputType.Skill1:

                break;

            case E_InputType.Skill2:

                break;

            case E_InputType.Menu:

                break;

            case E_InputType.Map:

                break;
        }
        _inputTimer[(int)inputType] = 0f;
    }
}