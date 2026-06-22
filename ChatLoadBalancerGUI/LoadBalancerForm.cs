using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatLoadBalancerGUI
{
    public partial class LoadBalancerForm : Form
    {
        private readonly List<ChatServerNode> _chatServers = new List<ChatServerNode>
        {
            new ChatServerNode("127.0.0.1", 8888),
            new ChatServerNode("127.0.0.1", 8889)
        };

        private int _nextServerIndex = 0;
        private readonly object _roundRobinLock = new object();

        private TcpListener? _listener;
        private CancellationTokenSource? _cts;
        private int _routedClientCount = 0;

        public LoadBalancerForm()
        {
            InitializeComponent();
            UpdateServerStatusLabels(false, false);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_listener != null)
            {
                AppendLog("[INFO] Load Balancer đang chạy rồi.");
                return;
            }

            if (!int.TryParse(txtLbPort.Text.Trim(), out int lbPort))
            {
                MessageBox.Show("Port Load Balancer không hợp lệ!");
                return;
            }

            try
            {
                _cts = new CancellationTokenSource();
                _listener = new TcpListener(IPAddress.Any, lbPort);
                _listener.Start();

                lblLbStatus.Text = "Running";
                lblLbStatus.ForeColor = Color.Green;

                btnStart.Enabled = false;
                btnStop.Enabled = true;

                AppendLog($"[LOAD BALANCER] Đang chạy tại port {lbPort}.");
                AppendLog("[LOAD BALANCER] Danh sách server:");
                foreach (var server in _chatServers)
                {
                    AppendLog($"- {server.Ip}:{server.Port}");
                }

                _ = MonitorServersAsync(_cts.Token);
                _ = AcceptClientsLoopAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể start Load Balancer: " + ex.Message);
                _listener = null;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _cts?.Cancel();
                _listener?.Stop();
            }
            catch
            {
                // Ignore stop errors
            }

            _listener = null;
            _cts = null;

            lblLbStatus.Text = "Stopped";
            lblLbStatus.ForeColor = Color.Red;

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            UpdateServerStatusLabels(false, false);
            AppendLog("[LOAD BALANCER] Đã dừng.");
        }

        private async Task AcceptClientsLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _listener != null)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = HandleClientRoutingAsync(client);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    AppendLog("[LỖI ACCEPT] " + ex.Message);
                }
            }
        }

        private async Task HandleClientRoutingAsync(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
            {
                try
                {
                    ChatServerNode? assignedServer = await GetAvailableServerAsync();

                    if (assignedServer == null)
                    {
                        await writer.WriteLineAsync("ERROR|No available chat server");
                        AppendLog("[LỖI] Không có Chat Server nào đang hoạt động.");
                        return;
                    }

                    string redirectMsg = $"REDIRECT|{assignedServer.Ip}|{assignedServer.Port}";
                    await writer.WriteLineAsync(redirectMsg);

                    _routedClientCount++;
                    UpdateRouteCount();

                    AppendLog($"[ĐIỀU PHỐI] Client mới -> {assignedServer.Ip}:{assignedServer.Port}");
                }
                catch (Exception ex)
                {
                    AppendLog("[LỖI ROUTING] " + ex.Message);
                }
            }
        }

        private async Task<ChatServerNode?> GetAvailableServerAsync()
        {
            int totalServers = _chatServers.Count;

            for (int i = 0; i < totalServers; i++)
            {
                ChatServerNode candidate;

                lock (_roundRobinLock)
                {
                    candidate = _chatServers[_nextServerIndex];
                    _nextServerIndex = (_nextServerIndex + 1) % totalServers;
                }

                bool isAlive = await IsServerAliveAsync(candidate.Ip, candidate.Port, 800);

                if (isAlive)
                {
                    return candidate;
                }

                AppendLog($"[HEALTH CHECK] Server {candidate.Ip}:{candidate.Port} không phản hồi, bỏ qua.");
            }

            return null;
        }

        private async Task MonitorServersAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool server8888Alive = await IsServerAliveAsync("127.0.0.1", 8888, 500);
                bool server8889Alive = await IsServerAliveAsync("127.0.0.1", 8889, 500);

                UpdateServerStatusLabels(server8888Alive, server8889Alive);

                await Task.Delay(1500, token).ContinueWith(_ => { });
            }
        }

        private async Task<bool> IsServerAliveAsync(string ip, int port, int timeoutMs)
        {
            try
            {
                using (TcpClient testClient = new TcpClient())
                {
                    Task connectTask = testClient.ConnectAsync(ip, port);
                    Task timeoutTask = Task.Delay(timeoutMs);

                    Task completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        return false;
                    }

                    await connectTask;
                    return testClient.Connected;
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateServerStatusLabels(bool server8888Alive, bool server8889Alive)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateServerStatusLabels(server8888Alive, server8889Alive)));
                return;
            }

            lblServer8888.Text = server8888Alive ? "Server 8888: Alive" : "Server 8888: Down";
            lblServer8888.ForeColor = server8888Alive ? Color.Green : Color.Red;

            lblServer8889.Text = server8889Alive ? "Server 8889: Alive" : "Server 8889: Down";
            lblServer8889.ForeColor = server8889Alive ? Color.Green : Color.Red;
        }

        private void UpdateRouteCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateRouteCount));
                return;
            }

            lblRouteCount.Text = $"Client đã route: {_routedClientCount}";
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

        private void LoadBalancerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnStop_Click(sender, e);
        }
    }

    public class ChatServerNode
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public ChatServerNode(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
    }
}