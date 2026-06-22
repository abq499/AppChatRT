using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServerGUI
{
    public partial class ChatServerForm : Form
    {
        private Process? _serverProcess;
        private int _connectedCount = 0;

        public ChatServerForm()
        {
            InitializeComponent();
            UpdateStatus(false);
        }

        private bool ShouldHideNoisyLog(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return true;

            if (line.Contains("[LỖI Unknown]"))
                return true;

            if (line.Contains("Unable to read data from the transport connection"))
                return true;

            if (line.Contains("An established connection was aborted"))
                return true;

            if (line.Contains("An existing connection was forcibly closed"))
                return true;

            if (line.Contains("warning CS"))
                return true;

            if (line.Contains("ChatServer.csproj"))
                return true;

            return false;
        }
        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                AppendLog("[INFO] Server đang chạy rồi.");
                return;
            }

            if (!int.TryParse(txtServerPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port server không hợp lệ!");
                return;
            }

            string chatServerProjectPath = FindChatServerProjectPath();

            if (string.IsNullOrEmpty(chatServerProjectPath) || !File.Exists(chatServerProjectPath))
            {
                MessageBox.Show(
                    "Không tìm thấy ChatServer.csproj.\n" +
                    "Hãy kiểm tra project ChatServer nằm cùng solution với ChatServerGUI.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            try
            {
                _connectedCount = 0;
                UpdateClientCount();

                var psi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"run --no-build --project \"{chatServerProjectPath}\"",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                _serverProcess = new Process();
                _serverProcess.StartInfo = psi;
                _serverProcess.EnableRaisingEvents = true;

                _serverProcess.OutputDataReceived += ServerProcess_OutputDataReceived;
                _serverProcess.ErrorDataReceived += ServerProcess_ErrorDataReceived;
                _serverProcess.Exited += ServerProcess_Exited;

                _serverProcess.Start();
                _serverProcess.BeginOutputReadLine();
                _serverProcess.BeginErrorReadLine();

                await Task.Delay(500);

                await _serverProcess.StandardInput.WriteLineAsync(port.ToString());
                await _serverProcess.StandardInput.FlushAsync();

                lblCurrentPort.Text = $"Port: {port}";
                UpdateStatus(true);

                AppendLog($"[GUI] Đã start ChatServer với port {port}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể start ChatServer: " + ex.Message);
                UpdateStatus(false);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServerProcess();
        }

        private void StopServerProcess()
        {
            try
            {
                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    _serverProcess.Kill(true);
                    _serverProcess.WaitForExit(1500);
                }
            }
            catch (Exception ex)
            {
                AppendLog("[GUI STOP ERROR] " + ex.Message);
            }
            finally
            {
                _serverProcess = null;
                UpdateStatus(false);
                AppendLog("[GUI] Server đã dừng.");
            }
        }

        private void ServerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
                return;

            if (ShouldHideNoisyLog(e.Data))
                return;

            AppendLog(e.Data);
            ParseServerLog(e.Data);
        }

        private void ServerProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
                return;

            if (ShouldHideNoisyLog(e.Data))
                return;

            AppendLog("[ERROR] " + e.Data);
        }

        private void ServerProcess_Exited(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ServerProcess_Exited(sender, e)));
                return;
            }

            UpdateStatus(false);
            AppendLog("[GUI] ChatServer process đã thoát.");
        }

        private void ParseServerLog(string line)
        {
            if (line.Contains(" connected on server "))
            {
                _connectedCount++;
                UpdateClientCount();

                string username = line.Split(" connected on server ")[0].Trim();
                AddOnlineUser(username);
            }
            else if (line.Contains(" disconnected from server "))
            {
                if (_connectedCount > 0)
                    _connectedCount--;

                UpdateClientCount();

                string username = line.Split(" disconnected from server ")[0].Trim();
                RemoveOnlineUser(username);
            }
        }

        private void AddOnlineUser(string username)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddOnlineUser(username)));
                return;
            }

            if (string.IsNullOrWhiteSpace(username) || username == "Unknown")
                return;

            if (!lbLocalUsers.Items.Contains(username))
                lbLocalUsers.Items.Add(username);
        }

        private void RemoveOnlineUser(string username)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => RemoveOnlineUser(username)));
                return;
            }

            if (lbLocalUsers.Items.Contains(username))
                lbLocalUsers.Items.Remove(username);
        }

        private void UpdateClientCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateClientCount));
                return;
            }

            lblClientCount.Text = $"Local clients: {_connectedCount}";
        }

        private void UpdateStatus(bool running)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(running)));
                return;
            }

            lblServerStatus.Text = running ? "Running" : "Stopped";
            lblServerStatus.ForeColor = running ? Color.Green : Color.Red;

            btnStart.Enabled = !running;
            btnStop.Enabled = running;
            txtServerPort.Enabled = !running;
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendLog(message)));
                return;
            }

            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
            rtbLog.AppendText(line + Environment.NewLine);
            rtbLog.ScrollToCaret();
        }

        private string FindChatServerProjectPath()
        {
            string baseDir = AppContext.BaseDirectory;

            string[] candidates =
            {
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\ChatServer\ChatServer.csproj")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\..\ChatServer\ChatServer.csproj"))
            };

            foreach (string path in candidates)
            {
                if (File.Exists(path))
                    return path;
            }

            return "";
        }

        private void ChatServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServerProcess();
        }
    }
}