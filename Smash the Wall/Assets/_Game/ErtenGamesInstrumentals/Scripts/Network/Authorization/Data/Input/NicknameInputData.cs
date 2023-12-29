using Helpers;
using System;
using TMPro;
using UI;
using UnityEngine;

namespace Authorization.Data.Input
{
    [Serializable]
    internal class NicknameInputData : InputDataBase
    {
        [field: SerializeField] public TMP_InputField nickNameField { get; protected set; }

        public void ValidateNickName(string nickName = null)
        {
            if (string.IsNullOrEmpty(nickName)) nickName = nickNameField.text;

            if (StringHelper.IsNickName(nickName) == false)
            {
                isCorrect = false;
                MessageToUser.instance.Log("Invalid nickname");
            }
            else
            {
                isCorrect = true;
            }

            IndicateCorrectOrNot();
        }

        public override void Clear()
        {
            nickNameField.text = string.Empty;

            base.Clear();
        }
    }
}