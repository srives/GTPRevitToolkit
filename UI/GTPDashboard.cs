﻿using Autodesk.Revit.Creation;
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
        public GTPDashboard(Document document, string version)
        {
            InitializeComponent();
            _document = document;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            tabs.SelectTab(1);
            LocalFileContext lfc = new LocalFileContext();
            Notifier notifier = new Notifier(lfc, Serilog.Log.Logger); // TO DO: Replace with my own logger
            notifier.StatsReceived += Notifier_StatsReceived;
            ElementExtractor.Execute(_document, notifier);
        }

        private void Notifier_StatsReceived(object sender, NotificationEventArgs e)
        {
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
                    progress.Value = (int)e.Progress;
                }));
            }
            else
            {
                progress.Value = (int)e.Progress;
            }

            var answer = string.Empty;
            foreach (var item in e.TemplateIdRunTimeList)
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
    }
}
