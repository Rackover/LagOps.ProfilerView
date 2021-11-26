using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LouveSystems.LagOps.ProfilerView
{
    public partial class MainWindow : Form
    {

        Dictionary<TreeNode, ActionReport> reportsPerNodes = new Dictionary<TreeNode, ActionReport>();

        public MainWindow()
        {
            InitializeComponent();

            this.DragDrop += MainWindow_DragDrop;
            this.DragEnter += MainWindow_DragEnter;

            TickTreeView.AfterSelect += TickTreeView_AfterSelect;
        }

        private void TickTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateChartForNode(e.Node);
        }

        private void UpdateChartForNode(TreeNode treeNode)
        {
            var serie = TickBreakdownChart.Series[0];
            serie.Points.Clear();
            serie["PieLabelStyle"] = "Disabled";


            foreach (TreeNode child in treeNode.Nodes)
            {
                if (reportsPerNodes.TryGetValue(child, out ActionReport childAction))
                {

                    serie.Points.AddXY(childAction.Action, childAction.TotalMiliseconds);
                }
            }
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            var data = (string[])e.Data.GetData(DataFormats.FileDrop);
            Load(System.IO.File.ReadAllText(data[0]).ToString());
        }

        new public void Load(string yml)
        {
            var result = new YamlDotNet.Serialization.Deserializer().Deserialize<ProfilerReport>(yml);

            ClearTreeView();

            foreach(var tick in result.Reports)
            {
                AddTickToView(tick);
            }
        }

        private void ClearTreeView()
        {
            TickTreeView.Nodes.Clear();
            reportsPerNodes.Clear();
        }

        private void AddTickToView(ProfilerTickReport tick)
        {
            this.SuspendLayout();
            var mainNode = new TreeNode($"Tick {tick.Tick,-4} ({tick.TickTime,1} ms)");
            mainNode.ForeColor = tick.IsConcerning ? Color.Red : (tick.TickTime <= 0 ? Color.Gray : Color.Black);

            int currentDepth = 0;
            TreeNode previousNode = null;

            foreach (var action in tick.ActionReports)
            {
                TreeNode thisNode = new TreeNode($"{action.Action,-10} - {action.TotalMiliseconds} ms ({(action.TotalMiliseconds / (float)tick.TickTime)*100f: 0.0}%)");

                TreeNode parentNode;

                if (currentDepth == action.Depth)
                {
                    if (previousNode == null)
                    {
                        parentNode = mainNode;
                    }
                    else
                    {
                        parentNode = previousNode.Parent;
                    }
                }
                else if (currentDepth > action.Depth)
                {
                    parentNode = previousNode.Parent;
                    while (currentDepth > action.Depth)
                    {
                        parentNode = parentNode.Parent;
                        currentDepth--;
                    }
                }
                else if (currentDepth == action.Depth - 1)
                {
                    parentNode = previousNode;
                }
                else
                {
                    throw new Exception();
                }

                reportsPerNodes[thisNode] = action;

                int i = 0;

                while (i < parentNode.Nodes.Count && action.TotalMiliseconds < reportsPerNodes[parentNode.Nodes[i]].TotalMiliseconds)
                {
                    i++;
                }

                parentNode.Nodes.Insert(i, thisNode);

                previousNode = thisNode;

                currentDepth = action.Depth;

            }

            TickTreeView.Nodes.Add(mainNode);

            this.ResumeLayout();
        }
    }
}
