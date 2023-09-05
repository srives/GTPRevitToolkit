using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using GTP.Extractors;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Document = Autodesk.Revit.DB.Document;
using Form = System.Windows.Forms.Form;

namespace GTP.UI
{
    public partial class GTPDashboard : Form
    {
        Document _document;
        bool stopProcess = false;

        public GTPDashboard(Document document, string version)
        {
            InitializeComponent();
            _document = document;
            progress.Visible = false;
            lblProgress.Visible = false;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            tabs.SelectTab(1);
            progress.Visible = true;
            lblProgress.Visible = true;
            RefreshRunTab();

            LocalFileContext lfc = new LocalFileContext();
            Notifier notifier = new Notifier(lfc, Serilog.Log.Logger); // TO DO: Replace with my own logger

            notifier.StatsReceived += Notifier_StatsReceived;
            var templateIdRunTimeList = ElementExtractor.Execute(_document, notifier);
            progress.Visible = false;
            lblProgress.Visible = false;
            UpdateGrid(templateIdRunTimeList);
            // UpdateRTF(templateIdRunTimeList);
            RefreshRunTab();
        }

        private void Notifier_StatsReceived(object sender, NotificationEventArgs e)
        {
            if (stopProcess)
            {
                return;
            }

            if (lblProgress.InvokeRequired)
            {
                lblProgress.Invoke(new Action(() =>
                {
                    lblProgress.Text = $"{e.Progress} of {e.Total}";
                }));
            }
            else
            {
                lblProgress.Text = $"{e.Progress} of {e.Total}";
            }

            if (progress.InvokeRequired)
            {
                progress.Invoke(new Action(() =>
                {
                    if (progress.Maximum != e.Total) progress.Maximum = (int)e.Total;
                    progress.Value = (int)e.Progress;
                }));
            }
            else
            {
                if (progress.Maximum != e.Total) progress.Maximum = (int)e.Total;
                progress.Value = (int)e.Progress;
            }

            UpdateGrid(e.TemplateIdRunTimeList, 60);
            RefreshRunTab();
        }
        private void UpdateGrid(List<KeyValuePair<string, long>> TemplateIdRunTimeList, int max = -1)
        {
            if (grid.Rows.Count >= max && max > 0)
            {
                UpdateGridRefresh(TemplateIdRunTimeList, max);
            }
            else if (grid.Rows.Count >= TemplateIdRunTimeList.Count)
            {
                UpdateGridRefresh(TemplateIdRunTimeList, max);
            }
            else // makes new rows, to expand the grid to hold more data
            {
                UpdateGridCreate(TemplateIdRunTimeList, max);
            }
        }

        private void UpdateGridRefresh(List<KeyValuePair<string, long>> TemplateIdRunTimeList, int max = -1)
        {
            var ct = TemplateIdRunTimeList.Count;
            if (ct > max && max > 0)
            {
                ct = max;
            }

            for (int i = 0; i < ct; i++)
            {
                if (grid.InvokeRequired)
                {
                    grid.Invoke(new Action(() =>
                    {
                        grid.Rows[i].Cells[0].Value = $"{TemplateIdRunTimeList[i].Value}";
                        grid.Rows[i].Cells[1].Value = TemplateIdRunTimeList[i].Key;
                    }));
                }
                else
                {
                    grid.Rows[i].Cells[0].Value = $"{TemplateIdRunTimeList[i].Value}";
                    grid.Rows[i].Cells[1].Value = TemplateIdRunTimeList[i].Key;
                }
            }

            grid.Update();
            grid.Refresh();
        }

        private void UpdateGridCreate(List<KeyValuePair<string, long>> TemplateIdRunTimeList, int max=-1)
        {
            var ct = TemplateIdRunTimeList.Count;
            if (ct > max && max > 0)
            {
                ct = max;
            }

            if (grid.InvokeRequired)
            {
                grid.Invoke(new Action(() =>
                {
                    grid.Rows.Clear();
                }));
            }
            else
            {
                grid.Rows.Clear();
            }

            for (int i = 0; i < ct; i++)
            {
                if (grid.InvokeRequired)
                {
                    grid.Invoke(new Action(() =>
                    {
                        grid.Rows.Add($"{TemplateIdRunTimeList[i].Value}", TemplateIdRunTimeList[i].Key);
                    }));
                }
                else
                {
                    grid.Rows.Add($"{TemplateIdRunTimeList[i].Value}", TemplateIdRunTimeList[i].Key);
                }
            }

            grid.Update();
            grid.Refresh();
        }

        /*
        private void UpdateRTF(List<KeyValuePair<string, long>> TemplateIdRunTimeList)
        {
            var answer = string.Empty;
            foreach (var item in TemplateIdRunTimeList)
            {
                answer += $"{item.Value} sec, {item.Key}{Environment.NewLine}";
            }

            if (rtbResults.InvokeRequired)
            {
                rtbResults.Invoke(new Action(() =>
                {
                    rtbResults.Text = answer;
                }));
            }
            else
            {
                rtbResults.Text = answer;
            }
        }
        */
        private void RefreshRunTab()
        {
            if (tabRun.InvokeRequired)
            {
                tabRun.Invoke(new Action(() =>
                {
                    tabRun.Update();
                    tabRun.Refresh();
                }));
            }
            else
            {
                tabRun.Update();
                tabRun.Refresh();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopProcess = true;
        }
    }
}
