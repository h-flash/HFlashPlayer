using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Windows.Forms;

namespace HFlashPlayer
{
	public partial class HFlashPlayer : Form
	{
		static string Scheme { get; set; } = "hflash";

		public string RegisteredScheme { get; set; }
		public string RegisteredApplication { get; set; }
		public string RegisterApplication { get { return Application.ExecutablePath + " %1"; } }

		public HFlashPlayer()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

			var args = Environment.GetCommandLineArgs();
			var cmd = args.Length > 1 ? args[1] : null;

			if (String.IsNullOrWhiteSpace(cmd))
			{
				CheckRegistry();

				if (String.IsNullOrEmpty(RegisteredScheme))
				{
					AdminRun("--register");
					Application.Exit();
				}
				else if (RegisterApplication != RegisteredApplication)
				{
					AdminRun("--register");
					Application.Exit();
				}

				this.lblInfo.Text = $"Scheme {RegisteredScheme} registered to {RegisteredApplication}";
			}
			else
			{

				if (cmd.Length > Scheme.Length + 3 && cmd.Substring(0, Scheme.Length + 3) == Scheme + "://")
				{
					cmd = cmd.Substring(Scheme.Length + 3);
				}
				if (cmd.Substring(cmd.Length - 1) == "/") cmd = cmd.Substring(0, cmd.Length - 1);


				if (cmd == "--register")
				{
					Register();
					Application.Exit();
				}
				else
				{
					this.lblInfo.Text = $"cmd:"+cmd;
					var url = Base64Decode(cmd);
					this.lblInfo.Text = $"url:"+url;
					Open(url);
					Application.Exit();
				}

			}




			//Application.Exit();
		}


		private void CheckRegistry()
		{
			var key = Registry.ClassesRoot.OpenSubKey(Scheme, false);
			if (key != null)
			{
				RegisteredScheme = Scheme;
			}

			var key2 = Registry.ClassesRoot.OpenSubKey(Scheme + "\\shell\\open\\command\\", false);
			if (key2 != null)
			{
				RegisteredApplication = (string)key2.GetValue("");
			}
		}

		private void Register()
		{
			var key = Registry.ClassesRoot.OpenSubKey(Scheme, true);
			if (key == null)
			{
				key = Registry.ClassesRoot.CreateSubKey(Scheme);
				key.SetValue("URL Protocol", "");
				key.SetValue("", $"URL:{Scheme} Protocol");

				var iconkey = key.CreateSubKey("DefaultIcon");
				iconkey.SetValue("", Application.ExecutablePath + ",1");

				var key2 = key.CreateSubKey("shell");
				var key3 = key2.CreateSubKey("open");
				var key4 = key3.CreateSubKey("command");
				key4.SetValue("", RegisterApplication);
			}
			else
			{
				var appkey = key.OpenSubKey("shell\\open\\command\\", true);
				if (appkey != null)
				{
					appkey.SetValue("", RegisterApplication);
				}
			}
			MessageBox.Show("Scheme " + Scheme + " Registered to " + Application.ExecutablePath);
		}

		private void AdminRun(string cmd)
		{
			ProcessStartInfo proc = new ProcessStartInfo();
			proc.UseShellExecute = true;
			proc.WorkingDirectory = Environment.CurrentDirectory;
			proc.FileName = Application.ExecutablePath;
			proc.Arguments = cmd;
			proc.Verb = "runas";
			try
			{
				Process.Start(proc);
			}
			catch
			{
				return;
			}
		}

		private string Base64Decode(string s)
		{
			var data = Convert.FromBase64String(s);
			return System.Text.Encoding.UTF8.GetString(data);
		}

		private void Open(string url)
		{
			var p = Process.Start(System.IO.Path.Combine(Application.StartupPath, "flashplayer.exe"), url);
			p.WaitForInputIdle();
		}

	}
}
