using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GTP.Extractors;
using Gtpx.ModelSync.CAD.UI;
using Gtpx.ModelSync.CAD.Utilities;
using Gtpx.ModelSync.DataModel.Models;
using Gtpx.ModelSync.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Document = Autodesk.Revit.DB.Document;
using Form = System.Windows.Forms.Form;

namespace GTP.UI
{
    public partial class GTPDashboard : Form
    {
        private string _version = "2025-04-15";
        Document _document;
        UIDocument _uiDoc; // do UIDocument uidoc = commandData.Application.ActiveUIDocument
        UIApplication _uiApp;
        bool stopProcess = false;
        CancellationTokenSource _source = null;
        CancellationToken _token = CancellationToken.None;

        public GTPDashboard(Document document, UIDocument uidoc, UIApplication uiApp, string version)
        {
            InitializeComponent();
            _document = document;
            _uiDoc = uidoc;
            _uiApp = uiApp;
            progress.Visible = false;
            lblProgress.Visible = false;

#if Revit2025
            Text = $"GTP Model HealthCheck for Revit 2025 v. {_version}";
#elif Revit2024
            Text = $"GTP Model HealthCheck for Revit 2024 v. {_version}";
#elif Revit2023
            Text = $"GTP Model HealthCheck for Revit 2023 v. {_version}";
#elif Revit2022
            Text = $"GTP Model HealthCheck for Revit 2022 v. {_version}";
#elif Revit2021
            Text = $"GTP Model HealthCheck for Revit 2021 v. {_version}";
#elif Revit2020
            Text = $"GTP Model HealthCheck for Revit 2020 v. {_version}";
#elif Revit2019
            Text = $"GTP Model HealthCheck for Revit 2019 v. {_version}";
#else
            Text = $"GTP Model HealthCheck for Revit ???? v. {_version}";
#endif
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            bool success = true;
            tbStart.ForeColor = System.Drawing.Color.Green;
            tbStop.ForeColor = System.Drawing.Color.Green;
            List<ProfilerStats> templateIdRunTimeList = null;
            success = int.TryParse(tbStart.Text, out var start);
            if (success)
            {
                success = int.TryParse(tbStop.Text, out var stop);
                if (success)
                {
                    _source = new CancellationTokenSource();
                    _token = _source.Token;
                    ClearDataGrid();
                    grid.Columns[2].Visible = cbMemory.Checked;
                    progress.Visible = true;
                    lblProgress.Visible = true;
                    tabs.SelectTab(1);
                    RefreshRunTab();
                    LocalFileContext lfc = new LocalFileContext();
                    Notifier notifier = new Notifier(lfc, Serilog.Log.Logger); // TO DO: Replace with my own logger
                    notifier.StatsReceived += Notifier_StatsReceived;
                    templateIdRunTimeList = ElementExtractor.Execute(_document, notifier, cbForceGCCollect.Checked, cbHighRefreshRate.Checked, cbMemory.Checked, (int)udProgressInterval.Value, start, stop, _token);
                    progress.Visible = false;
                    lblProgress.Visible = false;
                    UpdateGrid(templateIdRunTimeList);
                    RefreshRunTab();
                }
                else
                {
                    tbStop.ForeColor = System.Drawing.Color.Red;
                    RefreshSettingTab();
                }
            }
            else
            {
                tbStart.ForeColor = System.Drawing.Color.Red;
                RefreshSettingTab();
            }
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
                    lblProgress.Text = $"{DateTime.Now} {e.Progress} of {e.Total} {e.MoreData ?? string.Empty}";
                }));
            }
            else
            {
                lblProgress.Text = $"{DateTime.Now} {e.Progress} of {e.Total} {e.MoreData ?? string.Empty}";
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

        private string Seconds(long ms)
        {
            return $"{(ms + 650L) / 1000L}"; // add 650 to round up
        }

        private void UpdateGrid(List<ProfilerStats> TemplateIdRunTimeList, int max = -1)
        {
            if (TemplateIdRunTimeList == null) return;
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
       
        private void UpdateGridRefresh(List<ProfilerStats> TemplateIdRunTimeList, int max = -1)
        {
            if (TemplateIdRunTimeList == null) return;
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
                        grid.Rows[i].Cells[0].Value = Seconds(TemplateIdRunTimeList[i].Milliseconds);
                        grid.Rows[i].Cells[1].Value = $"{TemplateIdRunTimeList[i].HitCount:n0}";
                        grid.Rows[i].Cells[2].Value = TemplateIdRunTimeList[i].Memory < 0 ? "0" : $"{TemplateIdRunTimeList[i].Memory:n0}";
                        grid.Rows[i].Cells[3].Value = TemplateIdRunTimeList[i].ParameterCount;
                        grid.Rows[i].Cells[4].Value = String.Join(",", TemplateIdRunTimeList[i].ElementIds);
                        grid.Rows[i].Cells[5].Value = TemplateIdRunTimeList[i].Key;
                    }));
                }
                else
                {
                    grid.Rows[i].Cells[0].Value = Seconds(TemplateIdRunTimeList[i].Milliseconds);
                    grid.Rows[i].Cells[1].Value = $"{TemplateIdRunTimeList[i].HitCount:n0}";
                    grid.Rows[i].Cells[2].Value = TemplateIdRunTimeList[i].Memory < 0 ? "0" : $"{TemplateIdRunTimeList[i].Memory:n0}";
                    grid.Rows[i].Cells[3].Value = TemplateIdRunTimeList[i].ParameterCount;
                    grid.Rows[i].Cells[4].Value = String.Join(",", TemplateIdRunTimeList[i].ElementIds);
                    grid.Rows[i].Cells[5].Value = TemplateIdRunTimeList[i].Key;
                }
            }

            grid.Update();
            grid.Refresh();
        }

        private void UpdateGridCreate(List<ProfilerStats> TemplateIdRunTimeList, int max=-1)
        {
            if (TemplateIdRunTimeList == null) return;
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

                        grid.Rows.Add(
                            Seconds(TemplateIdRunTimeList[i].Milliseconds),
                            $"{TemplateIdRunTimeList[i].HitCount:n0}",
                            TemplateIdRunTimeList[i].Memory < 0 ? "0" : $"{TemplateIdRunTimeList[i].Memory:n0}",
                            TemplateIdRunTimeList[i].ParameterCount > 0 ? $"{TemplateIdRunTimeList[i].ParameterCount}" : string.Empty,
                            String.Join(",", TemplateIdRunTimeList[i].ElementIds),
                            TemplateIdRunTimeList[i].Key);
                    }));
                }
                else
                {
                    grid.Rows.Add(
                        Seconds(TemplateIdRunTimeList[i].Milliseconds),
                        $"{TemplateIdRunTimeList[i].HitCount:n0}",
                        TemplateIdRunTimeList[i].Memory < 0 ? "0" : $"{TemplateIdRunTimeList[i].Memory:n0}",
                        TemplateIdRunTimeList[i].ParameterCount > 0 ? $"{TemplateIdRunTimeList[i].ParameterCount}" : string.Empty,
                        String.Join(",", TemplateIdRunTimeList[i].ElementIds),
                        TemplateIdRunTimeList[i].Key);
                }
            }

            grid.Update();
            grid.Refresh();
        }

        private void ClearDataGrid()
        {
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

        private void RefreshSettingTab()
        {
            if (tabRun.InvokeRequired)
            {
                tabRun.Invoke(new Action(() =>
                {
                    tabSettings.Update();
                    tabSettings.Refresh();
                }));
            }
            else
            {
                tabSettings.Update();
                tabSettings.Refresh();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_token != CancellationToken.None && _source != null && _token.CanBeCanceled)
                _source.Cancel();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_token != CancellationToken.None && _source != null && _token.CanBeCanceled)
                  _source.Cancel();
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// If user clicks in the cell, we load the first element id into the Revit view
        /// </summary>
        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = grid.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];
            var cellValue = cell.Value.ToString();
            if (string.IsNullOrEmpty(cellValue))
            {
                return;
            }
            var cellValueList = cellValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (cellValueList.Length == 0)
            {
                return;
            }
            if (long.TryParse(cellValueList[0], out var id))
            {

#if Revit2024 || Revit2025
                var element = new ElementId(id);
#else
                var element = new ElementId((int)id);
#endif
                var el = _document.GetElement(element);
                if (el == null)
                {
                    return;
                }
                var ids = new List<ElementId>() { element }; 
                _uiDoc.ShowElements(ids);
            }
        }
    }
}
