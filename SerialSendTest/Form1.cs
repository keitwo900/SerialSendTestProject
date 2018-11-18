using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialSendTest
{
	public partial class Form1 : Form
	{

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Form1()
		{
			InitializeComponent();
			serialPort1.Encoding = Encoding.UTF8; //エンコーディング指定
		}

		/// <summary>
		/// シリアルポートを取得して、ComboBoxのリストに追加します。
		/// </summary>
		void GetComName()
		{
			string[] ports = SerialPort.GetPortNames(); //ポート取得

			//ComboBoxに追加
			foreach (string port in ports)
			{
				comboBox_ComName.Items.Add(port);
			}
		}

		//-------------------------------------------------------------------
		//-------------------------------------------------------------------
		// 以下、フォームのイベント関連
		//-------------------------------------------------------------------
		//-------------------------------------------------------------------

		/// <summary>
		/// ポート名更新ボタンが押されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button_PortUpdate_Click(object sender, EventArgs e)
		{
			GetComName();
		}

		/// <summary>
		/// 接続ボタンが押されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button_Connect_Click(object sender, EventArgs e)
		{
			if (button_Connect.Text == "接続") //ポートオープン
			{
				if (comboBox_ComName.Text == "")
				{
					MessageBox.Show("ポートを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				serialPort1.PortName = comboBox_ComName.Text;

				try
				{
					serialPort1.Open();
					button_Connect.Text = "切断";
				}
				catch (System.UnauthorizedAccessException ex) //別プロセスがすでに指定したCOMを開いている場合など
				{
					MessageBox.Show(ex.ToString());
				}
			}
			else //ポートクローズ
			{
				serialPort1.Close();
				button_Connect.Text = "接続";
			}
		}

		/// <summary>
		/// 送信ボタンが押されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button_Tx_Click(object sender, EventArgs e)
		{
			if (serialPort1.IsOpen)
			{
				serialPort1.Write(textBox_Tx.Text + "\r\n");
			}
			else
			{
				MessageBox.Show("シリアルポートは閉じられています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// フォームロード
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			GetComName();

			//COMポートが1つでも存在した場合は、最初に検出したCOMポートをデフォルトでセットする
			if(comboBox_ComName.Items.Count != 0)
			{
				comboBox_ComName.Text = comboBox_ComName.Items[0].ToString();
			}
		}
	}
}
