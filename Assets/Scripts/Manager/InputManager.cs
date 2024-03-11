/* 制作日 2023/11/30～
*　製作者 ニシガキ
*　最終更新日 2023/11/30
*/

using UnityEngine;
using System.Collections;
 
public class InputManager : MonoBehaviour 
{
	// InputEvent
	private InputEvents _inputEvents = default;

	// 入力判定の最低値
	private const float INPUT_MINIMUN_VALUE = 0.71f;
	// 入力判定の最大値
	private const float INPUT_MAXIMAM_VALUE = 1f;

	// 入力中かどうか　要素数は入力の種類
	private bool[] _inputStates = new bool[13];
 
	private void Awake()
	{
        _inputEvents = this.gameObject.GetComponent<InputEvents>();
    }
	
	private void Start()
	{
	}

	private void Update()
	{
		InputSystem(E_InputType.Right, Input.GetAxisRaw("Horizontal"));
		InputSystem(E_InputType.Left , -Input.GetAxisRaw("Horizontal"));
		InputSystem(E_InputType.Up, Input.GetAxisRaw("Vertical"));
		InputSystem(E_InputType.Down , -Input.GetAxisRaw("Vertical"));
		InputSystem(E_InputType.Action , Input.GetAxisRaw("Action"));
		InputSystem(E_InputType.Jump , Input.GetAxisRaw("Jump"));
		InputSystem(E_InputType.Dash , Input.GetAxisRaw("Dash"));
		InputSystem(E_InputType.Attack , Input.GetAxisRaw("Attack"));
		InputSystem(E_InputType.Heal , Input.GetAxisRaw("Heal"));
		InputSystem(E_InputType.Skill1 , Input.GetAxisRaw("Skill1"));
		InputSystem(E_InputType.Skill2 , Input.GetAxisRaw("Skill2"));
		InputSystem(E_InputType.Menu , Input.GetAxisRaw("Menu"));
		InputSystem(E_InputType.Map , Input.GetAxisRaw("Map"));
	}

	private void OnDestroy()
	{
	}

	private void InputSystem(E_InputType inputType ,float inputValue)
    {
		int inputNumber = (int)inputType;
		if (inputValue > INPUT_MINIMUN_VALUE)
		{
			_inputEvents.Execution(inputType);
            if (!_inputStates[inputNumber])
            {
				_inputStates[inputNumber] = true;
            }
		}
		else if (_inputStates[inputNumber])
		{
			_inputEvents.Exit(inputType);
			_inputStates[inputNumber] = false;
		}
    }
}