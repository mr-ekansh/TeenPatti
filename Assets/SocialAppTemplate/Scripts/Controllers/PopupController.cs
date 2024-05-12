using UnityEngine;
using UnityEngine.UI;
using System;

namespace SocialApp
{
    public class PopupController : MonoBehaviour
    {

        [SerializeField]
        private Text TitleLabel = default;
        [SerializeField]
        private Text MessageLabel = default;
        [SerializeField]
        private GameObject ErrorWindow = default;
        [SerializeField]
        private GameObject ConfirmationWindow = default;
		[SerializeField]
		private GameObject BankWindow = default;
		[SerializeField]
		private GameObject WithdrawWindow = default;
        [SerializeField]
        private Text TitleLabel_1 = default;
        [SerializeField]
        private Text MessageLabel_1 = default;
        private Action Callback;

        [SerializeField]
        private Text DMessageLabel = default;
        [SerializeField]
        private GameObject DErrorWindow = default;

		[SerializeField]
		private Text BankMessageLabel = default;
		[SerializeField]
		private Text WithdrawMessageLabel = default;

		[SerializeField]
		private GameObject WithdrawContinuePopUpWindow = default;
		[SerializeField]
		private Text WithdrawContinueMessageLabel = default;

        public void ShowMessage(PopupMessage _msg)
        {
            TitleLabel.text = _msg.Title;
            MessageLabel.text = _msg.Message;
            TitleLabel_1.text = _msg.Title;
            MessageLabel_1.text = _msg.Message;
            Callback = _msg.Callback;
            if(_msg.Title=="Logout")
            {
                ErrorWindow.SetActive(false);
                ConfirmationWindow.SetActive(true);
                DErrorWindow.SetActive(false);
				BankWindow.SetActive(false);
				WithdrawWindow.SetActive(false);
				WithdrawContinuePopUpWindow.SetActive(false);
            }
            else
            {
                ErrorWindow.SetActive(true);
                ConfirmationWindow.SetActive(false);
                DErrorWindow.SetActive(false);
				BankWindow.SetActive(false);
				WithdrawWindow.SetActive(false);
				WithdrawContinuePopUpWindow.SetActive(false);
            }
        }
        public void ShowMessage(PopupMessage _msg,int _new)
        {
            ErrorWindow.SetActive(false);
            ConfirmationWindow.SetActive(false);
            DErrorWindow.SetActive(true);
			BankWindow.SetActive(false);
			WithdrawWindow.SetActive(false);
			WithdrawContinuePopUpWindow.SetActive(false);
            DMessageLabel.text = _msg.Message;
        }
		public void ShowBankMessage(PopupMessage _msg,int _new)//0 for  Withdraw Now! //1 for Ok
		{
			ErrorWindow.SetActive(false);
			ConfirmationWindow.SetActive(false);
			DErrorWindow.SetActive(false);
			BankWindow.SetActive(false);
			WithdrawWindow.SetActive(false);
			WithdrawContinuePopUpWindow.SetActive(false);
			Callback = _msg.Callback;
			if(!string.IsNullOrEmpty(_msg.Message))
			{
				if(_new==0)
				{
					BankWindow.SetActive(true);
					BankMessageLabel.text =_msg.Message;
				}
				else
				{
					DErrorWindow.SetActive(true);
					DMessageLabel.text =_msg.Message;
				}
			}
		}
		public void ShowWithdrawMessage(PopupMessage _msg,int _new)//0 for  Continue Playing! //1 for Ok
		{
			ErrorWindow.SetActive(false);
			ConfirmationWindow.SetActive(false);
			DErrorWindow.SetActive(false);
			BankWindow.SetActive(false);
			WithdrawWindow.SetActive(false);
			WithdrawContinuePopUpWindow.SetActive(false);
			Callback = _msg.Callback;
			if(!string.IsNullOrEmpty(_msg.Message))
			{
				if(_new==0)
				{
					WithdrawWindow.SetActive(true);
					WithdrawMessageLabel.text =_msg.Message;
				}
				else if(_new==1)
				{
					DErrorWindow.SetActive(true);
					DMessageLabel.text =_msg.Message;
				}
				else if(_new ==2)
				{
					WithdrawContinuePopUpWindow.SetActive(true);
					WithdrawContinueMessageLabel.text =_msg.Message;
				}
			}
		}
        public void ShowPurchase(PopupMessage _msg)
        {
            //TitleLabel5.text = _msg.Title;
            //Purchase.SetActive(true);
        }
        public void ShowError(PopupMessage _msg)
        {
            //TitleLabel5.text = _msg.Title;
            //Purchase.SetActive(true);
        }
        public void CloseWindow()
        {
            if (Callback != null)
            {
                Callback.Invoke();
            }
            AppManager.VIEW_CONTROLLER.HidePopupMessage();
        }
		public void WithdrawCancelWindow()
		{
			AppManager.VIEW_CONTROLLER.HidePopupMessage();
		}
		public void WithdrawContinueWindow()
		{
			if (Callback != null)
			{
				Callback.Invoke();
			}
			AppManager.VIEW_CONTROLLER.HidePopupMessage();
		}
        public void YesWindow()
        {
            if (TitleLabel.text == "Logout")
            {
                //Debug.Log("Log me Out");

                AppManager.USER_SETTINGS.Logout();
            }
        }
    }

    public class PopupMessage
    {
        public string Title;
        public string Message;
        public Action Callback = null;
    }
}
